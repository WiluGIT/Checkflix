

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkflix.Data.Persistance;
using Checkflix.Models;
using Checkflix.Models.Enums;
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

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseViewModel>> UpdatePreferences([FromBody] UserPreferencesViewModel userPreferencesViewModel)
        {
            try
            {
                var validationResponse = new ResponseViewModel
                {
                    Status = ResponseStatus.Success,
                    Messages = new List<string>()
                };
                var user = await _repository.GetUserWithPreferencesCollections(_userManager.GetUserId(User));
                if (user != null)
                {
                    var vodList = new Collection<ApplicationUserVod>();
                    var categoriesList = new Collection<ApplicationUserCategory>();

                    foreach (var v in userPreferencesViewModel.Vods)
                    {
                        // var currentVod = await _repository.GetVod(v.VodId);
                        var userVod = new ApplicationUserVod
                        {
                            ApplicationUserId = user.Id,
                            VodId = v.VodId
                        };
                        vodList.Add(userVod);
                    }

                    foreach (var c in userPreferencesViewModel.Categories)
                    {
                        var userCategory = new ApplicationUserCategory
                        {
                            CategoryId = c.CategoryId,
                            ApplicationUserId = user.Id,
                        };
                        categoriesList.Add(userCategory);
                    }

                    user.ApplicationUserVods
                            .Except(vodList)
                            .ToList()
                            .ForEach(x => user.ApplicationUserVods.Remove(x));

                    vodList
                        .Except(user.ApplicationUserVods)
                        .ToList()
                        .ForEach(x => user.ApplicationUserVods.Add(x));

                    user.ApplicationUserCategories
                        .Except(categoriesList)
                        .ToList()
                        .ForEach(x => user.ApplicationUserCategories.Remove(x));

                    categoriesList
                        .Except(user.ApplicationUserCategories)
                        .ToList()
                        .ForEach(x => user.ApplicationUserCategories.Add(x));


                    _repository.UpdateUser(user);
                    if (await _repository.SaveAll())
                    {
                        validationResponse.Messages.Add("Preferencje zostały uaktualnione");
                        return Ok(validationResponse);
                    }
                }
                validationResponse.Status = ResponseStatus.Error;
                validationResponse.Messages.Add("Preferencje nie zostały uaktualnione");
                return BadRequest(validationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get vods {ex}");
                return BadRequest("Couldn't update preferences");
            }
        }
    }
}