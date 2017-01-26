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
    public class LeavesController : Controller
    {
        private ILeaveNotifierRepository _repo;

        public LeavesController(ILeaveNotifierRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repo.GetAllLeaves());
            }
            catch (Exception)
            {
            }
            return BadRequest();
        }
    }
}
