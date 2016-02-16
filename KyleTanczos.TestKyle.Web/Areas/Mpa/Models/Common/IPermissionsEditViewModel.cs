using System.Collections.Generic;
using KyleTanczos.TestKyle.Authorization.Dto;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}