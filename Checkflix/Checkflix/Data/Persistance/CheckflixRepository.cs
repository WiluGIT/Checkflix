using Checkflix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Data.Persistance
{
    public class CheckflixRepository : ICheckflixRepository
    {
        private readonly ApplicationDbContext _context;
        public CheckflixRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Productions
        public async Task<IEnumerable<Production>> GetAllProductions()
        {
            return await _context.Productions.ToListAsync();
        }

        public async Task<Production> GetProduction(int id)
        {
            return await _context.Productions.FindAsync(id);
        }

        public void AddProduction(Production production)
        {
             _context.Productions.Add(production);
        }

        public void RemoveProduction(Production production)
        {
            _context.Productions.Remove(production);
        }

        public void UpdateProduction(Production production)
        {
            _context.Entry(production).State = EntityState.Modified;
        }

        public bool ProductionsExists(int id)
        {
            return _context.Productions.Any(e => e.ProductionId == id);
        }


        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
