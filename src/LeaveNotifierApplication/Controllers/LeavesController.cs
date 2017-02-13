using AutoMapper;
using LeaveNotifierApplication.Data;
using LeaveNotifierApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using LeaveNotifierApplication.Filters;
using Microsoft.AspNetCore.Identity;
using LeaveNotifierApplication.Data.Models;
using LeaveNotifierApplication.Data.Extensions;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Controllers
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

        [HttpGet("[controller]")]
        public IActionResult Get([FromQuery] QueryModel query)
        {
            try
            {
                var items = _repo.GetAllLeaves().SortBy(query.SortOrder, query.IsAsc).Where(query.SearchKey, query.SearchValue, query.IsFull);
                var leaves = _mapper.Map<IEnumerable<LeaveModel>>(items);
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while getting all Leaves: {ex}");
            }
            return BadRequest();
        }

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

        [HttpGet("users/{userName}/[controller]")]
        public async Task<IActionResult> GetLeavesByUserName(string userName, [FromQuery] QueryModel query)
        {
            try
            {
                // Check first if userName is really a user
                var user = await _userMgr.FindByNameAsync(userName);
                if (user == null)
                {
                    return NotFound($"User {userName} not found");
                }
                var items = _repo.GetLeavesByUserName(userName).SortBy(query.SortOrder, query.IsAsc).Where(query.SearchKey, query.SearchValue, query.IsFull);
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
