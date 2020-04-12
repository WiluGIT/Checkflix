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

        public async Task<ActionResult<IEnumerable<Production>>> GetAllProductions()
        {
            return await _context.Productions.ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
