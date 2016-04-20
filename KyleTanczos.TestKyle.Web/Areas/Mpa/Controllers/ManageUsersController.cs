using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class ManageUsers
    {
        public ManageUsers()
        {
            Tab = new Tab();
        }

        public Tab Tab { get; set; }
    }
    public class ManageUsersController : Controller
    {
        // GET: Mpa/ManageUsers
        public ActionResult Index()
        {
            string state = "PA"; // set from user info
            string agencyToken = "Superior"; // set from user info

            GetPcrFormSelect2Options options = new GetPcrFormSelect2Options(state, agencyToken);
            ManageUsers model = new ManageUsers();
            model.Tab = new Tab()
            {
                PartialTemplateName = "TabSingleColumn",
                TabTargetName = "ManageUsersTab",
                Sections = new List<Section>()
                         {
                            new Section()
                            {
                                ResponsiveWidth = 12,
                                PartialTemplateName = "SectionWithDialog",
                                Dialog = new Dialog()
                                {
                                        DialogTargetId = "ManageUsers",
                                        DialogTitle = "ManageUsers",
                                        NgFormName = "ManageUsers",
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.ManageUsers.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new TextBox() { DisplayName = "UserName", NgModel = "forms.ManageUsers.userName"
                                            , ResponsiveWidth = 12
                                            },
                                             new TextBox() { DisplayName = "First Name", NgModel = "forms.ManageUsers.firstName"
                                            , ResponsiveWidth = 12
                                            },
                                              new TextBox() { DisplayName = "Last Name", NgModel = "forms.ManageUsers.lastName"
                                            , ResponsiveWidth = 12
                                            },
                                            new TextBox() { DisplayName = "State ID", NgModel = "forms.ManageUsers.stateId",
                                                 ResponsiveWidth = 12
                                                },
                                            new TextBox() { DisplayName = "Agency Certification Status", NgModel = "forms.ManageUsers.agencyCertificationStatus",
                                                 ResponsiveWidth = 12
                                                },
                                            new CheckBox() { DisplayName = "Active Crew Member?", NgModel = "forms.ManageUsers.isEmt", ResponsiveWidth = 12
                                               },
                                            new TextBox() { DisplayName = "Email Address", NgModel = "forms.ManageUsers.emailAddress",
                                                ResponsiveWidth = 12
                                            },
                                             new TextBox() { PlaceHolder = "Password", NgModel = "forms.ManageUsers.password",
                                                 ResponsiveWidth = 12//, ControlType = ControlTypeEnum.Password
                                                },
                                            new TextBox() { PlaceHolder = "Password (repeat)", NgModel = "forms.ManageUsers.confimPassword",
                                                 ResponsiveWidth = 12//, ControlType = ControlTypeEnum.Password
                                                },
                                            new CheckBox() { DisplayName = "Set Random Password", NgModel = "forms.ManageUsers.setRandomPassword",
                                                ResponsiveWidth = 12
                                            },
                                            new CheckBox() { DisplayName = "Should Change Password Next Login", NgModel = "forms.ManageUsers.shouldChangePassword",
                                                ResponsiveWidth = 12
                                            },
                                            new CheckBox() { DisplayName = "Send Activation Email.", NgModel = "forms.ManageUsers.sendActivationEmail",
                                                ResponsiveWidth = 12
                                            },
                                            new CheckBox() { DisplayName = "Active", NgModel = "forms.ManageUsers.active",
                                                ResponsiveWidth = 12
                                            },
                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                        NgSubmitClick = "AddItemToDatabase('ManageUsers');",
                                        NgCancelClick = "ClearCloseModal('#ManageUsers, 'ManageUsers');"
                                },
                                SectionName = "ManageUsers",
                                Controls = new List<Ctrl>()
                                {
                                    new TableListView() {
                                        ngListName = "ManageUsers",
                                        ngFieldNames =  new List<string>() { "userName", "stateId", "agencyCertificationStatus", "isEmt" },
                                        DisplayNames = new List<string>() { "UserName", "State ID", "Agency Certification Status", "Active Crew"},
                                        ResponsiveWidth = 12
                                    }
                                }
                            }
                    }

            };
            return View(model);
        }

        // GET: Mpa/ManageUsers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mpa/ManageUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mpa/ManageUsers/Create
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

        // GET: Mpa/ManageUsers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mpa/ManageUsers/Edit/5
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

        // GET: Mpa/ManageUsers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mpa/ManageUsers/Delete/5
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
