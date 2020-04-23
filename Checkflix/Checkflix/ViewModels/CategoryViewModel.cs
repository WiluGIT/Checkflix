using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        [Required]
        public int GenreApiId { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
