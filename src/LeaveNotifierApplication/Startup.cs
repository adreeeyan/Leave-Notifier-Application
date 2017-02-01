using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LeaveNotifierApplication.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LeaveNotifierApplication.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.Swagger.Model;

namespace LeaveNotifierApplication
{
    public class Startup
    {
        private IHostingEnvironment _env;
        public IConfigurationRoot _config { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _config = builder.Build();

            _env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add the application wide config file
            services.AddSingleton(_config);

            // Add the db stuffs
            services.AddDbContext<LeaveNotifierDbContext>(ServiceLifetime.Scoped);
            services.AddScoped<ILeaveNotifierRepository, LeaveNotifierRepository>();
            services.AddTransient<LeaveNotifierDbContextSeedData>();

            // Add our mapper
            services.AddAutoMapper();

            // Add the Identity
            services.AddIdentity<LeaveNotifierUser, IdentityRole>()
                .AddEntityFrameworkStores<LeaveNotifierDbContext>();

            services.Configure<IdentityOptions>(config =>
            {
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = (ctx) =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }

                        return Task.CompletedTask;
                    },

                    OnRedirectToAccessDenied = (ctx) =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 403;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            // Add authorization configs
            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("SuperUsers", p => p.RequireClaim("SuperUser", "True"));
            });

            // Add framework services.
            services.AddMvc();

            // Register the Swagger generator
            services.AddSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "LeaveNotifier API",
                    Description = "API Usage",
                    TermsOfService = "None"
                });
            });

            // Register the OpenIddict services, including the default Entity Framework stores.
            services.AddOpenIddict<LeaveNotifierUser, LeaveNotifierDbContext>()
            // Integrate with EFCore
            .AddEntityFramework<LeaveNotifierDbContext>()
            // Use Json Web Tokens (JWT)
            .UseJsonWebTokens()
            // Set a custom token endpoint (default is /auth/token)
            .EnableTokenEndpoint("/api/auth/token")
            // Set a custom auth endpoint (default is /auth/authorize)
            .EnableAuthorizationEndpoint("/api/auth/authorize")
            // Set a custom revoker endpoint (default is /auth/revoke)
            .EnableRevocationEndpoint("/api/auth/revoke")
            // Allow client applications to use the grant_type=password flow.
            .AllowPasswordFlow()
            // Enable support for both authorization & implicit flows
            .AllowAuthorizationCodeFlow()
            .AllowImplicitFlow()
            // Allow the client to refresh tokens.
            .AllowRefreshTokenFlow()
            // Disable the HTTPS requirement (not recommended in production)
            .DisableHttpsRequirement()
            // Register a new ephemeral key for development.
            // We will register a X.509 certificate in production.
            .AddEphemeralSigningKey();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            LeaveNotifierDbContextSeedData seeder)
        {
            loggerFactory.AddConsole(_config.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Use the identity
            app.UseIdentity();

            // Use OpenIddict
            app.UseOpenIddict();

            // Middleware for JWT Authentication
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                Authority = "http://localhost:5000",
                TokenValidationParameters = new TokenValidationParameters()
                {
                    // ValidIssuer = _config["Tokens:Issuer"],
                    // ValidAudience = _config["Tokens:Audience"],
                    // ValidateIssuerSigningKey = true,
                    // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"])),
                    ValidateAudience = false,
                    ValidateLifetime = true
                }
            });

            // Enable swagger file
            app.UseSwagger();

            // Enable the swagger UI (this should be dev only)
            if (_env.IsDevelopment())
            {
                app.UseSwaggerUi();
            }

            app.UseMvc();

            seeder.EnsureSeedData().Wait();
        }
    }
}
