using System.Collections.Generic;
using KyleTanczos.TestKyle.Auditing.Dto;
using KyleTanczos.TestKyle.Dto;

namespace KyleTanczos.TestKyle.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
