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
                            if (userProductionVM.ToWatch == true)
                            {
                                validationResponse.Messages.Add("Dodano do 'Do obejrzenia'");
                            }
                            else if (userProductionVM.ToWatch == false)
                            {
                                validationResponse.Messages.Add("Usunięto z 'Do obejrzenia'");
                            }
                            userProduction.ToWatch = userProductionVM.ToWatch;
                        }
                        else if (userProductionVM.Watched != null)
                        {
                            if (userProductionVM.Watched == true)
                            {
                                validationResponse.Messages.Add("Dodano do 'Obejrzane'");
                            }
                            else if (userProductionVM.Watched == false)
                            {
                                validationResponse.Messages.Add("Usunięto z 'Obejrzane'");
                            }
                            userProduction.Watched = userProductionVM.Watched;
                        }
                        else if (userProductionVM.Favourites != null)
                        {
                            if (userProductionVM.Favourites == true)
                            {
                                validationResponse.Messages.Add("Dodano do 'Ulubione'");
                            }
                            else if (userProductionVM.Favourites == false)
                            {
                                validationResponse.Messages.Add("Usunięto z 'Ulubione'");
                            }
                            userProduction.Favourites = userProductionVM.Favourites;
                        }
                        if (userProduction.ToWatch == false && userProduction.Watched == false && userProduction.Favourites == false)
                        {
                            _repository.RemoveUserProduction(userProduction);
                        }
                        else
                        {
                            validationResponse.Data = (ApplicationUserProductionViewModel)_mapper.Map<ApplicationUserProduction, ApplicationUserProductionViewModel>(userProduction);
                        }
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
                            };
                            if (userProductionVM.ToWatch != null)
                            {
                                if (userProductionVM.ToWatch == true)
                                {
                                    validationResponse.Messages.Add("Dodano do 'Do obejrzenia'");
                                }
                                else if (userProductionVM.ToWatch == false)
                                {
                                    validationResponse.Messages.Add("Usunięto z 'Do obejrzenia'");
                                }
                                newUserProduction.ToWatch = userProductionVM.ToWatch;
                            }
                            else if (userProductionVM.Watched != null)
                            {
                                if (userProductionVM.Watched == true)
                                {
                                    validationResponse.Messages.Add("Dodano do 'Obejrzane'");
                                }
                                else if (userProductionVM.Watched == false)
                                {
                                    validationResponse.Messages.Add("Usunięto z 'Obejrzane'");
                                }
                                newUserProduction.Watched = userProductionVM.Watched;
                            }
                            else if (userProductionVM.Favourites != null)
                            {
                                if (userProductionVM.Favourites == true)
                                {
                                    validationResponse.Messages.Add("Dodano do 'Ulubione'");
                                }
                                else if (userProductionVM.Favourites == false)
                                {
                                    validationResponse.Messages.Add("Usunięto z 'Ulubione'");
                                }
                                newUserProduction.Favourites = userProductionVM.Favourites;
                            }
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseViewModel>> GetUserProductions()
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
                    var userProductions = await _repository.GetUserProductionsIds(user.Id);
                    if (userProductions != null)
                    {
                        validationResponse.Messages.Add("Udało się pobrać produkcje");
                        validationResponse.Data = _mapper.Map<IEnumerable<ApplicationUserProduction>, IEnumerable<ApplicationUserProductionViewModel>>(userProductions);
                        return Ok(validationResponse);
                    }
                }
                validationResponse.Messages.Add("Nie udało się pobrać produkcji");
                validationResponse.Status = ResponseStatus.Error;
                return BadRequest(validationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add production to collection{ex}");
                return BadRequest("Bad request");
            }
        }
    }
}