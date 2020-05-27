using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkflix.Data.Persistance;
using Checkflix.Models;
using Checkflix.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkflix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICheckflixRepository _repository;
        private readonly ILogger<ProductionsController> _logger;
        private readonly IMapper _mapper;

        public CategoriesController(ICheckflixRepository repository, ILogger<ProductionsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories()
        {
            try
            {
                var categories = await _repository.GetAllCategories();

                if (categories == null)
                    return NotFound();

                return Ok(_mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(categories));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get categories {ex}");
                return BadRequest("Bad request");
            }

        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> UpdateCategories(IEnumerable<CategoryViewModel> categories)
        {
            var mapCategories = _mapper.Map<IEnumerable<CategoryViewModel>, IEnumerable<Category>>(categories);
            var actualCategories = await _repository.GetAllCategories();


            var result = actualCategories.Where(p => mapCategories.Any(l => p.GenreApiId != l.GenreApiId));

            if (result.Count()>0)
                return BadRequest("Categories are up to date");


            _repository.UpdateCategories(mapCategories);

            if (await _repository.SaveAll())
                return Ok(categories);

            return BadRequest("Failed to add production");
        }
    }
}