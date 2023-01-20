using HRManagementSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRManagementSystem.Controllers
{
    public class DesignationController : Controller
    {
        // GET: Designation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadDesignation()
        {
            DataTable dt = null;
            List<Designation> bts = new List<Designation>();
            try
            {
                DesignationBusinessService designation = new DesignationBusinessService();
                dt = designation.DesignationSelect();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Designation b = new Designation();
                    b.Name = dt.Rows[i]["Name"].ToString();
                    b.id = dt.Rows[i]["id"].ToString();
                    bts.Add(b);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ViewData["designation"] = bts;
            return View("Designation");
        }

        [HttpPost]
        public ActionResult AddDesignation(string Name)
        {
            Designation designation = new Designation();
            designation.Name = Name;
            try
            {
                DesignationBusinessService designationServices = new DesignationBusinessService();
                designationServices.AddDesignation(designation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public JsonResult EditDesignation(string designationId, string designationName)
        {
            bool result = false;
            Designation designation = new Designation();
            designation.id = designationId;
            designation.Name = designationName;
            try
            {
                DesignationBusinessService designationServices = new DesignationBusinessService();
                result = designationServices.EditDesignationName(designation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        public JsonResult RemoveDesignation(string DesignationId)
        {
            DataTable dt = null;
            Designation designation = new Designation();
            designation.id = DesignationId;
            try
            {
                DesignationBusinessService designationServices = new DesignationBusinessService();
                dt = designationServices.RemoveDesignation(designation);
            }
            catch (Exception ex) { }

            return Json(JsonConvert.SerializeObject(dt));
        }
    }
}