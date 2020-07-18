using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Checkflix.Data;
using Checkflix.Models;
using Checkflix.Data.Persistance;
using Microsoft.Extensions.Logging;
using Checkflix.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using Checkflix.Models.Enums;
using Checkflix.Data.QueryExtensions;
using Newtonsoft.Json;

namespace Checkflix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductionsController : ControllerBase
    {
        private readonly ICheckflixRepository _repository;
        private readonly ILogger<ProductionsController> _logger;
        private readonly IMapper _mapper;

        public ProductionsController(ICheckflixRepository repository, ILogger<ProductionsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper; 

        }

        // GET: api/Productions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionViewModel>>> GetProductions([FromQuery]PostQueryFilters filters)
        {
            try
            {
                var productions = await _repository.GetAllProductions(filters);
                if(productions == null)
                    return NotFound();

                var metadata = new {
                    productions.TotalCount,
                    productions.PageSize,
                    productions.CurrentPage,
                    productions.TotalPages,
                    productions.HasNextPage,
                    productions.HasPreviousPage
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(_mapper.Map<IEnumerable<Production>, IEnumerable<ProductionViewModel>>(productions));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get productions {ex}");
                return BadRequest("Bad request");
            }

        }

        // GET: api/Productions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Production>> GetProduction(int id)
        {
            try
            {
                var production = await _repository.GetProduction(id);

                if (production == null)
                {
                    return NotFound("Production with specified id does not exist");
                }

                return Ok(_mapper.Map<Production, ProductionViewModel>(production));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get product {ex}");
                return BadRequest("Bad request");
            }

        }

        // PUT: api/Productions/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseViewModel>> PutProduction(int id, [FromBody]ProductionViewModel production)
        {
            if (id != production.ProductionId)
            {
                return BadRequest();
            }

            try
            {
                var mapPorduction = _mapper.Map<ProductionViewModel, Production>(production);
                var response = ValidateProduction(mapPorduction);

                if (response.Status == ResponseStatus.Error)
                    return response;

                var production2update = await _repository.GetProduction(mapPorduction.ProductionId);
                production2update.ImbdId = mapPorduction.ImbdId;
                production2update.ImbdRating = mapPorduction.ImbdRating;
                production2update.Poster = mapPorduction.Poster;
                production2update.ProductionId = mapPorduction.ProductionId;
                production2update.ReleaseDate = mapPorduction.ReleaseDate;
                production2update.Synopsis = mapPorduction.Synopsis;
                production2update.Title = mapPorduction.Title;
                production2update.Type = mapPorduction.Type;
                production2update.Subtitle = mapPorduction.Subtitle;

                ICollection<VodProduction> vodProductionList = new Collection<VodProduction>();
                ICollection<ProductionCategory> productionCategoriesList = new Collection<ProductionCategory>();

                foreach (var v in production.Vods)
                {
                    var currentVod = await _repository.GetVod(v.VodId);
                    var vodProduction = new VodProduction
                    {
                        Production = production2update,
                        Vod = currentVod
                    };
                    vodProductionList.Add(vodProduction);
                }

                foreach (var c in production.Categories)
                {
                    var currentCategory = await _repository.GetCategory(c.CategoryId);
                    var prodCategory = new ProductionCategory
                    {
                        Category = currentCategory,
                        Production = production2update,
                    };
                    productionCategoriesList.Add(prodCategory);
                }

                production2update.VodProductions
                    .Except(vodProductionList)
                    .ToList()
                    .ForEach(x => production2update.VodProductions.Remove(x));

                vodProductionList
                    .Except(production2update.VodProductions)
                    .ToList()
                    .ForEach(x => production2update.VodProductions.Add(x));

                production2update.ProductionCategories
                    .Except(productionCategoriesList)
                    .ToList()
                    .ForEach(x => production2update.ProductionCategories.Remove(x));

                productionCategoriesList
                    .Except(production2update.ProductionCategories)
                    .ToList()
                    .ForEach(x => production2update.ProductionCategories.Add(x));


                _repository.UpdateProduction(production2update);

                if (await _repository.SaveAll())
                {
                    response.Messages.Add("Produkcja została uaktualniona");
                    response.Data = (ProductionViewModel)_mapper.Map<Production, ProductionViewModel>(production2update);
                    return Ok(response);
                }
                else
                {
                    response.Status = ResponseStatus.Error;
                    response.Messages.Add("Produkcja nie została uaktualniona");
                    return response;
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Productions
        [HttpPost]
        public async Task<ActionResult<ResponseViewModel>> PostProduction([FromBody]ProductionViewModel production)
        {
            try
            {
                var mapPorduction = _mapper.Map<ProductionViewModel, Production>(production);

                var response = ValidateProduction(mapPorduction);
                if (response.Status == ResponseStatus.Error)
                    return response;


                // adding item to shared table ex. VodProduction add also to production table and vod table if not exist
                foreach (var v in production.Vods)
                {
                    var currentVod = await _repository.GetVod(v.VodId);
                    var vodProduction = new VodProduction
                    {
                        Production = mapPorduction,
                        Vod = currentVod
                    };
                    _repository.AddVodProduction(vodProduction); // first loop add production and vod, another only vods
                }

                foreach (var c in production.Categories)
                {
                    var currentCategory = await _repository.GetCategory(c.CategoryId);
                    var productionCategory = new ProductionCategory
                    {
                        Category = currentCategory,
                        Production = mapPorduction,
                    };
                    _repository.AddProductionCategory(productionCategory);
                }


                if (await _repository.SaveAll())
                {
                    response.Messages.Add("Produkcja została dodana");
                    response.Data = (ProductionViewModel)_mapper.Map<Production, ProductionViewModel>(mapPorduction);
                    return CreatedAtAction("GetProduction", new { id = mapPorduction.ProductionId }, response);
                }
                else
                {
                    response.Messages.Add("Produkcja nie została dodana");
                    response.Status = ResponseStatus.Error;
                    return response;
                }
                    
            }
            catch(Exception ex)
            {
                return BadRequest($"Coś poszło nie tak. Error: {ex}");
            }                                
        }

        [HttpPost]
        public async Task<ActionResult<ResponseViewModel>> BulkProductionsCreate([FromBody]IEnumerable<ProductionViewModel> productions)
        {
            try 
            {
                var response = ValidateBulkCreate(productions);

                if (response.Status == ResponseStatus.Error)
                    return BadRequest(response);

                if(_repository.AnyProductionsExists())
                {
                    // delete all existing productions
                    _repository.RemoveAllProductions();
                    if (!await _repository.SaveAll())
                    {
                        response.Status = ResponseStatus.Error;
                        response.Messages.Add("Nie udało się usunąć istniejących produkcji");
                        return response;
                    }
                }
                             
                // add new productions
                foreach (var p in productions)
                {
                    var mapPorduction = _mapper.Map<ProductionViewModel, Production>(p);

                    foreach (var v in p.Vods)
                    {
                        var currentVod = await _repository.GetVod(v.VodId);
                        var vodProduction = new VodProduction
                        {
                            Production = mapPorduction,
                            ProductionId = mapPorduction.ProductionId,
                            Vod = currentVod,
                            VodId = v.VodId
                        };
                        _repository.AddVodProduction(vodProduction); // first loop add production and vod, another only vods
                    }

                    foreach (var c in p.Categories)
                    {
                        var currentCategory = await _repository.GetCategory(c.CategoryId);
                        var productionCategory = new ProductionCategory
                        {
                            Category = currentCategory,
                            CategoryId = currentCategory.CategoryId,
                            Production = mapPorduction,
                            ProductionId = mapPorduction.ProductionId
                        };
                        _repository.AddProductionCategory(productionCategory);
                    }
                }

                if (await _repository.SaveAll())
                {
                    response.Messages.Add("Produkcje zostały dodane");
                    return Ok(response);
                }
                else
                {
                    response.Status = ResponseStatus.Error;
                    response.Messages.Add("Produkcje nie zostały dodane");
                    return response;
                }
            }
            catch(Exception ex)
            {
                return BadRequest($"Coś poszło nie tak. Error: {ex}");
            }            
        }

        // DELETE: api/Productions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseViewModel>> DeleteProduction(int id)
        {
            try
            {
                var production = await _repository.GetProduction(id);

                var response = ValidateProduction(production);

                if (response.Status == ResponseStatus.Error)
                    return response;

                _repository.RemoveProduction(production);
                if (await _repository.SaveAll())
                {
                    response.Data = (ProductionViewModel)_mapper.Map<Production, ProductionViewModel>(production);
                    response.Messages.Add("Produkcja została usunięta");
                    return Ok(response);
                }
                else
                {
                    response.Status = ResponseStatus.Error;
                    response.Messages.Add("Nie udało się usunąć produkcji z bazy");
                    return response;
                }
            }
            catch(Exception ex)
            {
                return BadRequest($"Coś poszło nie tak. Error: {ex}");
            }                      

        }

        private bool ProductionExists(int id)
        {
            return _repository.ProductionsExists(id);
        }


        private ResponseViewModel ValidateProduction(Production production)
        {
            var validationResponse = new ResponseViewModel
            {
                Status = ResponseStatus.Success,
                Messages = new List<string>()
            };

            if (production == null)
            {
                validationResponse.Status = ResponseStatus.Error;
                validationResponse.Messages.Add("Podana produkcja jest pusta");
                return validationResponse;
            }

            return validationResponse;
        }

        private ResponseViewModel ValidateBulkCreate(IEnumerable<ProductionViewModel> productions)
        {
            var validationResponse = new ResponseViewModel
            {
                Status = ResponseStatus.Success,
                Messages = new List<string>()
            };

            if (productions == null || productions.Count() == 0)
            {
                validationResponse.Status = ResponseStatus.Error;
                validationResponse.Messages.Add("Kolekcja, którą chcesz dodać jest pusta");
                return validationResponse;
            }

            return validationResponse;
        }
    }
}
