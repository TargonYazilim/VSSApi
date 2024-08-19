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

                gen.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "API Key must appear in the header",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "X-Api-Key",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });

                gen.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            },
                            Scheme = "ApiKeyScheme",
                            Name = "ApiKey",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,/// Tokendaki verilerin doğruluğunu kontrol et 
                        ValidateAudience = true,/// Tokendaki verilerin doğruluğunu kontrol et 
                        ValidateLifetime = true,/// Token'ın süresinin geçip geçimediğini kontrol et.
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty)),

                        NameClaimType = ClaimTypes.NameIdentifier //JWT üzerinde Name claimne karşılık gelen değeri User.Identity.Name propertysinden elde edebiliriz.
                    };
                });
            #endregion
        }
    }
}
