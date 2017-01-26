using LeaveNotifierApplication.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LeaveNotifierApplication.Data
{
    public class LeaveNotifierDbContext: IdentityDbContext
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

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:LeaveNotifierDbContextConnection"]);
        }

    }
}
