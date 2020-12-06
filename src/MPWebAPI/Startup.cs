using System;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MPWebAPI.Models;
using MPWebAPI.Fixtures;
using MPWebAPI.Services;
using Newtonsoft.Json;
using AspNet.Security.OpenIdConnect.Server;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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
            //var sqlConnectionString = Configuration["DevDBConnectionString"];
            var sqlConnectionString = Configuration.GetSection("DevDB").GetValue<string>("ConnectionString") ;
            
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

            services.AddDbContext<DBContext>(
			options => {
				options.UseNpgsql(sqlConnectionString);
				options.UseOpenIddict();
				
			});

			            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
			
			// Register the OpenIddict services.
			services.AddOpenIddict( )

				// Register the OpenIddict core services.
				.AddCore(options =>
				{
					options.UseEntityFrameworkCore()
						   .UseDbContext<DbContext>();


				})

				.AddServer(options =>
                        {
                        /*
                             .AddMvcBinders()
                                        .EnableTokenEndpoint("/api/auth/token")
                                        .UseJsonWebTokens()
                                        .AllowPasswordFlow()
                                        .AllowRefreshTokenFlow()
                                        .DisableHttpsRequirement()
                                        .SetAccessTokenLifetime(TimeSpan.FromSeconds(3600))
                                        .AddEphemeralSigningKey();
                                        */
                            // Register the ASP.NET Core MVC binder used by OpenIddict.
                            // Note: if you don't call this method, you won't be able to
                            // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                           // options.UseMvc();

                            // Enable the token endpoint (required to use the password flow).
                            options.EnableTokenEndpoint("/api/auth/token");

                            // Allow client applications to use the grant_type=password flow.
                            options.AllowPasswordFlow();

                            // During development, you can disable the HTTPS requirement.
                            options.DisableHttpsRequirement();


                         // AddMvcBinders() is now UseMvc().
                               // options.UseMvc();
                          options .UseJsonWebTokens();
                          //  .AllowPasswordFlow()
                         // options  .AllowRefreshTokenFlow()
                         //  options.DisableHttpsRequirement()
                          //  options.SetAccessTokenLifetime(TimeSpan.FromSeconds(3600))
                            options.AddEphemeralSigningKey();
                        });

                        //.AddMvcBinders()

                       // .AddValidation();

				
						
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
           // app.UseOpenIddict();


            app.UseJwtBearerAuthentication(
                new JwtBearerOptions
                {
                   // AuthenticationScheme = OpenIdConnectServerDefaults.AuthenticationScheme,

                     AuthenticationScheme =  JwtBearerDefaults.AuthenticationScheme,
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true,
                    RequireHttpsMetadata = false,
                    Audience = "http://localhost:5000/",
                    Authority = "http://localhost:5000/"
                }
            );




            app.UseExceptionHandler("/api/error");
            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                if (context.HttpContext.Response.StatusCode == 404)
                {
                    await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject("The resource cannot be found yo! (404)"), Encoding.UTF8);
                }
                if (context.HttpContext.Response.StatusCode == 500)
                {
                    await context.HttpContext.Response.WriteAsync(
                        JsonConvert.SerializeObject(
                            "Oh Noes! Your request could not be served because things went explody! (500 Talk to Sam)"), Encoding.UTF8);
                }
            });

           // app.UseOpenIddictServer();
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
