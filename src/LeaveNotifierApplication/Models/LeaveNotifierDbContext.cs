using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenIddict;

namespace LeaveNotifierApplication.Models
{
    public class LeaveNotifierDbContext: OpenIddictDbContext<LeaveNotifierUser>
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
