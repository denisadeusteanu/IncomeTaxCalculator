using AutoMapper;
using IncomeTaxCalculator.DTOs;
using Persistence.Models;

namespace IncomeTaxCalculator.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaxBandDto, TaxBand>()
                .ForMember(taxBand => taxBand.Id, opt => opt.MapFrom(taxBandDto => Guid.NewGuid()));
        }
    }
}
