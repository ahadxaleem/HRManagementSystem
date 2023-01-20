using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.Controllers
{
    public class DepartmentsController : Controller
    {
        // GET: Departments
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult LoadDepartments()
        {
            DataTable dt = null;
            List<Departments> dts = new List<Departments>();
            try
            {
                DepartmentBusinessService Department = new DepartmentBusinessService();
                dt = Department.DapartmentsSelect();
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Departments d = new Departments();
                    d.Name = dt.Rows[i]["Name"].ToString();
                    d.id = dt.Rows[i]["id"].ToString();
                    dts.Add(d);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            ViewData["departments"] = dts;
            return View("Departements");
        }
        [HttpPost]
        public ActionResult AddDepartment(string Name)
        {
            Departments department = new Departments();
            department.Name = Name;
            try
            {
                DepartmentBusinessService DepartmentSerice = new DepartmentBusinessService();
                DepartmentSerice.AddDepartment(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        [HttpPost]
        public JsonResult RemoveDepartment(string departmentId)
        {
            DataTable dt = null;
            Departments department = new Departments();
            department.id = departmentId;
            try
            {
                DepartmentBusinessService DepartmentSerice = new DepartmentBusinessService();
                dt = DepartmentSerice.RemoveDepartment(department);
            }
            catch (Exception ex) { }

            return Json(JsonConvert.SerializeObject(dt));

        }
        [HttpPost]
        public JsonResult EditDepartment(string departmentId, string departmentName)
        {
            DataTable dt = null;
            Departments department = new Departments();
            department.id = departmentId;
            department.Name = departmentName;
            try
            {
                DepartmentBusinessService businessService = new DepartmentBusinessService();
                businessService.EditDepartmentName(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(JsonConvert.SerializeObject(dt));
        }


    }
}