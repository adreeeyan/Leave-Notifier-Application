using AutoMapper;
using LeaveNotifierApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using LeaveNotifierApplication.Api.Models;
using LeaveNotifierApplication.Data.Models;

namespace LeaveNotifierApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger;
        private IMapper _mapper;
        private ILeaveNotifierRepository _repo;

        public UsersController(ILeaveNotifierRepository repo,
            ILogger<UsersController> logger,
            IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all the users
        /// </summary>
        [Authorize(Policy = "SuperUsers")]
        [HttpGet]
        public IActionResult Get([FromQuery] QueryModel<LeaveNotifierUser> query)
        {
            try
            {
                var items = QueryModel<LeaveNotifierUser>.Query(_repo.GetAllUsers(), query);
                var users = _mapper.Map<IEnumerable<LeaveNotifierUserModel>>(items);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured when getting all users: {ex}");
            }
            return BadRequest();
        }

        /// <summary>
        /// Retrieves a specific user
        /// </summary>
        [Authorize(Policy = "SuperUsers")]
        [HttpGet("{userName}")]
        public IActionResult Get(string userName)
        {
            try
            {
                var user = _mapper.Map<LeaveNotifierUserModel>(_repo.GetUserByUserName(userName));
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured when getting user: {ex}");
            }
            return BadRequest($"Cannot get user ({userName}) information.");
        }
    }
}
