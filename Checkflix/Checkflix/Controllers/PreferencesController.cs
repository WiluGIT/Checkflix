

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Checkflix.Data.Persistance;
using Checkflix.Models;
using Checkflix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkflix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly ICheckflixRepository _repository;
        private readonly ILogger<ProductionsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public PreferencesController(ICheckflixRepository repository, ILogger<ProductionsController> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: api/Vods
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserPreferencesViewModel>>> GetUserPreferences()
        {
            try
            {
                var userPreferences = await _repository.GetUserPreferences(_userManager.GetUserId(User));
                if (userPreferences == null)
                    return NotFound();

                return Ok(userPreferences);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get vods {ex}");
                return BadRequest("Couldn't get vods");
            }

        }
    }
}