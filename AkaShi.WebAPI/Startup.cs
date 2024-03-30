using System.Text.Json.Serialization;
using AkaShi.Core.Extensions;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Extensions;
using AkaShi.WebAPI.Extensions;
using AkaShi.WebAPI.Filters;
using AkaShi.WebAPI.Middlewares;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AkaShi.WebAPI;

public class Startup
{
    private IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var migrationAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration["ConnectionStrings:AkaShiDBConnection"],
                opt => opt.MigrationsAssembly(migrationAssembly)));

        services.RegisterAutoMapper();
        
        services.RegisterCustomValidators();
        
        services.AddInfrastructure();
        services.AddCore();
        services.AddWebApi();

        services.ConfigureJwt(Configuration);
        services.AddCors();
        
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddMvcCore(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
            .AddAuthorization();
        
        services.ConfigureCustomValidationErrors();
        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            options.JsonSerializerOptions.MaxDepth = 300;
        });

        /*builder.Services.AddControllers();*/

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services
            .AddEndpointsApiExplorer()
            .AddMvc();
        services.ConfigureSwagger();        
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });

        app.UseCors(builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Token-Expired")
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition")
            .WithOrigins("http://localhost:3000"));
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<UserDataSaverMiddleware>();
        
        app.UseEndpoints(cfg =>
        {
            cfg.MapControllers();
        });
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        
        InitializeDatabase(app);
    }
    
    private static void InitializeDatabase(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
}