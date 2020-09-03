using Checkflix.Data.QueryExtensions;
using Checkflix.Models;
using Checkflix.Models.CustomEntities;
using Checkflix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkflix.Data.Persistance
{
    public interface ICheckflixRepository
    {
        void AddProduction(Production production);
        void AddProductionCategory(ProductionCategory productionCategory);
        void AddUserProduction(ApplicationUserProduction userProduction);
        void AddVodProduction(VodProduction vodProduction);
        bool AnyProductionsExists();
        Task<IEnumerable<Category>> GetAllCategories();
        Task<PagedList<Production>> GetAllProductions(PostQueryFilters filters);

        Task<IEnumerable<Vod>> GetAllVods();
        Task<Category> GetCategory(int id);
        Task<Production> GetProduction(int id);
        Task<ProductionCategory> GetProductionCategory(int categoryId, int productionId);
        Task<ApplicationUserProduction> GetUserProduction(string userId, int productionId);
        Task<Vod> GetVod(int id);
        Task<VodCountViewModel> GetVodCount();
        Task<VodProduction> GetVodProduction(int vodId, int productionId);
        bool ProductionsExists(int id);
        void RemoveAllProductions();
        void RemoveProduction(Production production);
        Task<bool> SaveAll();
        void UpdateCategories(IEnumerable<Category> categories);
        void UpdateProduction(Production production);
        void UpdateProductionCategory(ProductionCategory productionCategory);
        void UpdateVodProduction(VodProduction vodProduction);
    }
}