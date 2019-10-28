
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Permissions;
using System.Security.Claims;

namespace MyXSpace.Web.ViewModels
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserViewModel>()
                 .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserViewModel, AppUser>()
                .ForMember(d => d.UserRoles, map => map.Ignore());

            CreateMap<AppUser, UserEditViewModel>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserEditViewModel, AppUser>()
                .ForMember(d => d.UserRoles, map => map.Ignore());
            CreateMap<AppUser, UserPatchViewModel>()
                .ReverseMap();

            //.ForMember(d => d.Users.Count, map => map.MapFrom(s => s.UsersCount != null ? s.UsersCount : 0))
            //.ReverseMap();

            CreateMap<ApplicationPermission, PermissionViewModel>()
                .ForMember(d => d.Value, map => map.MapFrom(s => s.Value))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.Name))
                .ForMember(d => d.Description, map => map.MapFrom(s => s.Description))
                .ForMember(d => d.GroupName, map => map.MapFrom(s => s.GroupName))
                .ReverseMap();

            CreateMap<PermissionViewModel, ClaimViewModel>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.Type)) //Name
                .ForMember(d => d.Value, map => map.MapFrom(s => s.Value))
                .ReverseMap();

            CreateMap<IdentityRoleClaim<string>, ClaimViewModel>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<PermissionViewModel, IdentityRoleClaim<string>>()
                .ForMember(d => d.ClaimType, map => map.MapFrom(s => s.Type))//CustomClaimTypes.Permission))//s => s.Type))
                .ForMember(d => d.ClaimValue, map => map.MapFrom(s => s.Value))
                .ReverseMap();

            //CreateMap<IdentityRoleClaim<string>, PermissionViewModel>()
            //    .ConvertUsing(s => Mapper.Map<PermissionViewModel>(ApplicationPermissions.GetPermissionByValue(s.ClaimValue)))
            //    ;

            CreateMap<AppRole, RoleViewModel>()
                .ForMember(d => d.RoleId, map => map.MapFrom(s => s.Id))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.Name))
                .ForMember(d => d.Description, map => map.MapFrom(s => s.Description))
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                //.ForMember(d => d.UsersCount, map => map.MapFrom(s => s.Users != null ? s.Users.Count : 0))
                .ReverseMap();
            //CreateMap<RoleViewModel, AppRole>()
            //    .ForMember(d => d.Id, map => map.MapFrom(s => s.RoleId))
            //    .ForMember(d => d.Name, map => map.MapFrom(s => s.Name))
            //    .ForMember(d => d.Description, map => map.MapFrom(s => s.Description))
            //    // .ForMember(d => d.Status, map => map.MapFrom(s => s.Active))
            //    .ForMember(d => d.Claims, map => map.MapFrom(s => s.Permissions));

            CreateMap<Customer, CustomerViewModel>()
                .ReverseMap();

        }
    }
}