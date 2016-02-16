using System.Collections.Generic;
using KyleTanczos.TestKyle.Authorization.Users.Dto;
using KyleTanczos.TestKyle.Dto;

namespace KyleTanczos.TestKyle.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}