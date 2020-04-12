using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Models
{
    public class VodProduction
    {
        public int VodId { get; set; }
        public Vod  Vod { get; set; }
        public int ProductionId { get; set; }
        public Production Production { get; set; }
    }
}
