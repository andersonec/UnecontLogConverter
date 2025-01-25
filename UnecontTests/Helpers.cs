using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UnecontLogConverter.MappingProfiles;

namespace UnecontTests
{
    public class Helpers
    {
        public static MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<LogProfile>();
        });
    }
}
