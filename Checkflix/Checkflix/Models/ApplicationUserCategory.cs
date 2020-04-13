using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Models
{
    public class ApplicationUserCategory
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
