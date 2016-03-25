using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KyleTanczos.TestKyle.PCR;
using Abp.Application.Services.Dto;
using KyleTanczos.TestKyle.BlobFile.Dto;
using Abp.AutoMapper;

namespace KyleTanczos.TestKyle.BlobFile
{
    public class BlobFileService : TestKyleAppServiceBase, IBlobFileService
    {
        private readonly IRepository<blobFile> _blobFileRepository;

        public BlobFileService(IRepository<blobFile> blobFileRepository)
        {
            _blobFileRepository = blobFileRepository;
        }

        public ListResultOutput<filesDto> GetBlobs()
        {
            var files = _blobFileRepository.GetAllList();
            return new ListResultOutput<filesDto>( files.MapTo<List<filesDto>>());
        }
    }
}
