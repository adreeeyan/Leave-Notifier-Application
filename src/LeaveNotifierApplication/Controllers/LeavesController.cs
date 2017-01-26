﻿using LeaveNotifierApplication.Data;
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
        private ILogger<LeavesController> _logger;
        private ILeaveNotifierRepository _repo;

        public LeavesController(ILeaveNotifierRepository repo,
            ILogger<LeavesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var leaves = _repo.GetAllLeaves();
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception while getting all Leaves: {ex}");
            }
            return BadRequest();
        }
    }
}
