using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using VSSApi.Mapping;

namespace VSSApi
{
    public static class ServiceRegistration
    {
        public static void AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<MappingProfile>();
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);




            #region Swagger
            services.AddSwaggerGen(gen =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Jwt Bearer Token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                var vibeBilisimLink = "http://vibebilisim.com/";

                gen.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "VSS Wep Api",
                    Version = "v1",
                    License = new OpenApiLicense
                    {
                        Name = "Powered by VibeBilisim",
                        Url = new Uri(vibeBilisimLink),
                    },
                    Contact = new OpenApiContact
                    {
                        Name = "Safa Uludoğan",
                        Email = "safa.uludogan@vibebilisim.com.tr"
                    },


                });

                gen.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                gen.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
                    {securityScheme, Array.Empty<string>()}
                });

            });




            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Admin", opt =>
                {
                    opt.TokenValidationParameters = new()
                    {
                        ValidateAudience = true, // Oluşturulacak token değerinin kimlerin/hangi originlerin/sitelerin kullanıcı belirlediğimiz değerdir.
                        ValidateIssuer = true, // Oluşturulacak token değerinin kimin dağıttığını ifade edeceğimiz alan
                        ValidateLifetime = true, // Oluşturulan token değerinin süresini kontrol edecek olan doğrulama
                        ValidateIssuerSigningKey = true, // Üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden suciry key verisinin doğrulamasıdır.

                        ValidAudience = configuration["Token:Audience"],
                        ValidIssuer = configuration["Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])),
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

                        NameClaimType = ClaimTypes.Name //JWT üzerinde Name claimne karşılık gelen değeri User.Identity.Name propertysinden elde edebiliriz.
                    };
                });
            #endregion
        }
    }
}
