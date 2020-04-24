using Checkflix.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.ViewModels
{
    public class VodViewModel
    {
        public int VodId { get; set; }
        public string PlatformName { get; set; }

    }
}
