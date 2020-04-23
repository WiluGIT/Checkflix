using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public int GenreApiId { get; set; }
        public string CategoryName { get; set; }

        

        public ICollection<ApplicationUserCategory> ApplicationUserCategories { get; set; }
        public ICollection<ProductionCategory> ProductionCategories { get; set; }

        public Category()
        {
            ApplicationUserCategories = new Collection<ApplicationUserCategory>();
            ProductionCategories = new Collection<ProductionCategory>();
        }
    }
}
