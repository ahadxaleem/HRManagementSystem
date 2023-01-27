using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRManagementSystem.Models;
using System.Data;
using Newtonsoft.Json;

namespace HRManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Home()
        {
            return View("Home");
        }

        public ActionResult Employee()
        {
            LoadDepartments();
            LoadBranches();
            LoadDesignation();
            return View("Employee");
        }
        public void LoadDepartments()
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ViewData["departments"] = dts;
        }
        public void LoadBranches()
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
        }
        public void LoadDesignation()
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
        }
        [HttpPost]
        public string CollectData(string data)
        {
            GeneralModel.TemporaryStrings = new string[1];
            GeneralModel.TemporaryStrings[0] = data;

            return "Success";
        }

        [HttpPost]
        public string CollectImage(FormCollection data)
        {
            if (Request.Files["files"] != null)
            {
                using (var binaryReader = new BinaryReader(Request.Files["files"].InputStream))
                {
                    byte[] Imagefile = binaryReader.ReadBytes(Request.Files["files"].ContentLength);//your image

                    GeneralModel.TemporaryImageContent = new byte[Request.Files["files"].ContentLength];
                    GeneralModel.TemporaryImageContent = Imagefile;
                }
            }

            return "Success";
        }

        [HttpPost]
        public string SaveEmployee(string isInserting)
        {
            EmployeeBusinessService eBusinessSerice = new EmployeeBusinessService();

            Employee emp = eBusinessSerice.ParseFromJSON(GeneralModel.TemporaryStrings[0]);
            emp.EmployeeMediaContents = GeneralModel.TemporaryImageContent;
            emp.EmployeeMediaContentType = "image/jpeg";

            eBusinessSerice.Employee_CRUD(emp, "INSERT");

            return "success";
        }
        public JsonResult LoadEmployee(string EmployeeCode)
        {
            Employee e = null;
            try
            {
                EmployeeBusinessService empBusinessSerice = new EmployeeBusinessService();
                e = empBusinessSerice.Employee_Select(EmployeeCode);
                e.DisplayImage = Convert.ToBase64String(e.EmployeeMediaContents);
            }
            catch (Exception ex)
            {
            }

            return Json(e);
            //return View("Login");
        }

        [HttpPost]
        public bool RemoveEmployee(string EmployeeCode)
        {
            bool dt = false;
            Employee employee = new Employee();
            employee.EmployeeCode = EmployeeCode;
            try
            {
                EmployeeBusinessService employeeServices = new EmployeeBusinessService();
                dt = employeeServices.Employee_CRUD(employee,"DELETE");
            }
            catch (Exception ex) { }

            return dt;
        }

        public ActionResult Departments()
        {
            return RedirectToAction("LoadDepartments", "Departments");
        }

        public ActionResult Branches()
        {
            return RedirectToAction("LoadBranches", "Branches");
        }

        public ActionResult EmployeeListing(int page=1, int rowlength=1)
        {
            DataTable dt = null;
            int totalEmp=0;
            List<Employee> bts = new List<Employee>();
            try
            {
                EmployeeBusinessService empBusinessSerice = new EmployeeBusinessService();
                dt = empBusinessSerice.Select_page(page,rowlength);
                totalEmp = empBusinessSerice.emp_count();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Employee e = new Employee();
                    e.EmployeeCode = dt.Rows[i]["EmployId"].ToString();
                    e.EmployeeName = dt.Rows[i]["EmployName"].ToString();
                    e.JoinDate = dt.Rows[i]["JoinDate"].ToString();
                    e.Email = dt.Rows[i]["Email"].ToString();
                    e.Department = dt.Rows[i]["Department"].ToString();
                    e.Branch = dt.Rows[i]["EmployBranch"].ToString();
                    e.Designation = dt.Rows[i]["EmployDesignation"].ToString();
                    bts.Add(e);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ViewData["employees"] = bts;
            ViewData["totalEmp"] = totalEmp;

            return View();
        }
        public ActionResult Designation()
        {
            return RedirectToAction("LoadDesignation", "Designation");
        }
    }
}