using System.ComponentModel.DataAnnotations;

namespace Checkflix.ViewModels
{
    public class ApplicationUserProductionViewModel
    {
        [Required]
        public int ProductionId { get; set; }
        public bool? Favourites { get; set; }
        public bool? ToWatch { get; set; }
        public bool? Watched { get; set; }
    }
}