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

namespace Checkflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<ProductionViewModel>>> GetProductions()
        {
            try
            {
                var products = await _repository.GetAllProductions();
                return Ok(_mapper.Map<IEnumerable<Production>, IEnumerable<ProductionViewModel>>(products));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get products {ex}");
                return BadRequest("Bad request");
            }
            
        }

        // GET: api/Productions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Production>> GetProduction(int id)
        {
            var production = await _repository.GetProduction(id);
  
            if (production == null)
            {
                return NotFound("Production with specified id does not exist");
            }

            return Ok(_mapper.Map<Production, ProductionViewModel>(production));
        }

        // PUT: api/Productions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduction(int id, [FromBody]ProductionViewModel production)
        {
            if (id != production.ProductionId)
            {
                return BadRequest();
            }
            var mapPorduction = _mapper.Map<ProductionViewModel, Production>(production);

            _repository.UpdateProduction(mapPorduction);
            //_context.Entry(mapPorduction).State = EntityState.Modified;

            try
            {
                if (await _repository.SaveAll())
                    return Ok(_mapper.Map<Production, ProductionViewModel>(mapPorduction));
                //await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/Productions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProductionViewModel>> PostProduction([FromBody]ProductionViewModel production)
        {
            var mapPorduction = _mapper.Map<ProductionViewModel, Production>(production);

            _repository.AddProduction(mapPorduction);

            if(await _repository.SaveAll())
                return CreatedAtAction("GetProduction", new { id = mapPorduction.ProductionId }, _mapper.Map<Production, ProductionViewModel>(mapPorduction));
           
            return BadRequest("Failed to add production");
        }

        // DELETE: api/Productions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Production>> DeleteProduction(int id)
        {
            var production = await _repository.GetProduction(id);
            if (production == null)
            {
                return NotFound();
            }

            _repository.RemoveProduction(production);
            if (await _repository.SaveAll())
                // return here deleted product to filter it out in front-end
                return Ok(_mapper.Map<Production, ProductionViewModel>(production));

            return production;
        }

        private bool ProductionExists(int id)
        {
            return _repository.ProductionsExists(id);
        }
    }
}
