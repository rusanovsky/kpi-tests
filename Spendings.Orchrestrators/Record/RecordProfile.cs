using AutoMapper;

namespace Spendings.Orchrestrators.Records
{
    public class RecordProfile : Profile
    {
        public RecordProfile()
        {
            CreateMap<Core.Records.Record, Record>()
                .ForMember(dest => dest.CategoryId, memberOptions: opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Amount, memberOptions: opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Date, memberOptions: opt => opt.MapFrom(src => src.Date));
        }
    }

    public class RecordContractProfile : Profile
    {
        public RecordContractProfile()
        {
            CreateMap<Record,Core.Records.Record>()
                .ForMember(dest => dest.CategoryId, memberOptions: opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Amount, memberOptions: opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Date, memberOptions: opt => opt.MapFrom(src => src.Date));
        }
    }
}
