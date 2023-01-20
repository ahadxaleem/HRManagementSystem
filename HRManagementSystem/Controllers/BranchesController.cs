using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Data;
using System.Web;
using System.Web.Mvc;
using HRManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.Controllers
{
    public class BranchesController : Controller
    {
        // GET: Branches
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadBranches()
        {
            DataTable dt = null;
            List<Branches> bts = new List<Branches>();
            try
            {
                BranchesBusinessService branches = new BranchesBusinessService();
                dt = branches.BranchesSelect();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Branches b = new Branches();
                    b.Name = dt.Rows[i]["Name"].ToString();
                    b.id = dt.Rows[i]["id"].ToString();
                    bts.Add(b);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ViewData["branches"] = bts;
            return View("Branches");
        }
        [HttpPost]
        public ActionResult AddBranch(string Name)
        {
            Branches branch = new Branches();
            branch.Name = Name;
            try
            {
                BranchesBusinessService branchServices = new BranchesBusinessService();
                branchServices.AddBranch(branch);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        [HttpPost]
        public JsonResult EditBranch(string branchId, string branchName)
        {
            bool result = false;
            Branches branch = new Branches();
            branch.id = branchId;
            branch.Name = branchName;
            try
            {
                BranchesBusinessService branchServices = new BranchesBusinessService();
                result = branchServices.EditBranchName(branch);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(JsonConvert.SerializeObject(result));
        }
        [HttpPost]
        public JsonResult RemoveBranch(string branchId)
        {
            DataTable dt = null;
            Branches branch = new Branches();
            branch.id = branchId;
            try
            {
                BranchesBusinessService branchServices = new BranchesBusinessService();
                dt = branchServices.RemoveBranch(branch);
            }
            catch (Exception ex) { }

            return Json(JsonConvert.SerializeObject(dt));
        }
    }
}