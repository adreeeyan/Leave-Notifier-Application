using System.Runtime.InteropServices;
using LeaveNotifierApplication.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LeaveNotifierApplication.Data
{
    public class LeaveNotifierDbContext : IdentityDbContext<LeaveNotifierUser>
    {
        private IConfigurationRoot _config;

        public LeaveNotifierDbContext(IConfigurationRoot config, DbContextOptions<LeaveNotifierDbContext> options) : base(options)
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
