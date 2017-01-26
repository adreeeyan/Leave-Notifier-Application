using LeaveNotifierApplication.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Data
{
    public class LeaveNotifierDbContextSeedData
    {
        private LeaveNotifierDbContext _context;
        private UserManager<LeaveNotifierUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfigurationRoot _config;

        public LeaveNotifierDbContextSeedData(LeaveNotifierDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<LeaveNotifierUser> userManager,
            IConfigurationRoot config)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
        }

        public async Task EnsureSeedData()
        {
            // Create the Db if it doesn't exist
            _context.Database.EnsureCreated();

            // Seed the roles
            if (!_context.Roles.Any())
            {
                await CreateRoles();
            }

            // Seed the users
            if (!_context.Users.Any())
            {
                await CreateUsers();
            }

            // Seed the leaves
            if (!_context.Leaves.Any())
            {
                CreateLeaves();
            }

            // Actually save the changes to the db
            await _context.SaveChangesAsync();

        }

        private async Task CreateRoles()
        {
            await CreateRole(Role.ADMINISTRATOR);
            await CreateRole(Role.REGISTERED);
        }

        private async Task CreateUsers()
        {
            // Create the admin
            await CreateUser("admin", "P@ssw0rd", Role.ADMINISTRATOR);

            // Create normal users
            await CreateUser("user1", "P@ssw0rd", Role.REGISTERED);
            await CreateUser("user2", "P@ssw0rd", Role.REGISTERED);
            await CreateUser("user3", "P@ssw0rd", Role.REGISTERED);
        }

        private void CreateLeaves()
        {
            var user1 = (LeaveNotifierUser)_context.Users.First(user => user.UserName == "user1");
            var firstLeave = new Leave()
            {
                DateCreated = DateTime.UtcNow,
                Justification = "first sample leave",
                Means = Means.EMAIL,
                Status = Status.UNFILED,
                User = user1
            };

            var user2 = (LeaveNotifierUser)_context.Users.First(user => user.UserName == "user2");
            var secondLeave = new Leave()
            {
                DateCreated = DateTime.UtcNow,
                Justification = "second sample leave",
                Means = Means.EMAIL,
                Status = Status.UNFILED,
                User = user2
            };
            
            var thirdLeave = new Leave()
            {
                DateCreated = DateTime.UtcNow,
                Justification = "third sample leave",
                Means = Means.SMS,
                Status = Status.FILED,
                User = user1
            };

            _context.Leaves.AddRange(firstLeave, secondLeave, thirdLeave);
        }

        private async Task CreateUser(string username, string password, string role)
        {
            DateTime dateCreated = DateTime.Now;
            DateTime lastModifiedDate = DateTime.Now;

            var user = new LeaveNotifierUser()
            {
                UserName = username,
                Email = username + "@kyoceraleaves.com",
                CreatedDate = dateCreated,
                LastModifiedDate = lastModifiedDate
            };

            if (await _userManager.FindByIdAsync(user.UserName) == null)
            {
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, role);
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
            }
        }

        private async Task CreateRole(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
