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
            return View();
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
