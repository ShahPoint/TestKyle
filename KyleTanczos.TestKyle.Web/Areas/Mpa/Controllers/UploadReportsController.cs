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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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



        private List<E14> GetE14(JToken x)
        {
            var e14_list = new List<E14>();

            foreach (var e14_json in x["E14"]) 
            {

                E14 e14 = new E14();

                var something2 = "" + e14_json.Value<string>("E14_01");

                var something = e14_json["E14_01"];

                //var count = something.ToList().Count();

                e14.E14_01 = e14_json.Value<string>("E14_01"); //e14_json.

                e14_list.Add(e14);
            }
            return e14_list;
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
                        pcrJobj => ObjectifyPcr(pcrJobj)
                   );


                    var JsonObject = JsonNetify(something435);

                    string jsonString = JsonObject.ToString();

                    XmlDocument doc = JsonConvert.DeserializeXmlNode(jsonString, "EMSDataSet");

                    string xmlString = doc.OuterXml;


                    var countsomething435 = something435.Count();

                    var aaa = "aaaaa";

                    //List<Record> recordsArray = new List<Record>();

                    //foreach (var record in reportJsonObj2)
                    //{
                    //    Record shitBeGoingDown;

                    //    try
                    //    {
                    //        shitBeGoingDown = record.ToObject<Record>();

                    //        recordsArray.Add(shitBeGoingDown);

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        string temp = ex.Message;
                    //    }

                    //}

                    //var asdfasdf = recordsArray.Count();


                    //EMSDataSet emsDataSet = Newtonsoft.Json.JsonConvert.DeserializeObject<EMSDataSet>(json, 
                    //    new Newtonsoft.Json.JsonSerializerSettings
                    //        {
                    //            Error = HandleDeserializationError
                    //        }
                    //    );

                    //var yyyyy = emsDataSet.Header.Record.First();
                    


    //public class EMSDataSet


                    //var jsonXmlDocument = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json);

                    //var str22 = document.OuterXml;

                    //var byteArray22 = GetBytes(str22);

                    //var len22 = byteArray22.Length;


                    //var elemTemp = xDoc.Descendants("Record");

                    //IEnumerable<XElement> de =
                    //        from el in xDoc.Descendants("Record")
                    //        select el;

                    //var cef = de.Count();

                    //var ccc = elemTemp.Count();

                    //foreach(XElement xElem in elemTemp)
                    //{
                    //    var x = xElem;

                        

                    //}

                    //int count888 = (from p in xDoc.Descendants("Record")
                                 
                    //             select p).Count();



                    //int count = document.GetElementsByTagName("Record").Count;



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

        private object JsonNetify(IEnumerable<Record> pcrObjects)
        {
            var rootObject = new JObject();

            rootObject.Add(new JProperty("@xmlns", "http://www.nemsis.org"));

            rootObject.Add(new JProperty("@xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"));

            rootObject.Add(new JProperty("@xsi:schemaLocation", "http://www.nemsis.org http://www.nemsis.org/media/XSD/EMSDataSet.xsd") );




            var pcrs = new JArray();

            foreach (var pcrObj in pcrObjects)
            {
                JObject pcrJson = new JObject();

                JObject E01 = new JObject();
                E01.Add(CreateJsonProperty("E01_01", pcrObj.E01.E01_01));
                E01.Add(CreateJsonProperty("E01_02", pcrObj.E01.E01_02));
                E01.Add(CreateJsonProperty("E01_03", pcrObj.E01.E01_03));
                E01.Add(CreateJsonProperty("E01_04", pcrObj.E01.E01_04));
                pcrJson.Add( new JProperty("E01", E01 ) );

                JObject E02 = new JObject();
                E02.Add(CreateJsonProperty("E02_01", pcrObj.E02.E02_01));
                E02.Add(CreateJsonProperty("E02_02", pcrObj.E02.E02_02));
                E02.Add(CreateJsonProperty("E02_03", pcrObj.E02.E02_04));
                E02.Add(CreateJsonProperty("E02_05", pcrObj.E02.E02_05));
                E02.Add(CreateJsonArray("E02_06", pcrObj.E02.E02_06));
                E02.Add(CreateJsonArray("E02_07", pcrObj.E02.E02_07));
                E02.Add(CreateJsonArray("E02_08", pcrObj.E02.E02_08));
                E02.Add(CreateJsonArray("E02_09", pcrObj.E02.E02_09));
                E02.Add(CreateJsonArray("E02_10", pcrObj.E02.E02_10));
                E02.Add(CreateJsonProperty("E02_12", pcrObj.E02.E02_12));
                E02.Add(CreateJsonProperty("E02_20", pcrObj.E02.E02_20));
                pcrJson.Add(new JProperty("E02", E02));

                JObject E03 = new JObject();
                E03.Add(CreateJsonProperty("E03_01", pcrObj.E03.E03_01));
                E03.Add(CreateJsonProperty("E03_02", pcrObj.E03.E03_02));
                pcrJson.Add(new JProperty("E03", E03));

                JArray E04 = new JArray();
                E04 = new JArray(
                        from item in pcrObj.E04
                        select new JObject(
                            CreateJsonProperty("E04_01", item.E04_01),
                            CreateJsonProperty("E04_02", item.E04_02),
                            CreateJsonProperty("E04_03", item.E04_03)
                         )
                     );
                pcrJson.Add(new JProperty("E04", E04));


                JObject E05 = new JObject();
                E05.Add(CreateJsonProperty("E05_02", pcrObj.E05.E05_02));
                E05.Add(CreateJsonProperty("E05_03", pcrObj.E05.E05_03));
                E05.Add(CreateJsonProperty("E05_04", pcrObj.E05.E05_04));
                E05.Add(CreateJsonProperty("E05_05", pcrObj.E05.E05_05));
                E05.Add(CreateJsonProperty("E05_06", pcrObj.E05.E05_06));
                E05.Add(CreateJsonProperty("E05_07", pcrObj.E05.E05_07));
                E05.Add(CreateJsonProperty("E05_09", pcrObj.E05.E05_09));
                E05.Add(CreateJsonProperty("E05_10", pcrObj.E05.E05_10));
                E05.Add(CreateJsonProperty("E05_11", pcrObj.E05.E05_11));
                E05.Add(CreateJsonProperty("E05_13", pcrObj.E05.E05_13));
                pcrJson.Add(new JProperty("E05", E05));


                JObject E06 = new JObject();
                E06.Add(CreateJsonProperty("E06_07", pcrObj.E06.E06_07));
                E06.Add(CreateJsonProperty("E06_11", pcrObj.E06.E06_11));
                E06.Add(CreateJsonProperty("E06_12", pcrObj.E06.E06_12));
                E06.Add(CreateJsonProperty("E06_13", pcrObj.E06.E06_13));
                E06.Add(CreateJsonProperty("E06_14", pcrObj.E06.E06_14));
                E06.Add(CreateJsonProperty("E06_15", pcrObj.E06.E06_15));
                E06.Add(CreateJsonProperty("E06_16", pcrObj.E06.E06_16));
                pcrJson.Add(new JProperty("E06", E06));

                JObject E07 = new JObject();
                E07.Add(CreateJsonProperty("E07_01", pcrObj.E07.E07_01));
                E07.Add(CreateJsonProperty("E07_15", pcrObj.E07.E07_15));
                E07.Add(CreateJsonProperty("E07_34", pcrObj.E07.E07_34));
                E07.Add(CreateJsonProperty("E07_35", pcrObj.E07.E07_35));
                pcrJson.Add(new JProperty("E07", E07));

                JObject E08 = new JObject();
                E08.Add(CreateJsonProperty("E08_05", pcrObj.E08.E08_05));
                E08.Add(CreateJsonProperty("E08_06", pcrObj.E08.E08_06));
                E08.Add(CreateJsonProperty("E08_07", pcrObj.E08.E08_07));
                E08.Add(CreateJsonProperty("E08_12", pcrObj.E08.E08_12));
                E08.Add(CreateJsonProperty("E08_13", pcrObj.E08.E08_13));
                E08.Add(CreateJsonProperty("E08_14", pcrObj.E08.E08_14));
                E08.Add(CreateJsonProperty("E08_15", pcrObj.E08.E08_15));
                pcrJson.Add(new JProperty("E08", E08));


                JObject E09 = new JObject();
                E09.Add(CreateJsonArray("E09_01", pcrObj.E09.E09_01));
                E09.Add(CreateJsonProperty("E09_02", pcrObj.E09.E09_02));
                E09.Add(CreateJsonProperty("E09_03", pcrObj.E09.E09_03));
                E09.Add(CreateJsonProperty("E09_04", pcrObj.E09.E09_04));
                E09.Add(CreateJsonProperty("E09_05", pcrObj.E09.E09_05));
                E09.Add(CreateJsonProperty("E09_11", pcrObj.E09.E09_11));
                E09.Add(CreateJsonProperty("E09_12", pcrObj.E09.E09_12));
                E09.Add(CreateJsonProperty("E09_13", pcrObj.E09.E09_13));
                E09.Add(CreateJsonArray("E09_14", pcrObj.E09.E09_14));
                E09.Add(CreateJsonProperty("E09_15", pcrObj.E09.E09_15));
                E09.Add(CreateJsonProperty("E09_16", pcrObj.E09.E09_16));
                pcrJson.Add(new JProperty("E09", E09));

                JObject E10 = new JObject();
                E10.Add(CreateJsonProperty("E10_01", pcrObj.E10.E10_01));
                E10.Add(CreateJsonProperty("E10_02", pcrObj.E10.E10_02));
                E10.Add(CreateJsonArray("E10_04", pcrObj.E10.E10_04));
                E10.Add(CreateJsonArray("E10_08", pcrObj.E10.E10_08));
                E10.Add(CreateJsonArray("E10_09", pcrObj.E10.E10_09));
                E10.Add(CreateJsonProperty("E10_10", pcrObj.E10.E10_10));
                pcrJson.Add(new JProperty("E10", E10));


                JObject E11 = new JObject();
                E11.Add(CreateJsonProperty("E11_01", pcrObj.E11.E11_01));
                E11.Add(CreateJsonProperty("E11_02", pcrObj.E11.E11_02));
                E11.Add(CreateJsonArray("E11_03", pcrObj.E11.E11_03));
                E11.Add(CreateJsonProperty("E11_04", pcrObj.E11.E11_04));
                E11.Add(CreateJsonProperty("E11_05", pcrObj.E11.E11_05));
                E11.Add(CreateJsonProperty("E11_11", pcrObj.E11.E11_11));
                pcrJson.Add(new JProperty("E11", E11));


                JObject E12 = new JObject();
                E12.Add(CreateJsonArray("E12_01", pcrObj.E12.E12_01));
                E12.Add(CreateJsonArray("E12_08", pcrObj.E12.E12_08));
                E12.Add(CreateJsonArray("E12_10", pcrObj.E12.E12_10));
                E12.Add(CreateJsonArray("E12_14", pcrObj.E12.E12_14));
                E12.Add(CreateJsonArray("E12_19", pcrObj.E12.E12_19));
                pcrJson.Add(new JProperty("E12", E12));

                JObject E13 = new JObject();
                E13.Add(CreateJsonProperty("E13_01", pcrObj.E13.E13_01));
                pcrJson.Add(new JProperty("E13", E13));

                JArray E14 = new JArray();
                E14 = new JArray(
                        from item in pcrObj.E14
                        select new JObject(
                            CreateJsonProperty("E14_03", item.E14_03),
                            CreateJsonProperty("E14_04", item.E14_04),
                            CreateJsonProperty("E14_05", item.E14_05),
                            CreateJsonProperty("E14_06", item.E14_06),
                            CreateJsonProperty("E14_07", item.E14_07),
                            CreateJsonProperty("E14_08", item.E14_08),
                            CreateJsonProperty("E14_11", item.E14_11),
                            CreateJsonProperty("E14_15", item.E14_15),
                            CreateJsonProperty("E14_16", item.E14_16),
                            CreateJsonProperty("E14_17", item.E14_17),
                            CreateJsonProperty("E14_27", item.E14_27),
                            CreateJsonProperty("E14_28", item.E14_28)
                         )
                     );
                pcrJson.Add(new JProperty("E14", E14));




                pcrs.Add(pcrJson);


            }

            var aaa = "aaa";

            JObject header = GetHeaderJObject();

            header.Add(new JProperty("Record", pcrs));

            rootObject.Add(new JProperty("Header", header));

            string jsonString = rootObject.ToString();

            return jsonString ;
        }

        private JProperty CreateJsonArray(string nemsisId, List<string> Array)
        {
            return new JProperty(nemsisId, new JArray(Array.ToArray()) );
        }

        private JObject GetHeaderJObject()
        {
            var header = new JObject();

            header.Add("D01_01", "25036");
            header.Add("D01_03", "42");
            header.Add("D01_04", "42049");
            header.Add("D01_07", "6112");
            header.Add("D01_08", "5810");
            header.Add("D01_09", "5870");
            header.Add("D01_21", "1033111554");
            header.Add("D02_07", "16502");

            return header;

        }

        private JProperty CreateJsonProperty(string nemsisId, string value)
        {
            return new JProperty(nemsisId, value);
        }

        private string GetStringValue(JToken jObj, string propertyName)
        {
            if( jObj[propertyName] == null )
                return "";

            if (jObj[propertyName].Type == JTokenType.Object  && jObj[propertyName].Value<string>("@xsi:nil") == "true")
                return null;

            return jObj.Value<string>(propertyName);

        }

        private List<string> GetArrayValue(JToken jObj, string propertyName)
        {
            if (jObj[propertyName] == null)
                return new List<string>();

            var valueList = (from item in jObj[propertyName]
                              select (item.HasValues ? item.Value<string>() : "")
                              ).ToList();

            return valueList;
        }


        private Record ObjectifyPcr(JToken x)
        {
            var pcr = new Record();

            pcr.E01 = new E01() //( x["E01"] == null ? new E01() :  new E01()
            {
                E01_01 = GetStringValue ( x["E01"], ("E01_01") ),
                E01_02 = GetStringValue ( x["E01"],("E01_02") ),
                E01_03 = GetStringValue ( x["E01"],("E01_03") ),
                E01_04 = GetStringValue ( x["E01"],("E01_04") )
            }; //})
                       
                pcr.E02 = new E02()
                {
                    E02_01 = GetStringValue ( x["E02"], ("E02_01") ),
                    E02_02 = GetStringValue ( x["E02"], ("E02_02") ),
                    E02_03 = GetStringValue ( x["E02"], ("E02_03") ),
                    E02_04 = GetStringValue ( x["E02"], ("E02_04") ),
                    E02_05 = GetStringValue ( x["E02"], ("E02_05") ),
                    E02_06 = GetArrayValue ( x["E02"], ("E02_06") ),
                    E02_07 = GetArrayValue ( x["E02"], ("E02_07") ),
                    E02_08 = GetArrayValue( x["E02"], ("E02_08")),
                    E02_09 = GetArrayValue ( x["E02"], ("E02_09") ),
                    E02_10 = GetArrayValue ( x["E02"], ("E02_10") ),
                    E02_11 = GetStringValue ( x["E02"], ("E02_11") ),
                    E02_12 = GetStringValue ( x["E02"], ("E02_12") ),
                    E02_13 = GetStringValue ( x["E02"], ("E02_13") ),
                    E02_14 = GetStringValue ( x["E02"], ("E02_14") ),
                    E02_16 = GetStringValue ( x["E02"], ("E02_16") ),
                    E02_17 = GetStringValue ( x["E02"], ("E02_17") ),
                    E02_18 = GetStringValue ( x["E02"], ("E02_18") ),
                    E02_19 = GetStringValue ( x["E02"], ("E02_19") ),
                    E02_20 = GetStringValue ( x["E02"], ("E02_20") )
                };
                       
                pcr.E03 = new E03()
                {
                    E03_01 = x["E03"].Value<string>("E03_01"),
                    E03_02 = x["E03"].Value<string>("E03_02")
                };
                       
                pcr.E04 = (x["E04"].HasValues ?



                       (
                               x["E04"].Type == JTokenType.Array
                                   ?
                               x["E04"].Select(y => ObjectifyE04(y)).ToList()
                                   : // else Type = JTokenType.Object
                               new List<E04>() { ObjectifyE04(x["E04"]) }

                           )

                       :
                       new List<E04>()
                       );

                       //x["E04"].Select(y => new E04()
                       //{
                       //    E04_01 = y.Value<string>("E04_01"),
                       //    E04_02 = y.Value<string>("E04_02"),
                       //    E04_03 = y.Value<string>("E04_03")
                       //}).ToList()

                       //: new List<E04>() )


                       
                pcr.E05 = new E05()
                {
                    E05_01 = GetStringValue( x["E05"], ("E05_01") ),
                    E05_02 = GetStringValue( x["E05"], ("E05_02") ),
                    E05_03 = GetStringValue( x["E05"], ("E05_03") ),
                    E05_04 = GetStringValue( x["E05"], ("E05_04") ),
                    E05_05 = GetStringValue( x["E05"], ("E05_05") ),
                    E05_06 = GetStringValue( x["E05"], ("E05_06") ),
                    E05_07 = GetStringValue( x["E05"], ("E05_07") ),
                    E05_08 = GetStringValue( x["E05"], ("E05_08") ),
                    E05_09 = GetStringValue( x["E05"], ("E05_09") ),
                    E05_10 = GetStringValue( x["E05"], ("E05_10") ),
                    E05_11 = GetStringValue( x["E05"], ("E05_11") ),
                    E05_12 = GetStringValue( x["E05"], ("E05_12") ),
                    E05_13 = GetStringValue( x["E05"], ("E05_13") )
                };
                       
                pcr.E06 = new E06()
                {
                    E06_07 = GetStringValue( x["E06"], ("E06_07") ),
          //NOTE: below patient zip needs a pattern to extract out of all the address object
                    //E06_08 = GetStringValue(x["E06"], ("E06_08")),
                    E06_11 = GetStringValue( x["E06"], ("E06_11") ),
                    E06_12 = GetStringValue( x["E06"], ("E06_12") ),
                    E06_13 = GetStringValue( x["E06"], ("E06_13") ),
                    E06_14 = GetStringValue( x["E06"], ("E06_14") ),
                    E06_15 = GetStringValue( x["E06"], ("E06_15") ),
                    E06_16 = GetStringValue( x["E06"], ("E06_16") ),
                    E06_17 = GetStringValue( x["E06"], ("E06_17") )
                };

                pcr.E07 = new E07()
                {
                    E07_01 = GetStringValue( x["E07"], ("E07_01") ),
                    E07_15 = GetStringValue( x["E07"], ("E07_15") ),
                    E07_32 = GetStringValue( x["E07"], ("E07_32") ),
                    E07_34 = GetStringValue( x["E07"], ("E07_34") ),      
                    E07_35 = GetStringValue( x["E07"], ("E07_35") ),
                };
                       
                pcr.E08 = new E08()
                {
                    E08_02 = GetStringValue( x["E08"], ("E08_02") ),
                    E08_05 = GetStringValue( x["E08"], ("E08_05") ),
                    E08_06 = GetStringValue( x["E08"], ("E08_06") ),
                    E08_07 = GetStringValue( x["E08"], ("E08_07") ),
                    E08_08 = GetStringValue( x["E08"], ("E08_08") ),
                    E08_09 = GetStringValue( x["E08"], ("E08_09") ),
                    E08_12 = GetStringValue(x["E08"], ("E08_12")),
                    E08_13 = GetStringValue( x["E08"], ("E08_13") ),
                    E08_14 = GetStringValue(x["E08"], ("E08_14")),
                    E08_15 = GetStringValue(x["E08"], ("E08_15")),
                };

            pcr.E09 = new E09()
            {
                E09_01 = GetArrayValue( x["E09"], ("E09_01") ),
                E09_02 = GetStringValue( x["E09"], ("E09_02") ),
                E09_03 = GetStringValue( x["E09"], ("E09_03") ),
                E09_04 = GetStringValue( x["E09"], ("E09_04") ),
                E09_05 = GetStringValue( x["E09"], ("E09_05") ),
                E09_08 = GetStringValue( x["E09"], ("E09_08") ),
                E09_11 = GetStringValue( x["E09"], ("E09_11") ),
                E09_12 = GetStringValue( x["E09"], ("E09_12") ),
                E09_13 = GetStringValue( x["E09"], ("E09_13") ),
                E09_14 = GetArrayValue( x["E09"], ("E09_14") ),
                E09_15 = GetStringValue( x["E09"], ("E09_15") ),
                E09_16 = GetStringValue( x["E09"], ("E09_16") )

            };
                           
                pcr.E10 = new E10()
                {
                    E10_01 = GetStringValue( x["E10"], ("E10_01") ),
                    E10_02 = GetStringValue( x["E10"], ("E10_02") ),
                    E10_03 = GetArrayValue( x["E10"], ("E10_03") ),
                    E10_04 = GetArrayValue(x["E10"], ("E10_04")),
                    E10_08 = GetArrayValue(x["E10"], ("E10_08")),
                    E10_09 = GetArrayValue(x["E10"], ("E10_09")),
                    E10_10 = GetStringValue( x["E10"], ("E10_10") )

                };
                pcr.E11 = new E11()
                {
                    E11_01 = GetStringValue( x["E11"], ("E11_01") ),
                    E11_02 = GetStringValue( x["E11"], ("E11_02") ),
                    E11_03 = GetArrayValue( x["E11"], ("E11_03") ),
                    E11_04 = GetStringValue( x["E11"], ("E11_04") ),
                    E11_05 = GetStringValue( x["E11"], ("E11_05") ),
                    E11_06 = GetStringValue( x["E11"], ("E11_06") ),
                    E11_07 = GetStringValue( x["E11"], ("E11_07") ),
                    E11_08 = GetStringValue( x["E11"], ("E11_08") ),
                    E11_11 = GetStringValue( x["E11"], ("E11_11") )

                };

            pcr.E12 = new E12()
                {
                    E12_01 = GetArrayValue( x["E12"], ("E12_01") ),
                    E12_08 = GetArrayValue( x["E12"], ("E12_08") ),
                    E12_10 = GetArrayValue( x["E12"], ("E12_10") ),
                    E12_14 = GetArrayValue( x["E12"], ("E12_14") ),
                    E12_19 = GetArrayValue( x["E12"], ("E12_19") ),
                    E12_20 = GetStringValue( x["E12"], ("E12_20") ),


                };

                pcr.E13 = new E13()
                {
                    E13_01 = x["E13"].Value<string>("E13_01"),

                };

                pcr.E14 = (x["E14"].HasValues ?

                           //GetE14(x)

                           (
                               x["E14"].Type == JTokenType.Array
                                   ?
                               x["E14"].Select(y => ObjectifyE14(y)).ToList()
                                   : // else Type = JTokenType.Object
                               new List<E14>() { ObjectifyE14(x["E14"]) }

                           )
                                   : new List<E14>()

                               );

            if (x["E15"].Children().Count() > 0)
            {
                pcr.E15 = new E15()
                {
                    E15_01 = GetArrayValue(x["E15"], "E15_01"),
                    E15_02 = GetArrayValue(x["E15"], "E15_02"),
                    E15_03 = GetArrayValue(x["E15"], "E15_03"),
                    E15_04 = GetArrayValue(x["E15"], "E15_04"),
                    E15_05 = GetArrayValue(x["E15"], "E15_05"),
                    E15_06 = GetArrayValue(x["E15"], "E15_06"),
                    E15_07 = GetArrayValue(x["E15"], "E15_07"),
                    E15_08 = GetArrayValue(x["E15"], "E15_08"),
                    E15_09 = GetArrayValue(x["E15"], "E15_09"),
                    E15_10 = GetArrayValue(x["E15"], "E15_10"),
                    E15_11 = GetArrayValue(x["E15"], "E15_11")
                };
            }
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

            //pcr.E19 = new E19()
            //{

            //    E19_02 = x["E19"].Value<string>("E19_02"),
            //    E19_13 = x["E19"].Value<string>("E19_13"),
            //    E19_14 = x["E19"].Value<string>("E19_14"),

            //};

            pcr.E20 = new E20()
            {
                E20_02 = GetStringValue( x["E20"], "E20_02"),
                E20_07 = GetStringValue( x["E20"], "E20_07"),
                E20_10 = GetStringValue( x["E20"], "E20_10"),
                E20_14 = GetStringValue( x["E20"], "E20_14"),
                E20_15 = GetStringValue( x["E20"], "E20_15"),
                E20_16 = GetStringValue( x["E20"], "E20_16"),
                E20_17 = GetStringValue( x["E20"], "E20_17")
            };

            pcr.E22 = new E22()
            {
                E22_01 = GetStringValue( x["E22"], "E22_01"),
                E22_02 = GetStringValue( x["E22"], "E22_02"),
                E22_06 = GetStringValue( x["E22"], "E22_06")

            };

            pcr.E23 = new E23()
            {
                E23_03 = GetStringValue( x["E23"], "E23_01"),
                E23_06 = GetStringValue( x["E23"], "E23_06"),
                E23_09 = GetStringValue( x["E23"], "E23_09"),
                E23_10 = GetStringValue( x["E23"], "E23_10"),
                E23_11 = GetStringValue(x["E23"], "E23_11")

            };

            return pcr;
        }

        private E04 ObjectifyE04(JToken e04jobj)
        {
            return new E04()
            {
                E04_01 = GetStringValue( e04jobj, "E04_01"),
                E04_02 = GetStringValue( e04jobj, "E04_02"),
                E04_03 = GetStringValue( e04jobj, "E04_03")
            };
        }

        private E14 ObjectifyE14(JToken e14jobj)
        {
            return new E14()
            {
   
                E14_02 = GetStringValue( e14jobj, "E14_02"),
                E14_03 = GetStringValue( e14jobj, "E14_03"),
                E14_04 = GetStringValue( e14jobj, "E14_04"),
                E14_05 = GetStringValue(e14jobj, "E14_05"),
                E14_06 = GetStringValue(e14jobj, "E14_06"),
                E14_07 = GetStringValue(e14jobj, "E14_07"),
                E14_08 = GetStringValue(e14jobj, "E14_08"),

                E14_11 = GetStringValue(e14jobj, "E14_11"),
                E14_15 = GetStringValue(e14jobj, "E14_15"),
                E14_16 = GetStringValue(e14jobj, "E14_16"),
                E14_17 = GetStringValue(e14jobj, "E14_17"),
                E14_27 = GetStringValue(e14jobj, "E14_27"),
                E14_28 = GetStringValue(e14jobj, "E14_28")

            };
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