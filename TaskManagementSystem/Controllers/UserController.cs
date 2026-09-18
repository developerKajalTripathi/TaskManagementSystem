using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;
using TaskManagementSystem.Filters;

namespace TaskManagementSystem.Controllers
{



    [RoleAuthorize("Admin", "Manager", "User")]
    public class UserController : Controller
    {
        // GET: User

        AppDbContext db = new AppDbContext();
        public ActionResult UserManagement()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        //add User
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check duplicate email
            var existingUser = db.Users
                .FirstOrDefault(x => x.Email == model.Email);

            if (existingUser != null)
            {
                ViewBag.Message = "Email already exists";
                return View(model);
            }

            PasswordService service = new PasswordService();

            model.PasswordHash = service.HashPassword(model.PasswordHash);
            model.IsActive = true;
            model.CreatedAt = DateTime.Now;

            db.Users.Add(model);
            db.SaveChanges();
            TempData["SuccessMessage"] = "User added successfully!";

            return RedirectToAction("UserManagement");
        }

        // Edit User
        //public ActionResult Edit(int id)
        //{
        //    var user = db.Users.Find(id);

        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(user);
        //}


        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View("Create", user);
        }


        [HttpPost]
        public ActionResult Edit(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Users.Find(model.Id);

            if (user == null)
            {
                return HttpNotFound();
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.Role = model.Role;
            user.IsActive = model.IsActive;

            db.SaveChanges();

            return RedirectToAction("UserManagement");
        }

        // Delete User
        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("UserManagement");
        }

    }
}