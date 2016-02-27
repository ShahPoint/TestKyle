using KyleTanczos.TestKyle.Web.Controllers;
using KyleTanczos.TestKyle.Web.Models.App;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class UploadReportsController : TestKyleControllerBase
    {
        AppContextDb db = new AppContextDb();

        
       

        //[HttpPost]
        //public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        //{
        //    foreach (var file in files)
        //    {
        //        var filename = Path.Combine(Server.MapPath("~/App_Data"), file.FileName);
        //        file.SaveAs(filename);
        //    }
        //    return Json(files.Select(x => new { name = x.FileName }));
        //}


        /// <summary>
        /// to Save DropzoneJs Uploaded Files
        /// </summary>
        public ActionResult SaveDropzoneJsUploadedFiles()
        {
            bool isSavedSuccessfully = false;




            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];



                if (file != null && file.ContentLength > 0 && file.ContentType == "text/xml")
                {
                    var document = new XmlDocument();
                    document.Load(file.InputStream);

                    int count = document.GetElementsByTagName("Record").Count;



                    var e5_04_nodeList = document.GetElementsByTagName("E05_04");

                    var dateTimeList = new List<DateTime>();

                    foreach (XmlNode item in e5_04_nodeList)
                    {
                        //var temp = DateTime.Parse( item.InnerText );

                        var incidentDateTime = DateTime.ParseExact(item.InnerText,
                                                       "yyyy-MM-dd'T'HH:mm:ss.f'Z'",
                                                       CultureInfo.InvariantCulture,
                                                       DateTimeStyles.AssumeUniversal |
                                                       DateTimeStyles.AdjustToUniversal);

                        dateTimeList.Add(incidentDateTime);

                    }

                    var records = document.GetElementsByTagName("Record");



                    var maxVar = dateTimeList.Max();

                    var minVar = dateTimeList.Min();

                    var countVar = dateTimeList.Count();

                    var str = document.OuterXml;

                    var byteArray = GetBytes(str);

                    var len = byteArray.Length;

                    //var fileCount = db.UploadedFiles.Count();

                    //var largeFiles = db.UploadedFiles.Where(x => x.Count > 1000).ToList();

                    //var countBlob = db.UploadedFiles.Where(x => x.Count > 1000 && x.file != null).ToList();

                    db.blobFiles.Add(
                            new blobFile()
                            {
                                fileContents2 = byteArray,
                                byteCount = len
                            });

                    var largeBlobs = db.blobFiles.Where(x => x.byteCount > 3000000).Count();

                    db.SaveChanges();

                    var dropZoneDto = new dropZoneDTO()
                    {
                        count = countVar,
                        min = minVar.ToString("MM/dd/yyyy"),
                        max = maxVar.ToString("MM/dd/yyyy")

                    };

                    return Json(new { Message = dropZoneDto });
                }

                
            }

            return new HttpNotFoundResult("file not valid");
        }


        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public class dropZoneDTO
        {
            public string min { get; set; }
            public string max { get; set; }
            public int count { get; set; }
        }

        // GET: Mpa/UploadReports
        public ActionResult Index()
        {
            var fileInfoList = db.blobFiles.Select(x => new filesDTO() { byteCount = x.byteCount, fileName = x.Id.ToString() }).ToList();

            return View(fileInfoList);
        }

        public class filesDTO
        {
            public string fileName { get; set; }
            public int byteCount { get; set; }
        } 
    }
}