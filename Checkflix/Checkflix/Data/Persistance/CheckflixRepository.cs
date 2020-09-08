using Checkflix.Data.QueryExtensions;
using Checkflix.Models;
using Checkflix.Models.CustomEntities;
using Checkflix.ViewModels;
using EFCore.BulkExtensions;
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
        public async Task<PagedList<Production>> GetAllProductions(PostQueryFilters filters)
        {
            var productions = new List<Production>();
            if (filters.Categories != null)
            {
                productions = await _context.ProductionCategories
                                .Where(x => filters.Categories
                                .Contains(x.CategoryId))
                                .Select(x => x.Production)
                                .Distinct()
                                // .Include(m => m.VodProductions)
                                // .ThenInclude(m => m.Vod)
                                // .Include(m => m.ProductionCategories)
                                // .ThenInclude(m => m.Category)
                                .ToListAsync();
            }
            else
            {
                productions = await _context.Productions
                            //    .Include(m => m.VodProductions)
                            //    .ThenInclude(m => m.Vod)
                            //    .Include(m => m.ProductionCategories)
                            //    .ThenInclude(m => m.Category)
                               .ToListAsync();
            }
            // Here in if condition implement filter logic for each patameter in PostQueryFilters class
            // Search string
            if (!string.IsNullOrEmpty(filters.SearchQuery))
            {
                var querySearch = filters.SearchQuery.ToLower();
                productions = productions.Where(x => x.Title.ToLower().Contains(querySearch) || x.Subtitle.ToLower().Contains(querySearch)).ToList();
            }
            // Vod platform // if filters have query like isHbo = false - filter netflix
            if (!filters.IsHbo || !filters.IsNetflix)
            {
                if (!filters.IsHbo)
                {
                    productions = productions.Where(x => x.VodProductions.Any(vp => vp.VodId.Equals(1))).ToList();
                }
                else if (!filters.IsNetflix)
                {
                    productions = productions.Where(x => x.VodProductions.Any(vp => vp.VodId.Equals(2))).ToList();
                }
            }
            // Year from to filter
            if (filters.YearFrom != null && filters.YearTo != null)
            {
                productions = productions.Where(x => x.ReleaseDate.Year >= filters.YearFrom && x.ReleaseDate.Year <= filters.YearTo).ToList();
            }
            // Rating from to filter
            if (filters.RatingFrom != null && filters.RatingTo != null)
            {
                productions = productions.Where(x => x.ImbdRating >= filters.RatingFrom && x.ImbdRating <= filters.RatingTo).ToList();
            }
            
            var pagedProductions = PagedList<Production>.Create(productions, filters.PageNumber, filters.PageSize);

            return pagedProductions;
        }

        public async Task<Production> GetProduction(int id)
        {
            return await _context.Productions
                .Where(m => m.ProductionId.Equals(id))
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
            _context.Productions.Update(production);
        }

        public bool ProductionsExists(int id)
        {
            return _context.Productions.Any(e => e.ProductionId == id);
        }

        public void RemoveAllProductions()
        {
            _context.Productions.RemoveRange(_context.Productions);
        }
        public bool AnyProductionsExists()
        {
            return _context.Productions.Any();
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
            foreach (var c in categories)
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

        public async Task<VodCountViewModel> GetVodCount()
        {
            var netflixCount = await _context.VodProductions.Where(v => v.VodId.Equals(1)).CountAsync();
            var hboCount = await _context.VodProductions.Where(v => v.VodId.Equals(2)).CountAsync();

            var countViewModel = new VodCountViewModel
            {
                NetflixCount = netflixCount,
                HboCount = hboCount
            };

            return countViewModel;
        }
        #endregion

        #region VodProduction
        public void AddVodProduction(VodProduction vodProduction)
        {
            _context.VodProductions.Add(vodProduction);
        }

        public async Task<VodProduction> GetVodProduction(int vodId, int productionId)
        {
            return await _context.VodProductions.Where(m => m.VodId.Equals(vodId) && m.ProductionId.Equals(productionId)).FirstOrDefaultAsync();
        }

        public async void UpdateVodProduction(VodProduction vodProduction)
        {
            var production2update = await _context.VodProductions.Where(m => m.ProductionId.Equals(vodProduction.ProductionId) && m.VodId.Equals(vodProduction.VodId)).FirstOrDefaultAsync();


            _context.Entry(production2update).CurrentValues.SetValues(production2update);
            //_context.VodProductions.Update(vodProduction);
        }

        #endregion

        #region ProductionCategory
        public void AddProductionCategory(ProductionCategory productionCategory)
        {
            _context.ProductionCategories.Add(productionCategory);
        }

        public async Task<ProductionCategory> GetProductionCategory(int categoryId, int productionId)
        {
            return await _context.ProductionCategories.Where(m => m.CategoryId.Equals(categoryId) && m.ProductionId.Equals(productionId)).FirstOrDefaultAsync();
        }

        public void UpdateProductionCategory(ProductionCategory productionCategory)
        {
            _context.ProductionCategories.Update(productionCategory);
        }

        #endregion

        #region UserProductions
        public async Task<ApplicationUserProduction> GetUserProduction(string userId, int productionId)
        {
            return await _context.ApplicationUserProductions.Where(m => m.ApplicationUserId.Equals(userId) && m.ProductionId.Equals(productionId)).FirstOrDefaultAsync();
        }
        public async Task<List<ApplicationUserProduction>> GetUserProductionsIds(string userId)
        {
            return await _context.ApplicationUserProductions.Where(m => m.ApplicationUserId.Equals(userId)).ToListAsync();
        }
        public void AddUserProduction(ApplicationUserProduction userProduction)
        {
            _context.ApplicationUserProductions.Add(userProduction);
        }
        public void RemoveUserProduction(ApplicationUserProduction userProduction)
        {
            _context.ApplicationUserProductions.Remove(userProduction);
        }
        #endregion

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
