using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Models
{
    public class Production
    {
        public int ProductionId { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Synopsis { get; set; }
        public ProductionType Type { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImbdId { get; set; }
        public float ImbdRating { get; set; }


        public ICollection<ApplicationUserProduction> ApplicationUserProductions { get; set; }
        public ICollection<VodProduction> VodProductions { get; set; }
        public ICollection<ProductionCategory> ProductionCategories { get; set; }

        public Production()
        {
            ApplicationUserProductions = new Collection<ApplicationUserProduction>();
            VodProductions = new Collection<VodProduction>();
            ProductionCategories = new Collection<ProductionCategory>();
        }
    }

    public enum ProductionType
    {
        Movie=0,
        Series
    }
}
