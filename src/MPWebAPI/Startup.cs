using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MPWebAPI.Models;
using Swashbuckle.Swagger.Model;

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

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(
                options => 
                options.SingleApiVersion(
                    new Info()
                    {
                        Version = "v1",
                        Title = "Merlin: Plan Web API",
                        Description = "Provides persistance storage and business logic services to Merlin: Plan",
                        TermsOfService = "None",
                        Contact = new Contact() {
                            Name = "Sam Win-Mason",
                            Email = "sam@lemonadelabs.io",
                            Url = "http://lemonadelabs.io"
                        }
                    }
                )
            );

            services.AddOpenIddict<MerlinPlanUser, PostgresDBContext>()
                .EnableTokenEndpoint("/api/auth/token")
                .EnableLogoutEndpoint("/api/auth/logout")
                .UseJsonWebTokens()
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .DisableHttpsRequirement()
                .AddEphemeralSigningKey();

            services.AddScoped<IMerlinPlanRepository, MerlinPlanRepository>();
            services.AddSingleton<IMerlinPlanBL, MerlinPlanBL>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIdentity();
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
            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
