using System.Runtime.InteropServices;
using LeaveNotifierApplication.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenIddict;

namespace LeaveNotifierApplication.Data
{
    public class LeaveNotifierDbContext : OpenIddictDbContext<LeaveNotifierUser>
    {
        private IConfigurationRoot _config;

        public LeaveNotifierDbContext(IConfigurationRoot config, DbContextOptions options) : base(options)
        {
            _config = config;
        }

        public DbSet<Leave> Leaves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                optionsBuilder.UseSqlServer(_config["ConnectionStrings:Windows:LeaveNotifierDbContextConnection"]);
            }
            else
            {
                optionsBuilder.UseSqlite(_config["ConnectionStrings:Linux:LeaveNotifierDbContextConnection"]);
            }
        }

    }
}
