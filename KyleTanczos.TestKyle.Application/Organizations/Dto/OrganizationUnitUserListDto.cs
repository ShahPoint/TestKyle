﻿using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using KyleTanczos.TestKyle.Authorization.Users;

namespace KyleTanczos.TestKyle.Organizations.Dto
{
    [AutoMapFrom(typeof(User))]
    public class OrganizationUnitUserListDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public DateTime AddedTime { get; set; }
    }
}