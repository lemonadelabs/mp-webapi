using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MPWebAPI.Models;

namespace MPWebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = Configuration["DevDB:ConnectionString"];
            
            services.AddDbContext<PostgresDBContext>(options => options.UseNpgsql(sqlConnectionString));
            
            services.AddIdentity<MerlinPlanUser, IdentityRole>()
                .AddEntityFrameworkStores<PostgresDBContext>()
                .AddDefaultTokenProviders();
            
            services.AddMvc();

            services.AddOpenIddict<MerlinPlanUser, PostgresDBContext>()
                .EnableTokenEndpoint("/api/connect/token")
                .EnableLogoutEndpoint("/api/connect/logout")
                .UseJsonWebTokens()
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .DisableHttpsRequirement()
                .AddEphemeralSigningKey();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseOAuthValidation();
            app.UseOpenIddict();
            app.UseJwtBearerAuthentication(
                new JwtBearerOptions()
                {
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true,
                    RequireHttpsMetadata = false,
                    Audience = "http://localhost:5000/",
                    Authority = "http://localhost:5000/"
                }
            );
            app.UseMvc();
        }
    }
}
