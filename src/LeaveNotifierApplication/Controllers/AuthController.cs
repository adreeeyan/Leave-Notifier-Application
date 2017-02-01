using LeaveNotifierApplication.Data;
using LeaveNotifierApplication.Data.Models;
using LeaveNotifierApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Controllers
{
    // We won't use this controller, OpenIddict provides what we need
    // [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private LeaveNotifierDbContext _context;
        private ILogger<AuthController> _logger;
        private SignInManager<LeaveNotifierUser> _signInMgr;
        private UserManager<LeaveNotifierUser> _userMgr;
        private IPasswordHasher<LeaveNotifierUser> _hasher;
        private IConfigurationRoot _config;

        public AuthController(LeaveNotifierDbContext context,
            SignInManager<LeaveNotifierUser> signInMgr,
            UserManager<LeaveNotifierUser> userMgr,
            IPasswordHasher<LeaveNotifierUser> hasher,
            IConfigurationRoot config,
            ILogger<AuthController> logger)
        {
            _context = context;
            _signInMgr = signInMgr;
            _userMgr = userMgr;
            _hasher = hasher;
            _config = config;
            _logger = logger;
        }

        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        {
            try
            {
                var user = await _userMgr.FindByNameAsync(model.UserName);
                if(user != null)
                {
                    if(_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var userClaims = await _userMgr.GetClaimsAsync(user);

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        }.Union(userClaims);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: _config["Tokens:Issuer"],
                            audience: _config["Tokens:Audience"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(15),
                            signingCredentials: creds
                        );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating JWT: {ex}");
            }

            return BadRequest("Cannot create JWT.");
        }
    }
}
