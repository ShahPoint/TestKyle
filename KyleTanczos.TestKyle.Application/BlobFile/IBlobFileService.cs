using Abp.Application.Services;
using Abp.Application.Services.Dto;
using KyleTanczos.TestKyle.BlobFile.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleTanczos.TestKyle.BlobFile
{
    public interface IBlobFileService: IApplicationService
    {
        ListResultOutput<filesDto> GetBlobs() ;
    }
}
