using AutoMapper;
using DriverActivityWeb.Models;
using DriverActivityWeb.ViewModels;

namespace DriverActivityWeb.Helper
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<AppUserImage, AppUserImageVM>().ReverseMap();
            CreateMap<AppUser, AppUserVM>()
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
