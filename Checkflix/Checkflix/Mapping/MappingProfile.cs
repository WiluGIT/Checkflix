using AutoMapper;
using Checkflix.Models;
using Checkflix.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Production, ProductionViewModel>()
                .ReverseMap();
            CreateMap<Category, CategoryViewModel>()
                .ReverseMap();

        }
    }
}
