using AutoMapper;

namespace Spendings.Orchrestrators.Users
{
    public class OrchUserProfile : Profile
    {
        public OrchUserProfile()
        {
            CreateMap<User,Core.Users.User>()
                .ForMember(dest => dest.Login, memberOptions: opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, memberOptions: opt => opt.MapFrom(src => src.Password));
               
        }
    }
    public class UserOrchProfile : Profile
    {
        public UserOrchProfile()
        {
            CreateMap<Core.Users.User, User>()
                .ForMember(dest => dest.Login, memberOptions: opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, memberOptions: opt => opt.MapFrom(src => src.Password));

        }
    }
}
