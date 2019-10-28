

using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MyXSpace.Core.Entities;

namespace MyXSpace.AppServices.Sessions.Dto
{
    [AutoMapFrom(typeof(AppUser))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
    }
}
