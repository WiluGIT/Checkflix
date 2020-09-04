using System;
using System.Collections.Generic;
using System.Security.Claims;
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
using Newtonsoft.Json;

namespace Checkflix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserProductionsController : ControllerBase
    {
        private readonly ICheckflixRepository _repository;
        private readonly ILogger<ProductionsController> _logger;
        private readonly IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;


        public UserProductionsController(ICheckflixRepository repository,
        ILogger<ProductionsController> logger,
        IMapper mapper,
        UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseViewModel>> PostUserProduction([FromBody] ApplicationUserProductionViewModel userProductionVM)
        {
            try
            {
                var validationResponse = new ResponseViewModel
                {
                    Status = ResponseStatus.Success,
                    Messages = new List<string>()
                };

                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var productionId = userProductionVM.ProductionId;
                    var userProduction = await _repository.GetUserProduction(user.Id, productionId);
                    if (userProduction != null)
                    {
                        if (userProductionVM.ToWatch != null)
                        {
                            userProduction.ToWatch = userProductionVM.ToWatch;
                        }
                        if (userProductionVM.Watched != null)
                        {
                            userProduction.Watched = userProductionVM.Watched;
                        }
                        if (userProductionVM.Favourites != null)
                        {
                            userProduction.Favourites = userProductionVM.Favourites;
                        }
                        validationResponse.Data = (ApplicationUserProductionViewModel)_mapper.Map<ApplicationUserProduction, ApplicationUserProductionViewModel>(userProduction);
                    }
                    else if (userProduction == null)
                    {
                        var production = await _repository.GetProduction(productionId);
                        if (production != null)
                        {
                            var newUserProduction = new ApplicationUserProduction
                            {
                                Production = production,
                                ApplicationUser = user,
                                ToWatch = userProductionVM.ToWatch,
                                Watched = userProductionVM.Watched,
                                Favourites = userProductionVM.Favourites
                            };
                            _repository.AddUserProduction(newUserProduction);
                            validationResponse.Data = (ApplicationUserProductionViewModel)_mapper.Map<ApplicationUserProduction, ApplicationUserProductionViewModel>(newUserProduction);
                        }
                        else
                        {
                            validationResponse.Messages.Add("Produkcja nie została znaleziona");
                        }
                    }
                    if (await _repository.SaveAll())
                    {
                        validationResponse.Messages.Add("Produkcja została dodana do kolekcji");
                        return CreatedAtAction("PostUserProduction", validationResponse);
                    }
                    else
                    {
                        validationResponse.Messages.Add("Nie udało się dodać produkcji do kolekcji");
                        validationResponse.Status = ResponseStatus.Error;
                        return BadRequest(validationResponse);
                    }
                }
                return BadRequest("Nie udało się dodać produkcji do kolekcji");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }

        }
    }
}