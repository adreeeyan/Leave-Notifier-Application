using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LeaveNotifierApplication.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeaveNotifierApplication.Data
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add app wide configuration object
            services.AddSingleton(_config);

            // Add the DB Context
            services.AddDbContext<LeaveNotifierDbContext>(ServiceLifetime.Scoped)
                .AddIdentity<LeaveNotifierUser, IdentityRole>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure()
        {
        }
    }
}
