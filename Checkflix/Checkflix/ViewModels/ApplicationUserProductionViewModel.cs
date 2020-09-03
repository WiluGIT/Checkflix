using System.ComponentModel.DataAnnotations;

namespace Checkflix.ViewModels
{
    public class ApplicationUserProductionViewModel
    {
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public int ProductionId { get; set; }
        [Required]
        public bool Favourites { get; set; } = false;
        [Required]
        public bool ToWatch { get; set; } = false;
        [Required]
        public bool Watched { get; set; } = false;
    }
}