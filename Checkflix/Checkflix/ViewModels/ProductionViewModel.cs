using Checkflix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.ViewModels
{
    public class ProductionViewModel
    {
        public int ProductionId { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Synopsis { get; set; }
        public ProductionType Type { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ImbdId { get; set; }
        public float ImbdRating { get; set; }
    }
}
