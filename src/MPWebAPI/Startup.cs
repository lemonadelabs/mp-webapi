using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MPWebAPI.Models;
using MPWebAPI.Fixtures;
using MPWebAPI.Services;

namespace MPWebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            
            if (env.IsDevelopment())
            {
                builder = builder.AddUserSecrets("io.lemonadelabs.mp-webapi");
            }

            builder = builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            HostingEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = Configuration["DevDBConnectionString"];
            
            services.AddOptions();

            services.Configure<MerlinPlanBLOptions>(
                Configuration.GetSection("BusinessRules"));

            services.Configure<AuthMessageSenderOptions>(options => 
                {
                    options.SendGridAPIKey = Configuration["SendGridAPIKey"];
                    options.SendGridUser = Configuration["SendGridUser"];
                    options.UrlHost = (HostingEnvironment.IsDevelopment()) ? 
                        Configuration.GetSection("FrontendHost").GetValue<string>("development") : 
                        Configuration.GetSection("FrontendHost").GetValue<string>("production");
                } 
            );

            services.AddDbContext<DBContext>(options => options.UseNpgsql(sqlConnectionString));
            
            services.AddIdentity<MerlinPlanUser, IdentityRole>(options => 
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Lockout.AllowedForNewUsers = true;
                }
            )
                .AddEntityFrameworkStores<DBContext>()
                .AddDefaultTokenProviders();

            
            services.AddMvc();

            services.AddOpenIddict<DBContext>()
                .AddMvcBinders()
                .EnableTokenEndpoint("/api/auth/token")
                .UseJsonWebTokens()
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .DisableHttpsRequirement()
                .SetAccessTokenLifetime(TimeSpan.FromSeconds(3600))
                .AddEphemeralSigningKey();

            services.AddScoped<IMerlinPlanRepository, MerlinPlanRepository>();
            services.AddTransient<IFixtureBuilder, FixtureBuilder>();
            services.AddScoped<IMerlinPlanBL, MerlinPlanBL>();
            services.AddSingleton<IEmailSender, AuthMessageSender>();
        }

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
            
           
            var fixtureConfig = Configuration.GetSection("Fixtures");
            if (!fixtureConfig.GetValue<bool>("Enabled")) return;
            var fixtureBuilder =  app.ApplicationServices.GetService<IFixtureBuilder>();
            fixtureBuilder.AddFixture(
                fixtureConfig.GetValue<string>("Fixture"),
                fixtureConfig.GetValue<bool>("FlushDB")
            );
        }
    }
}
