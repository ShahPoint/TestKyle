using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KyleTanczos.TestKyle.Web.Models.App
{
    public class UploadedFile
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        [Column(TypeName = "text")]
        public string RawFile { get; set; }
        public string RawXml { get; set; }
        public string FileName { get; set; }
        public DateTime StartDateRange { get; set; }
        public DateTime EndDateRange { get; set; }
        public int Count { get; set; }
        public List<UploadedPcr> Pcrs { get; set; }
        public blobFile file { get; set; }
    }

    public class blobFile
    {
        public int Id { get; set; }
        public byte[] fileContents2 { get; set; }
        public int byteCount { get; set; }
        public DateTime created { get; set; }
        public string fileName { get; set; }
    }

    public class UploadedPcr
    {
        public int Id { get; set; }
        public DateTime IncidentDate { get; set; }
        public OutComeType outcome { get; set; }
    }

    public class OutComeType
    {
        public int Id { get; set; }
        public string OutcomeName { get; set; }
    }
    
}