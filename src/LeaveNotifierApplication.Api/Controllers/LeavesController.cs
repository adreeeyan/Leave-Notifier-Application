using AutoMapper;
using LeaveNotifierApplication.Data;
using LeaveNotifierApplication.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using LeaveNotifierApplication.Api.Filters;
using Microsoft.AspNetCore.Identity;
using LeaveNotifierApplication.Data.Models;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Api.Controllers
{
    [Route("api")]
    [Authorize]
    [ValidateModel]
    public class LeavesController : Controller
    {
        private ILogger<LeavesController> _logger;
        private IMapper _mapper;
        private ILeaveNotifierRepository _repo;
        private UserManager<LeaveNotifierUser> _userMgr;

        public LeavesController(ILeaveNotifierRepository repo,
            UserManager<LeaveNotifierUser> userMgr,
            ILogger<LeavesController> logger,
            IMapper mapper)
        {
            _repo = repo;
            _userMgr = userMgr;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve all the leaves
        /// </summary>
        [HttpGet("[controller]")]
        public IActionResult Get([FromQuery] QueryModel<Leave> query)
        {
            try
            {
                var items = QueryModel<Leave>.Query(_repo.GetAllLeaves(), query);
                var leaves = _mapper.Map<IEnumerable<LeaveModel>>(items);
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while getting all Leaves: {ex}");
            }
            return BadRequest();
        }

        /// <summary>
        /// Retrieve a specific leave
        /// </summary>
        [HttpGet("[controller]/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var leave = _mapper.Map<LeaveModel>(_repo.GetLeaveById(id));
                return Ok(leave);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while getting leave: {ex}");
            }

            return BadRequest($"Cannot get leave {id}");
        }

        /// <summary>
        /// Retrieves all the leaves from a specific user
        /// </summary>
        [HttpGet("users/{userName}/[controller]")]
        public async Task<IActionResult> GetLeavesByUserName(string userName, [FromQuery] QueryModel<Leave> query)
        {
            try
            {
                // Check first if userName is really a user
                var user = await _userMgr.FindByNameAsync(userName);
                if (user == null)
                {
                    return NotFound($"User {userName} not found");
                }
                var items = QueryModel<Leave>.Query(_repo.GetLeavesByUserName(userName), query);
                var leaves = _mapper.Map<IEnumerable<LeaveModel>>(items);
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while getting leaves of user: {ex}");
            }

            return BadRequest($"Cannot get leaves of {userName}");
        }

    }
}
