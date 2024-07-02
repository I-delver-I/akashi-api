using System.Text;
using AkaShi.Core.Auth;
using AkaShi.Core.DTO.Auth;
using AkaShi.Core.DTO.Library;
using AkaShi.Core.DTO.LibraryVersion;
using AkaShi.Core.DTO.User;
using AkaShi.Core.Logic.Abstractions;
using AkaShi.WebAPI.Logic;
using AkaShi.WebAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AkaShi.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddWebApi(this IServiceCollection services)
    {
        services.AddScoped<UserDataStorage>();
        services.AddTransient<IUserDataSetter>(s => s.GetService<UserDataStorage>());
        services.AddTransient<IUserDataGetter>(s => s.GetService<UserDataStorage>());
    }
    
    public static void RegisterCustomValidators(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<RevokeRefreshTokenDTO>, RevokeRefreshTokenDTOValidator>();
        services.AddSingleton<IValidator<RefreshTokenDTO>, RefreshTokenDTOValidator>();
        services.AddSingleton<IValidator<UserRegisterDTO>, UserRegisterDTOValidator>();
        services.AddSingleton<IValidator<UserLoginDTO>, UserLoginDTOValidator>();
        services.AddSingleton<IValidator<NewLibraryVersionDTO>, NewLibraryVersionDTOValidator>();
        services.AddSingleton<IValidator<NewLibraryDTO>, NewLibraryDTOValidator>();
    }
    
    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["SecretJWTKey"]; // get value from system environment
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        // jwt wire up
        // Get options from app settings
        var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

        // Configure JwtIssuerOptions
        services.Configure<JwtIssuerOptions>(options =>
        {
            options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
            options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

            ValidateAudience = true,
            ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            RequireExpirationTime = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(configureOptions =>
        {
            configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            configureOptions.TokenValidationParameters = tokenValidationParameters;
            configureOptions.SaveToken = true;

            configureOptions.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }
    
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "AkaShi",
                Description = "An internet registry of program libraries for C#."
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                Description = "Enter 'Bearer' [space] and then your token in the text input below. " +
                              "<br/><b>Example:</b> 'Bearer 12345abcdef'",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });

            /*var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));*/
        });
    }
    
    public static void ConfigureCustomValidationErrors(this IServiceCollection services)
    {
        // override modelstate
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                    .SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
                var result = new
                {
                    Message = "Validation errors",
                    Errors = errors
                };

                return new BadRequestObjectResult(result);
            };
        });
    }
}