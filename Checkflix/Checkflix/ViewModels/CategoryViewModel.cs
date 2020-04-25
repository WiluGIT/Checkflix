using Checkflix.Models;
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
        public int GenreApiId { get; set; }
        public string CategoryName { get; set; }
    }
}
