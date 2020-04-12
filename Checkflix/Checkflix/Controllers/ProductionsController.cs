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

namespace Checkflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICheckflixRepository _repository;
        private readonly ILogger<ProductionsController> _logger;

        public ProductionsController(ApplicationDbContext context, ICheckflixRepository repository, ILogger<ProductionsController> logger)
        {
            _context = context;
            _repository = repository;
            _logger = logger;
        }

        // GET: api/Productions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Production>>> GetProductions()
        {
            try
            {
                return await _repository.GetAllProductions();
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
            var production = await _context.Productions.FindAsync(id);

            if (production == null)
            {
                return NotFound();
            }

            return production;
        }

        // PUT: api/Productions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduction(int id, Production production)
        {
            if (id != production.ProductionId)
            {
                return BadRequest();
            }

            _context.Entry(production).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
        public async Task<ActionResult<Production>> PostProduction(Production production)
        {
            _context.Productions.Add(production);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduction", new { id = production.ProductionId }, production);
        }

        // DELETE: api/Productions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Production>> DeleteProduction(int id)
        {
            var production = await _context.Productions.FindAsync(id);
            if (production == null)
            {
                return NotFound();
            }

            _context.Productions.Remove(production);
            await _context.SaveChangesAsync();

            return production;
        }

        private bool ProductionExists(int id)
        {
            return _context.Productions.Any(e => e.ProductionId == id);
        }
    }
}
