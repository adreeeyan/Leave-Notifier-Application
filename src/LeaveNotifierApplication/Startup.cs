using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LeaveNotifierApplication.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LeaveNotifierApplication.Data.Models;

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
            services.AddDbContext<LeaveNotifierDbContext>(ServiceLifetime.Scoped)
                .AddIdentity<LeaveNotifierUser, IdentityRole>();
            services.AddScoped<ILeaveNotifierRepository, LeaveNotifierRepository>();
            services.AddTransient<LeaveNotifierDbContextSeedData>();

            // Add the Identity
            services.AddIdentity<LeaveNotifierUser, IdentityRole>()
                .AddEntityFrameworkStores<LeaveNotifierDbContext>();

            // Add framework services.
            services.AddMvc();

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

            app.UseMvc();

            seeder.EnsureSeedData().Wait();
        }
    }
}
