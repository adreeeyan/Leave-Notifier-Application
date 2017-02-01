using AutoMapper;
using LeaveNotifierApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using LeaveNotifierApplication.Models;
using LeaveNotifierApplication.Data.Extensions;

namespace LeaveNotifierApplication.Controllers
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

        [Authorize(Policy = "SuperUsers")]
        [HttpGet]
        public IActionResult Get(string[] searchKey, string[] searchValue, bool[] isFull, string sortOrder = "CreatedDate", bool asc = false)
        {
            try
            {
                var items = _repo.GetAllUsers().SortBy(sortOrder, asc).Where(searchKey, searchValue, isFull);
                var users = _mapper.Map<IEnumerable<LeaveNotifierUserModel>>(items);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured when getting all users: {ex}");
            }
            return BadRequest();
        }

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
