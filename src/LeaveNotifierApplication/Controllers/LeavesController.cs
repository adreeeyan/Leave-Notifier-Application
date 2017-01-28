using AutoMapper;
using LeaveNotifierApplication.Data;
using LeaveNotifierApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LeaveNotifierApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LeavesController : Controller
    {
        private ILogger<LeavesController> _logger;
        private IMapper _mapper;
        private ILeaveNotifierRepository _repo;

        public LeavesController(ILeaveNotifierRepository repo,
            ILogger<LeavesController> logger,
            IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var leaves = _mapper.Map<IEnumerable<LeaveModel>>(_repo.GetAllLeaves());
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
