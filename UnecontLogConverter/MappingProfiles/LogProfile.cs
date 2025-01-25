using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text.Json;
using UnecontLogConverter.Entities;
using UnecontLogConverter.Infrastructure;
using UnecontLogConverter.ViewModels;

namespace UnecontLogConverter.MappingProfiles
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<Log, LogTransformed>()
                .ForMember(dr => dr.LogId, opt => opt.MapFrom(d => d.Id))
                ;

            CreateMap<Log, LogViewModel>()
                .ForMember(dr => dr.Content, opt => opt.MapFrom(d => d.Content))
                ;

            CreateMap<LogTransformed, LogTransformedViewModel>()
                .ForMember(dr => dr.TransformedContent, opt => opt.MapFrom(d => d.TransformedContent))
                ;

            CreateMap<Log, LogListViewModel>()
                ;

            CreateMap<LogTransformed, LogListViewModel>()
                ;
        }
    }
}
