using CryptoHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenIddict;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Models
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

            // Create default application
            if (!_context.Applications.Any())
            {
                CreateApplication();
            }

            // Seed the roles
            if (!_context.Roles.Any())
            {
                await CreateRoles();
            }            // Seed the users
            if (!_context.Users.Any())
            {
                CreateUsers();
            }
            // Seed the leaves
            if (!_context.Leaves.Any())
            {
                CreateLeaves();
            }

            // Actually save the changes to the db
            await _context.SaveChangesAsync();

        }

        private void CreateApplication()
        {
            _context.Applications.Add(new OpenIddictApplication
            {
                Id = _config["Authentication:OpenIddict:ApplicationId"],
                DisplayName = _config["Authentication:OpenIddict:DisplayName"],
                RedirectUri = _config["Authentication:OpenIddict:TokenEndPoint"],
                LogoutRedirectUri = "/",
                ClientId = _config["Authentication:OpenIddict:ClientId"],
                ClientSecret = Crypto.HashPassword(_config["Authentication:OpenIddict:ClientSecret"]),
                Type = OpenIddictConstants.ClientTypes.Public
            });
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
            var firstLeave = new Leave()
            {
                DateCreated = DateTime.UtcNow,
                Justification = "first sample leave",
                Means = Means.EMAIL,
                Status = Status.UNFILED,
                User = "" // todo
            };

            _context.Leaves.Add(firstLeave);
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

            if (await _userManager.FindByIdAsync(user.Id) == null)
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
