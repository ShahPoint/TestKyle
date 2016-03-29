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

using System.Xml.Linq;

using Microsoft.AspNet.Identity;

using KyleTanczos.TestKyle.Web.Models.App;

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
        public void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }

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

                    var records = document.GetElementsByTagName("Record");

                    


                    XDocument xDoc = XDocument.Load(new XmlNodeReader(document));

                    string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(document);

                    int csdf = json.Count();

                    json = json.Replace(@"{""?xml"":{""@version"":""1.0"",""@encoding"":""UTF-8""},""EMSDataSet"":", "");

                    json = json.Substring(0, json.Length - 1);

                    Newtonsoft.Json.Linq.JObject jsonObj  = Newtonsoft.Json.Linq.JObject.Parse(json);

                    var reportJsonObj1 = jsonObj["Header"];

                    var reportJsonObj2 = reportJsonObj1["Record"];

                    var something435 = reportJsonObj2.Select(
                        x => 
                        new Record()
                        {
                            E01 = new E01() //( x["E01"] == null ? new E01() :  new E01()
                            {
                                E01_01 = x["E01"].Value<string>("E01_01"),
                                E01_02 = x["E01"].Value<string>("E01_02"),
                                E01_03 = x["E01"].Value<string>("E01_03"),
                                E01_04 = x["E01"].Value<string>("E01_04")
                            } //})
                            ,
                            E02 = new E02()
                            {
                                E02_01 = x["E02"].Value<string>("E02_01"),
                                E02_02 = x["E02"].Value<string>("E02_02"),
                                E02_03 = x["E02"].Value<string>("E02_03"),
                                E02_04 = x["E02"].Value<string>("E02_04"),
                                E02_05 = x["E02"].Value<string>("E02_05"),
                                E02_06 = x["E02"].Value<string>("E02_06"),
                                E02_07 = x["E02"].Value<string>("E02_07"),
                                E02_08 = x["E02"]["E02_08"].HasValues ? 
                                                        x["E02"]["E02_08"].Values<string>().ToList()
                                                        : new List<string>(),
                                E02_09 = x["E02"].Value<string>("E02_09"),
                                E02_10 = x["E02"].Value<string>("E02_10"),
                                E02_11 = x["E02"].Value<string>("E02_11"),
                                E02_12 = x["E02"].Value<string>("E02_12"),
                                E02_13 = x["E02"].Value<string>("E02_13"),
                                E02_14 = x["E02"].Value<string>("E02_14"),
                                E02_16 = x["E02"].Value<string>("E02_16"),
                                E02_17 = x["E02"].Value<string>("E02_17"),
                                E02_18 = x["E02"].Value<string>("E02_18"),
                                E02_19 = x["E02"].Value<string>("E02_19"),
                                E02_20 = x["E02"].Value<string>("E02_20")
                            }
                            ,
                            E03 = new E03()
                            {
                                E03_01 = x["E03"].Value<string>("E03_01"),
                                E03_02 = x["E03"].Value<string>("E03_02")
                            }
                            ,
                            E04 = (x["E04"].HasValues ?

                            x["E04"].Select(y => new E04()
                            {
                                E04_01 = y.Value<string>("E04_01"),
                                E04_02 = y.Value<string>("E04_02"),
                                E04_03 = y.Value<string>("E04_03")
                            }).ToList()

                            : new List<E04>() )


                            ,
                            E05 = new E05()
                            {
                                E05_01 = x["E05"].Value<string>("E05_01"),
                                E05_02 = x["E05"].Value<string>("E05_02"),
                                E05_03 = x["E05"].Value<string>("E05_03"),
                                E05_04 = x["E05"].Value<string>("E05_04"),
                                E05_05 = x["E05"].Value<string>("E05_05"),
                                E05_06 = x["E05"].Value<string>("E05_06"),
                                //E05_07 = x["E05"].Value<string>("E05_01"),
                                //E05_08 = x["E05"].Value<string>("E05_01"),
                                //E05_09 = x["E05"].Value<string>("E05_01"),
                                //E05_10 = x["E05"].Value<string>("E05_01"),
                                E05_11 = x["E05"].Value<string>("E05_11"),
                                E05_12 = x["E05"].Value<string>("E05_12"),
                                E05_13 = x["E05"].Value<string>("E05_13")
                            }
                            ,
                            E06 = new E06()
                            {
                                E06_01_0 = new E06_01_0(),
                                E06_04_0 = new E06_04_0(),
                                E06_06 = x["E06"].Value<string>("E06_01"),
                                E06_11 = x["E06"].Value<string>("E06_01"),
                                E06_12 = x["E06"].Value<string>("E06_01"),
                                E06_13 = x["E06"].Value<string>("E06_01"),
                                //E06_14_0 = x["E06"].Value<string>("E06_01"),
                                E06_16 = x["E06"].Value<string>("E06_01"),
                                E06_17 = x["E06"].Value<string>("E06_01")
                            },
                            E07 = new E07()
                            {
                                E07_01 = x["E07"].Value<string>("E07_01"),
                                E07_15 = x["E07"].Value<string>("E07_15"),
                                //E07_18_0 = x["E07"].Value<string>("E07_18"),
                                //E07_27_0 = x["E07"].Value<string>("E07_27"),
                                E07_32 = x["E07"].Value<string>("E07_32"),
                                E07_34 = x["E07"].Value<string>("E07_34"),
                                //E07_35_0 = x["E07"].Value<string>("E07_35"),
                            }
                            ,
                            E08 = new E08()
                            {
                                E08_02 = x["E08"].Value<string>("E08_02"),
                                E08_05 = x["E08"].Value<string>("E08_05"),
                                E08_06 = x["E08"].Value<string>("E08_06"),
                                E08_07 = x["E08"].Value<string>("E08_07"),
                                E08_08 = x["E08"].Value<string>("E08_08"),
                                E08_09 = x["E08"].Value<string>("E08_09"),
                                //E08_10 = x["E08"].Value<string>("E08_10"),
                                //E08_11_0 = x["E08"].Value<string>("E08_02"),
                                E08_13 = x["E08"].Value<string>("E08_02"),
                            },
                            E09 = new E09()
                            {
                                //E09_01 = x["E09"].Value<string>("E09_01"),
                                E09_02 = x["E09"].Value<string>("E09_02"),
                                E09_03 = x["E09"].Value<string>("E09_03"),
                                E09_04 = x["E09"].Value<string>("E09_04"),
                                E09_05 = x["E09"].Value<string>("E09_05"),
                                E09_08 = x["E09"].Value<string>("E09_08"),
                                E09_11 = x["E09"].Value<string>("E09_11"),
                                E09_12 = x["E09"].Value<string>("E09_12"),
                                E09_13 = x["E09"].Value<string>("E09_13"),
                                E09_14 = x["E09"].Value<string>("E09_14"),
                                E09_15 = x["E09"].Value<string>("E09_15"),
                                E09_16 = x["E09"].Value<string>("E09_16")

                                }
                                ,
                            E10 = new E10()
                            {
                                E10_01 = x["E10"].Value<string>("E10_01"),
                                E10_02 = x["E10"].Value<string>("E10_02"),
                                E10_03 = x["E10"].Value<string>("E10_03"),
                                E10_10 = x["E10"].Value<string>("E10_10")

                            },
                            E11 = new E11()
                            {
                                E11_01 = x["E11"].Value<string>("E11_01"),
                                E11_02 = x["E11"].Value<string>("E11_02"),
                                //E11_03 = x["E11"].Value<string>("E11_03"),
                                E11_04 = x["E11"].Value<string>("E11_04"),
                                E11_05 = x["E11"].Value<string>("E11_05"),
                                E11_06 = x["E11"].Value<string>("E11_06"),
                                E11_07 = x["E11"].Value<string>("E11_07"),
                                E11_08 = x["E11"].Value<string>("E11_08"),
                                E11_11 = x["E11"].Value<string>("E11_11")

                            },
                            E12 = new E12()
                            {
                                E12_01 = x["E12"].Value<string>("E12_01"),
                                //E12_08 = x["E12"].Value<string>("E12_08"),
                                //E12_09 = x["E12"].Value<string>("E12_09"),
                                //E12_10 = x["E12"].Value<string>("E12_10"),
                                //E12_14_0 = x["E12"].Value<string>("E12_14_0"),
                                E12_19 = x["E12"].Value<string>("E12_19"),
                                E12_20 = x["E12"].Value<string>("E12_20"),
                                //E12_4_0 = x["E12"].Value<string>("E12_4_0"),

                            },
                            E13 = new E13()
                            {
                                E13_01 = x["E13"].Value<string>("E13_01"),

                            }
                            //,
                            //E14 = (x["E14"].HasValues ?

                            //    x["E14"].Select(y => new E14()
                            //    {
                            //        E14_01 = y.Value<string>("E14_01"),
                            //        E14_02 = y.Value<string>("E14_02"),
                            //        E14_03 = y.Value<string>("E14_03"),
                            //        //E14_04_0 = y.Value<string>("E14_04_0"),
                            //        E14_07 = y.Value<string>("E14_07"),
                            //        E14_08 = y.Value<string>("E14_08"),
                            //        E14_09 = y.Value<string>("E14_09"),
                            //        E14_10 = y.Value<string>("E14_10"),
                            //        E14_11 = y.Value<string>("E14_11"),
                            //        E14_12 = y.Value<string>("E14_12"),
                            //        //E14_13 = y.Value<string>("E14_13"),
                            //        E14_14 = y.Value<string>("E14_14"),
                            //        //E14_15_0 = y.Value<string>("E14_15_0"),
                            //        E14_19 = y.Value<string>("E14_19"),
                            //        E14_22 = y.Value<string>("E14_22"),
                            //        E14_23 = y.Value<string>("E14_23"),
                            //        E14_27 = y.Value<string>("E14_27")

                            //    }).ToList()

                            //    : new List<E14>()

                            //)
                            //, 
                            //E15 = new E15()
                            // {
                            //     //E15_02 = x["E15"].Value<string>("E15_02"),
                            //     E15_03 = x["E15"].Value<string>("E15_03"),
                            //     E15_05 = x["E15"].Value<string>("E15_05"),
                            //     E15_06 = x["E15"].Value<string>("E15_06"),
                            //     E15_07 = x["E15"].Value<string>("E15_07"),
                            //     E15_08 = x["E15"].Value<string>("E15_08"),
                            //     E15_09 = x["E15"].Value<string>("E15_09"),
                            //     //E15_10 = x["E15"].Value<string>("E15_10"),
                            //     E15_11 = x["E15"].Value<string>("E15_11"),

                            // }
                            // ,
                            //E16 = new E16()
                            //{
                            //    //E16_00_0 = x["E16"].Value<string>("E16_00_0"),
                            //    E16_01 = x["E16"].Value<string>("E16_01"),

                            //}
                            ////,
                            ////E17 = (x["E17"].HasValues ?

                            ////    x["E17"].Select(y => new E17()
                            ////    {
                            ////         E17_01 = y.Value<string>("E17_01")

                            ////    }).ToList()

                            ////    : new List<E17>())

                            ////, E18 = (x["E18"].HasValues ?

                            ////    x["E18"].Select(y => new E18()
                            ////    {
                            ////         E18_01 = y.Value<string>("E18_01"),
                            ////        E18_02 = y.Value<string>("E18_02"),
                            ////        E18_03 = y.Value<string>("E18_03"),
                            ////        E18_04 = y.Value<string>("E18_04"),
                            ////        //E18_05_0 = y.Value<string>("E18_05_0"),
                            ////        E18_08 = y.Value<string>("E18_08"),
                            ////        E18_09 = y.Value<string>("E18_09"),
                            ////        E18_10 = y.Value<string>("E18_10"),
                            ////        E18_11 = y.Value<string>("E18_11"),

                            ////    }).ToList()

                            ////    : new List<E18>())

                            //, E19 = new E19()
                            //{
                            //     //E19_01_0 = x["E19"].Value<string>("E19_11_0"),
                            //     //E19_12 = x["E19"].Value<string>("E19_12"),
                            //     E19_13 = x["E19"].Value<string>("E19_13"),
                            //     E19_14 = x["E19"].Value<string>("E19_14"),

                            //}, E20 = new E20()
                            //{
                            //    E20_01 = x["E20"].Value<string>("E20_01"),
                            //    E20_02 = x["E20"].Value<string>("E20_02"),
                            //    //E20_03_0 = x["E20"].Value<string>("E20_03"),
                            //    E20_06 = x["E20"].Value<string>("E20_06"),
                            //    //E20_08 = x["E20"].Value<string>("E20_08"),
                            //    E20_09 = x["E20"].Value<string>("E20_09"),
                            //    E20_10 = x["E20"].Value<string>("E20_10"),
                            //    E20_14 = x["E20"].Value<string>("E20_14"),
                            //    E20_15 = x["E20"].Value<string>("E20_15"),
                            //    E20_16 = x["E20"].Value<string>("E20_16"),
                            //    E20_17 = x["E20"].Value<string>("E20_17")
                            //}

                            ////, E22 = new E22()
                            ////{
                            ////    E22_01 = x["E22"].Value<string>("E22_01"),
                            ////    E22_02 = x["E22"].Value<string>("E22_02"),
                            ////    E22_06 = x["E22"].Value<string>("E22_06")

                            ////}, E23 = new E23()
                            ////{
                            ////    E23_03 = x["E23"].Value<string>("E23_01"),
                            ////    E23_06 = x["E23"].Value<string>("E23_06"),
                            ////    //E23_09_0 = x["E23"].Value<string>("E23_09_0"),
                            ////    E23_10 = x["E23"].Value<string>("E23_10")

                            ////}
                        });

                    var countsomething435 = something435.Count();


                    List<Record> recordsArray = new List<Record>();

                    foreach (var record in reportJsonObj2)
                    {
                        Record shitBeGoingDown;

                        try
                        {
                            shitBeGoingDown = record.ToObject<Record>();

                            recordsArray.Add(shitBeGoingDown);

                        }
                        catch (Exception ex)
                        {
                            string temp = ex.Message;
                        }

                    }

                    var asdfasdf = recordsArray.Count();


                    //EMSDataSet emsDataSet = Newtonsoft.Json.JsonConvert.DeserializeObject<EMSDataSet>(json, 
                    //    new Newtonsoft.Json.JsonSerializerSettings
                    //        {
                    //            Error = HandleDeserializationError
                    //        }
                    //    );

                    //var yyyyy = emsDataSet.Header.Record.First();
                    


    //public class EMSDataSet


                    var jsonXmlDocument = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json);

                    var str22 = document.OuterXml;

                    var byteArray22 = GetBytes(str22);

                    var len22 = byteArray22.Length;


                    var elemTemp = xDoc.Descendants("Record");

                    IEnumerable<XElement> de =
                            from el in xDoc.Descendants("Record")
                            select el;

                    var cef = de.Count();

                    var ccc = elemTemp.Count();

                    foreach(XElement xElem in elemTemp)
                    {
                        var x = xElem;

                        

                    }

                    int count888 = (from p in xDoc.Descendants("Record")
                                 
                                 select p).Count();



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
                    
                    //var records = document.GetElementsByTagName("Record");

                    ////List<PcrPaNemsis> PcrsTemp = new List<PcrPaNemsis>();

                    //E01_01 = node.SelectNodes("");

                    //foreach (XmlElement node in records)
                    //{
                    //    var xml = node.OuterXml;

                    //    //foreach(XmlNode node in node.ChildNodes

                    //    //var elements11 = node.we;

                    //    //var cc2345 = elements11.Count;

                    //    var sffsds = node.GetElementsByTagName("E01");

                    //    var xPath13 = node.SelectSingleNode("//Record");

                    //    var xPath134 = node.SelectSingleNode("//Record");

                    //    var jjjj = node.GetElementsByTagName("E05_04");

                    //    var xPath1 = node.SelectNodes("//Record");

                    //    var xPath2 = node.SelectNodes("//E01");

                    //    var tempPcr = new PcrPaNemsis();

                    //    XmlNodeList nodes;

                    //    nodes = node.GetElementsByTagName("E01_01");

                    //    tempPcr.E01_01 = nodes[0] == null ? "" : nodes[0].InnerText;

                    //    nodes = node.GetElementsByTagName("E01_02");

                    //    tempPcr.E01_02 = nodes[0] == null ? "" : nodes[0].InnerText;


                    //    PcrsTemp.Add(tempPcr);
                    //}


                    var maxVar = dateTimeList.Max();

                    var minVar = dateTimeList.Min();

                    var countVar = dateTimeList.Count();

                    var str = document.OuterXml;

                    var byteArray = GetBytes(str);

                    var len = byteArray.Length;

                    //var fileCount = db.UploadedFiles.Count();

                    //var largeFiles = db.UploadedFiles.Where(x => x.Count > 1000).ToList();

                    //var countBlob = db.UploadedFiles.Where(x => x.Count > 1000 && x.file != null).ToList();

                    var uploadedFileTemp = new UploadedFile()
                    {
                        ByteCount = len,
                        
                        CreatedByUserName = User.Identity.Name ,
                        CreatedByUserId = int.Parse( User.Identity.GetUserId() ),
                        Created = DateTime.UtcNow,
                        EndDateRange = maxVar,
                        StartDateRange = minVar,
                        FileName = file.FileName,
                        TripCount = countVar
                        //, Pcrs = PcrsTemp

                        , RawFile = new RawFile() { }//FileContents = byteArray }
                    };


                    db.UploadedFiles.Add(uploadedFileTemp );


                    //var largeBlobs = db.blobFiles.Where(x => x.byteCount > 3000000).Count();

                    //db.SaveChangesAsync();

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
            var fileInfoList = db.UploadedFiles.Select(x => new filesDTO() { Created = x.Created, ByteCount = x.ByteCount, FileName = x.FileName, Id = x.Id, CreatedByUserName = x.CreatedByUserName, TripCount = x.TripCount, StartDateRange = x.StartDateRange, EndDateRange = x.EndDateRange }).ToList();

            //var int1 = db.Database.ExecuteSqlCommand("TRUNCATE TABLE [RawFiles]");

            //var int2 = db.Database.ExecuteSqlCommand("TRUNCATE TABLE [PcrPaNemsis]");

            //var int3 = db.Database.ExecuteSqlCommand("TRUNCATE TABLE [UploadedFiles]");

            //var uploadedFILE  = db.blobFiles.FirstOrDefault();

            //var ccc = db.blobFiles.Count();

            //foreach (var entity in db.blobFiles)

            //db.blobFiles.Remove(uploadedFILE);

            //db.SaveChanges();


            //var fileInfoList = db.blobFiles.Select(x => new filesDTO() { /*Created = x.Created, ByteCount = x.ByteCount, FileName = x.FileName,*/ Id = x.Id  /* , CreatedByUserName = x.CreatedByUserName, TripCount = x.TripCount, StartDateRange = x.StartDateRange, EndDateRange = x.EndDateRange*/ }).ToList();

            

            return View(fileInfoList);
        }

        public class filesDTO
        {
            public int Id { get; set; }
            public DateTime Created { get; set; }
            public string CreatedByUserName { get; set; }
            public int CreatedByUserId { get; set; }
            public string FileName { get; set; }
            public DateTime StartDateRange { get; set; }
            public DateTime EndDateRange { get; set; }
            public int TripCount { get; set; }
            public int ByteCount { get; set; }
        } 
    }
}