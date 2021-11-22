using AutoMapper;

namespace Spendings.Orchrestrators.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Core.Categories.Category, Category>()
                .ForMember(dest => dest.Name, memberOptions: opt => opt.MapFrom(src => src.Name));
        }
    }

    public class CategoryContactProfile : Profile
    {
        public CategoryContactProfile()
        {
            CreateMap<Category, Core.Categories.Category>()
                 .ForMember(dest => dest.Name, memberOptions: opt => opt.MapFrom(src => src.Name));
        }
    }
}
