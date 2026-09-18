using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Data;
using TaskManagementSystem.Filters;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    

    [RoleAuthorize("Admin", "Manager", "User")]
    public class TaskController : Controller
    {
        // GET: Task
        AppDbContext db = new AppDbContext();

        // Task List
        //public ActionResult Index()
        //{
        //    var tasks = db.Tasks.ToList();

        //    return View(tasks);
        //}


        public ActionResult Indexssss()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string role = Convert.ToString(Session["Role"]);

            var tasks = db.Tasks.AsQueryable();

            // User ko sirf apne assigned tasks dikhenge
            if (role == "User")
            {
                tasks = tasks.Where(x => x.AssignedToUserId == userId);
            }

            // Manager ko uski team ke tasks dikhenge
            else if (role == "Manager")
            {
                var managerTeamIds = db.Teams
                    .Where(x => x.ManagerId == userId)
                    .Select(x => x.Id)
                    .ToList();

                tasks = tasks.Where(x =>
                    x.TeamId.HasValue &&
                    managerTeamIds.Contains(x.TeamId.Value));
            }

            // Admin ko sabhi tasks dikhenge

            return View(tasks.ToList());
        }



        //public ActionResult Index()
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    int userId = Convert.ToInt32(Session["UserId"]);
        //    string role = Convert.ToString(Session["Role"]);

        //    var tasks = db.Tasks.AsQueryable();

        //    if (role == "User")
        //    {
        //        tasks = tasks.Where(x => x.AssignedToUserId == userId);
        //    }
        //    else if (role == "Manager")
        //    {
        //        var managerTeamIds = db.Teams
        //            .Where(x => x.ManagerId == userId)
        //            .Select(x => x.Id)
        //            .ToList();

        //        tasks = tasks.Where(x =>
        //            x.TeamId.HasValue &&
        //            managerTeamIds.Contains(x.TeamId.Value));
        //    }

        //    ViewBag.Teams = db.Teams.ToList();
        //    ViewBag.Users = db.Users.ToList();

        //    return View(tasks.ToList());
        //}




        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string role = Convert.ToString(Session["Role"]);

            var tasks = db.Tasks.AsQueryable();

            if (role == "User")
            {
                
                tasks = tasks.Where(x => x.AssignedToUserId == userId);
            }
            else if (role == "Manager")
            {
                // Manager ko tasks dikhao
                tasks = tasks.Where(x => x.TeamId.HasValue);
            }

            ViewBag.Teams = db.Teams.ToList();
            ViewBag.Users = db.Users.ToList();

            return View(tasks.ToList());
        }



        // Create Task Page
        public ActionResult Create()
        {
            ViewBag.Teams = db.Teams.ToList();

            ViewBag.Users = db.Users
                .Where(x => x.Role == "User" && x.IsActive == true)
                .ToList();

            return View(new TaskItem());
        }

        // Save Task
        [HttpPost]
        public ActionResult Create(TaskItem model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teams = db.Teams.ToList();

                ViewBag.Users = db.Users
                    .Where(x => x.Role == "User" && x.IsActive == true)
                    .ToList();

                return View(model);
            }

            model.CreatedAt = DateTime.Now;

            db.Tasks.Add(model);
            db.SaveChanges();

            if (model.AssignedToUserId.HasValue)
            {
                Notification notification = new Notification();

                notification.UserId = model.AssignedToUserId.Value;
                notification.Message = "New task assigned to you: " + model.Title;
                notification.IsRead = false;
                notification.CreatedAt = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Task created successfully!";

            return RedirectToAction("Index");
        }

        // Edit Task
        public ActionResult Edit(int id)
        {
            var task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            ViewBag.Teams = db.Teams.ToList();

            ViewBag.Users = db.Users
                .Where(x => x.Role == "User" && x.IsActive == true)
                .ToList();

            return View("Create", task);
        }

        // Update Task

        [HttpPost]
        public ActionResult Edit(TaskItem model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teams = db.Teams.ToList();

                ViewBag.Users = db.Users
                    .Where(x => x.Role == "User" && x.IsActive == true)
                    .ToList();

                return View("Create", model);
            }

            var task = db.Tasks.Find(model.Id);

            if (task == null)
            {
                return HttpNotFound();
            }

            // Old status save
            string oldStatus = task.Status;

            task.Title = model.Title;
            task.Description = model.Description;
            task.AssignedToUserId = model.AssignedToUserId;
            task.TeamId = model.TeamId;
            task.Status = model.Status;
            task.Priority = model.Priority;
            task.Deadline = model.Deadline;

            db.SaveChanges();

            // Status change notification
            if (oldStatus != model.Status && model.AssignedToUserId.HasValue)
            {
                Notification notification = new Notification();

                notification.UserId = model.AssignedToUserId.Value;
                notification.Message = "Task status updated: " + model.Title +
                                       " is now " + model.Status;

                notification.IsRead = false;
                notification.CreatedAt = DateTime.Now;

                db.Notifications.Add(notification);
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Task updated successfully!";

            return RedirectToAction("Index");
        }





        //[HttpPost]
        //public ActionResult Edit(TaskItem model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.Teams = db.Teams.ToList();

        //        ViewBag.Users = db.Users
        //            .Where(x => x.Role == "User" && x.IsActive == true)
        //            .ToList();

        //        return View("Create", model);
        //    }

        //    var task = db.Tasks.Find(model.Id);

        //    if (task == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    task.Title = model.Title;
        //    task.Description = model.Description;
        //    task.AssignedToUserId = model.AssignedToUserId;
        //    task.TeamId = model.TeamId;
        //    task.Status = model.Status;
        //    task.Priority = model.Priority;
        //    task.Deadline = model.Deadline;

        //    db.SaveChanges();

        //    TempData["SuccessMessage"] = "Task updated successfully!";

        //    return RedirectToAction("Index");
        //}

        // Delete Task
        public ActionResult Deletessssss(int id)
        {
            var task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Task deleted successfully!";

            return RedirectToAction("Index");
        }





        public ActionResult Delete(int id)
        {
            var task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string role = Convert.ToString(Session["Role"]);

            // User kisi task ko delete nahi karega
            if (role == "User")
            {
                return new HttpStatusCodeResult(403, "Access Denied");
            }

            // Manager sirf apni team ka task delete karega
            if (role == "Manager")
            {
                var isMyTeamTask = db.Teams.Any(x =>
                    x.Id == task.TeamId &&
                    x.ManagerId == userId);

                if (!isMyTeamTask)
                {
                    return new HttpStatusCodeResult(403, "Access Denied");
                }
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Task deleted successfully!";

            return RedirectToAction("Index");
        }


    }
}