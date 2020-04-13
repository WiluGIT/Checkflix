using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Models
{
    public class ProductionCategory
    {
        public int ProductionId { get; set; }
        public Production Production { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
       
    }
}
