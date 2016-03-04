using KyleTanczos.TestKyle.EntityFramework;
using KyleTanczos.TestKyle.Web.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class PcrForm
    {
        public PcrForm()
        {
            Tabs = new List<Tab>();
        }

        public List<Tab> Tabs { get; set; }
    }

    public class Tab
    {
        public Tab()
        {
            Sections = new List<Section>();
        }

        public List<Section> Sections { get; set; }
    }

    public class Section
    {
        public Section()
        {
            Controls = new List<Ctrl>();
            NgWidth = 6;
        }
        public string SectionName { get; set; }
        public int NgWidth { get; set; }
        public List<Ctrl> Controls { get; set; }
    }

    public class Ctrl
    {
        public Ctrl()
        {
            NgWidth = 6;
            DropDownOptions = new List<Select2Option>();
        }
        public string DisplayName { get; set; }
        public ControlTypeEnum ControlType { get; set; }
        public List<Select2Option> DropDownOptions { get; set; }
        public int NgWidth { get; set; }
        public string PlaceHolder { get; set; }
        public string ClientId { get; set; }



    }

    public static class CtrlHelpers
    {

        public static List<Select2Option> GetSelect2Options(IEnumerable<string> strOptions)
        {
            var returnOptions = strOptions.Select(x => new Select2Option() { id = x, text = x }).ToList();

            return returnOptions;
        }
    }

    public class Select2Option
    {
        public string id { get; set; }
        public string text { get; set; }
    }


    public enum ControlTypeEnum { TextBox, DropDownList, Select2, Select2Single, Select2Many, Select2TagsSingle, Select2TagsMany }

   // public enum NgWidthEnum {ng1, ng2, ng3, ng4, ng5, ng6, ng7, ng8, ng9, ng10, ng11, ng12 }
    public class PcrFormController : Controller
    {
        // GET: Mpa/PcrForm
        public ActionResult Index()
        {
            PcrForm pcrForm = new PcrForm();

            Tab tab = new Tab();

            Section section = new Section() { SectionName = "Test 1", NgWidth = 9 };

            Ctrl control = new Ctrl() { DisplayName = "FirstName", ControlType = ControlTypeEnum.Select2,
                DropDownOptions = CtrlHelpers.GetSelect2Options(new string[] { "one", "two", "three" })
            };



            section.Controls.Add(control);

            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);

            tab.Sections.Add(section);


            tab.Sections.Add(section);
            tab.Sections.Add(section);

            pcrForm.Tabs.Add(tab);

            tab = new Tab();

            section = new Section() { SectionName = "Raggedy Ann" }; ;

            control = new Ctrl() { DisplayName = "DropDown", ControlType = ControlTypeEnum.DropDownList,
            DropDownOptions = CtrlHelpers.GetSelect2Options(new string[] { "one", "two", "three" })
            };

            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);
            section.Controls.Add(control);

            tab.Sections.Add(section);
            tab.Sections.Add(section);
            tab.Sections.Add(section);

            pcrForm.Tabs.Add(tab);



            return View(pcrForm);
        }

        // GET: Mpa/PcrForm/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mpa/PcrForm/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mpa/PcrForm/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Mpa/PcrForm/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mpa/PcrForm/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Mpa/PcrForm/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mpa/PcrForm/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
