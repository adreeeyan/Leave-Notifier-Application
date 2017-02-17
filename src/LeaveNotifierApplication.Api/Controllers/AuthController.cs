using LeaveNotifierApplication.Data;
using LeaveNotifierApplication.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LeaveNotifierApplication.Api.Models;

namespace LeaveNotifierApplication.Api.Controllers
{
    /// <summary>
    /// Authentication and Authorization API
    /// </summary>
    [Route("api/[controller]")]
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

        /// <summary>
        /// Returns an access token which will be used by the client to authenticate with the server
        /// </summary>
        /// <param name="model">Username and Password in JSON format</param>
        /// <remarks>
        /// Client needs to add the access token returned by this controller.
        /// And any subsequent calls in other controllers must have an Authorization header with a value of bearer
        /// (Example): 
        ///             Header: Authorization
        ///             Value: bearer token
        /// </remarks>
        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        {
            try
            {
                var user = await _userMgr.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
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
                            expires: DateTime.UtcNow.AddHours(1),
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
