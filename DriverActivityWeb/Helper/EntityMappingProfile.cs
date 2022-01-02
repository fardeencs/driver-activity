using AutoMapper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Helper
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<AppUserImage, AppUserFileVM>().ReverseMap();
            CreateMap<AppUserSignature, AppUserFileVM>().ReverseMap();
            CreateMap<DriverEod, DriverEodVM>().ReverseMap();
            CreateMap<RouteConfig, RouteConfigVM>().ReverseMap();
            CreateMap<AppUser, AppUserVM>()
                //.ForMember(d => d.FullName, s => s.MapFrom(_ => $"{_.FirstName} {_.LastName}"));
                //.ForMember(
                //    dest => Convert.ToInt32(dest.QID),
                //    opt => opt.MapFrom(src => $"{src.QID}")
                //)
                //.ForMember(
                //    dest => Convert.ToInt32(dest.StaffID),
                //    opt => opt.MapFrom(src => $"{src.StaffID}")
                //)
                ////.ForMember(
                ////    dest => Convert.ToInt32(dest.VehicleID),
                ////    opt => opt.MapFrom(src => $"{src.VehicleID}")
                ////)
                .ReverseMap();
        }
    }
}
