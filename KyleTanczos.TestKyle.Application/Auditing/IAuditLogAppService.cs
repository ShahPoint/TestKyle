using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using KyleTanczos.TestKyle.Auditing.Dto;
using KyleTanczos.TestKyle.Dto;

namespace KyleTanczos.TestKyle.Auditing
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultOutput<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);

        Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);
    }
}