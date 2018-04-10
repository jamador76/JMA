using System;
using JMA.DAL;
using JMA.Mapping.DTOs;
using JMA.Mapping.ViewModels;
using AutoMapper;

namespace JMA.Mapping
{
    public sealed class BaseProfile : Profile
    {
        public BaseProfile()
        {
            // create maps
            CreateMap<ClaimFormViewModel, ClaimantDTO>();
            CreateMap<ClaimantDTO, UnknownClaimant>();
            CreateMap<ClaimantDTO, KnownClaimant>();
            CreateMap<KnownClaimantDTO, KnownClaimFormViewModel>();
            CreateMap<KnownClaimFormViewModel, KnownClaimantDTO>();
            CreateMap<KnownClaimant, KnownClaimantDTO>();
        }
    }
}