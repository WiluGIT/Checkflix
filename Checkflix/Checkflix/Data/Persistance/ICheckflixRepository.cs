using Checkflix.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkflix.Data.Persistance
{
    public interface ICheckflixRepository
    {
        void AddProduction(Production production);
        Task<IEnumerable<Production>> GetAllProductions();
        Task<Production> GetProduction(int id);
        bool ProductionsExists(int id);
        void RemoveProduction(Production production);
        Task<bool> SaveAll();
        void UpdateProduction(Production production);
    }
}