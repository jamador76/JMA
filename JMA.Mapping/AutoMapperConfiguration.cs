using System;
using AutoMapper;

namespace JMA.Mapping
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<BaseProfile>();
            });
        }
    }
}