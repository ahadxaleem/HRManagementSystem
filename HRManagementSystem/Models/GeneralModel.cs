using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRManagementSystem.Models
{
    public class GeneralModel
    {

        public static User CurrentUser
        {
            get
            {
                return HttpContext.Current.Session["currentUser"] as User;
            }
            set
            {
                HttpContext.Current.Session["currentUser"] = value;
            }
        }

        public static string[] TemporaryStrings
        {
            set { HttpContext.Current.Session["temporaryStrings"] = value; }
            get
            {
                if (HttpContext.Current.Session["temporaryStrings"] != null)
                {
                    return (string[])HttpContext.Current.Session["temporaryStrings"];
                }

                return null;
            }
        }
        public static byte[] TemporaryImageContent
        {
            set { HttpContext.Current.Session["temporaryImageContent"] = value; }
            get
            {
                if (HttpContext.Current.Session["temporaryImageContent"] != null)
                {
                    return (byte[])HttpContext.Current.Session["temporaryImageContent"];
                }

                return null;
            }
        }
    }
}