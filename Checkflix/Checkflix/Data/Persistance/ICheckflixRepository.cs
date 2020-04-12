using Checkflix.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkflix.Data.Persistance
{
    public interface ICheckflixRepository
    {
        Task<ActionResult<IEnumerable<Production>>> GetAllProductions();
        Task<bool> SaveAll();
    }
}