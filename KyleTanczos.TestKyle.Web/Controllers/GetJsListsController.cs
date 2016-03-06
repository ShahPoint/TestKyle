using eCloudPCR2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KyleTanczos.TestKyle.Web.Controllers
{
    public class GetJsListsController : ApiController
    {

        public class Select2Option
        {
            public string id { get; set; }
            public string text { get; set; }
        }


        public IHttpActionResult GetPcr(string id) // id represents the resource name to retrieve
        {
            var returnList = new List<string[]>();

            switch (id)
            {
                case "PatientMedicationList":
                    return Ok(new PatientMedicationList().MedicationList.Select(x => (new Select2Option() { text = x[0], id = x[1] })));
            }




            if (returnList == null)
            {
                return NotFound();
            }
            return Ok(returnList);

            //return Ok(list);
        }

        
    }
}
