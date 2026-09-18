using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Data;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        //  Account
        public ActionResult Index()
        {
            return View();
        }

        AppDbContext db = new AppDbContext();

        //  Account/Login
        public ActionResult Login()
        {
            return View();
        }

      



        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password))
            {
                ViewBag.Message = "Please enter Email and Password";
                return View();
            }

            // Database se Email + Password check
            var user = db.Users.FirstOrDefault(x =>x.Email == Email &&x.PasswordHash == Password &&x.IsActive == true);

            if (user == null)
            {
                ViewBag.Message = "Invalid Email or Password";
                return View();
            }

           
            Session["UserId"] = user.Id;
            Session["UserName"] = user.Name;
            Session["Email"] = user.Email;
            Session["Role"] = user.Role.ToString();

            ViewBag.Message = "Login Successful";

            return RedirectToAction("Index", "Dashboard");
        }




        // Logout
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Login", "Account");
        }



    }
}