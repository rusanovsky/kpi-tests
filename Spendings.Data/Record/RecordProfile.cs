using AutoMapper;

namespace Spendings.Data.Records
{
    public class RecordDaoProfile : Profile
    {
        public RecordDaoProfile()
        {
            CreateMap<Record, Core.Records.Record>()
                .ForMember(dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, memberOptions: opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Amount, memberOptions: opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CategoryId, memberOptions: opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Date, memberOptions: opt => opt.MapFrom(src => src.Date));
        }

    }
    public class DaoRecordProfile : Profile
    {
        public DaoRecordProfile()
        {
            CreateMap<Core.Records.Record,Record>()
                .ForMember(dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, memberOptions: opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Amount, memberOptions: opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CategoryId, memberOptions: opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Date, memberOptions: opt => opt.MapFrom(src => src.Date));
        }

    }
}
