using LeaveNotifierApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger;
        private ILeaveNotifierRepository _repo;

        public UsersController(ILeaveNotifierRepository repo,
            ILogger<UsersController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public IActionResult Get()
        {
            try
            {
                var users = _repo.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured when getting all users: {ex}");
            }
            return BadRequest();
        }
    }
}
