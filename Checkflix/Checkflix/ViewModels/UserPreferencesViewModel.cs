using System.Collections.Generic;

namespace Checkflix.ViewModels
{
    public class UserPreferencesViewModel
    {
        public ICollection<VodViewModel> Vods { get; set; }
        public ICollection<CategoryViewModel> Categories { get; set; }
    }
}