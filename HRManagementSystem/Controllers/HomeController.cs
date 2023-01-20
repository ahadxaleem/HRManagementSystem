using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRManagementSystem.Models;

namespace HRManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TeamView()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string FirstName, string LastName, string Email, string Password, string ConfirmPassword)
        {
            User user = new User();
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;
            user.Password = Password;
            try
            {
                UserBusinessService uBusinessSerice = new UserBusinessService();
                uBusinessSerice.User_CRUD(user);
            }
            catch(Exception ex)
            {
            }

            return View("Login");
        }

        [HttpPost]
        public JsonResult SignIn(string Email, string Password)
        {
            User user = new User();
            try
            {
                UserBusinessService uBusinessSerice = new UserBusinessService();
                user = uBusinessSerice.User_Select(Email, Password);

                if (user != null)
                {
                    //HttpContext.Current.Session[linkedSalesman] = value;
                    GeneralModel.CurrentUser = user;

                }
            }
            catch (Exception ex)
            {
            }

            return Json(user);
            //return View("Login");
        }

        public ActionResult RedirectToLogout()
        {
            GeneralModel.CurrentUser = null;
            return RedirectToAction("Login", "Home");
        }



    }
}

     