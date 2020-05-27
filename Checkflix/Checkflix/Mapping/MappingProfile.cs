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
                .ForMember(dto => dto.Vods, opt => opt.MapFrom(x => x.VodProductions.Select(y => y.Vod).ToList()))
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(x => x.ProductionCategories.Select(y => y.Category).ToList()))
                .ReverseMap();

            CreateMap<Category, CategoryViewModel>()
                .ReverseMap();

            CreateMap<Vod, VodViewModel>()
                .ReverseMap();

        }
    }
}
