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

        #region Productions
        // Productions
        public async Task<IEnumerable<Production>> GetAllProductions()
        {
            return await _context.Productions
                .Include(m=> m.VodProductions)
                .ThenInclude(m=>m.Vod)
                .Include(m=>m.ProductionCategories)
                .ThenInclude(m=>m.Category)
                .ToListAsync();
        }

        public async Task<Production> GetProduction(int id)
        {
            return await _context.Productions
                .Where(m=>m.ProductionId.Equals(id))
                .Include(m => m.VodProductions)
                .ThenInclude(m => m.Vod)
                .Include(m => m.ProductionCategories)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync();
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

        #endregion

        #region Categories
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public void UpdateCategories(IEnumerable<Category> categories)
        {
            foreach(var c in categories)
            {
                _context.Categories.Add(c);
            }
        }
        #endregion

        #region Vods
        public async Task<Vod> GetVod(int id)
        {
            return await _context.Vods.FindAsync(id);
        }
        public async Task<IEnumerable<Vod>> GetAllVods()
        {
            return await _context.Vods.ToListAsync();
        }
        #endregion

        #region VodProduction
        public void AddVodProduction(VodProduction vodProduction)
        {
            _context.VodProductions.Add(vodProduction);
        }
        #endregion

        #region ProductionCategory
        public void AddProductionCategory(ProductionCategory productionCategory)
        {
            _context.ProductionCategories.Add(productionCategory);
        }
        #endregion
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
