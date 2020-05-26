using Checkflix.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.ViewModels
{
    public class ProductionViewModel
    {
        public int ProductionId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public string Synopsis { get; set; }
        [Required]
        public ProductionType Type { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string ImbdId { get; set; }
        [Required]
        public float ImbdRating { get; set; }

        [Required]
        public ICollection<VodViewModel> Vods { get; set; }
        [Required]
        public ICollection<CategoryViewModel> Categories { get; set; }
    }
}
