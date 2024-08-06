using DataAccess;
using Business;
using VSSApi;
using Shared.Models;
using System.Collections.ObjectModel;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using VSSApi.Extension;
using Serilog.Context;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
//policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
policy.WithOrigins("http://localhost:5023", "https://localhost:5023")
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
));


var log = new LoggerConfiguration()
    .WriteTo.File("logs/.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "VB_LOGS",
            AutoCreateSqlTable = true
        },
        columnOptions: new ColumnOptions
        {
            AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn { ColumnName = "Username", DataType = SqlDbType.NVarChar }
            }
        })
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(log);

builder.Host.UseSerilog(log);

builder.Services.AddHttpContextAccessor();//Client'tan gelen request neticesinde oluþturulan HttpContext nesnesine katmanlardaki class'lar üzerinden eriþebilmemizi saðlar. Ve aktif kullanýcý bilgilerini verir

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDataAccess();
builder.Services.Business();
builder.Services.AddApi(builder.Configuration);



var app = builder.Build();


if (app.Environment.IsDevelopment())// Hata kodlarýný yakalamak için.
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
app.UseSwagger();
app.UseSwaggerUI();

//}

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());
app.UseSerilogRequestLogging();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("Username", username);
    await next();
});

app.Services.GenerateDb(); /// Generate db in not exists


app.MapControllers();

app.Run();
