using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KyleTanczos.TestKyle.PCR;
namespace KyleTanczos.TestKyle.BlobFile.Dto
{
    [AutoMapFrom(typeof(blobFile))]
    //public class PersonListDto : FullAuditedEntityDto
    public class filesDto: Entity
    {
            public string created { get; set; }
            public string fileName { get; set; }
            public int byteCount { get; set; }
    }
}
