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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        AppDbContext db = new AppDbContext();


        public ActionResult Index(string status, string priority, string deadline)
        {
            var tasks = db.Tasks.AsQueryable();

            // Status Filter
            if (!string.IsNullOrEmpty(status))
            {
                tasks = tasks.Where(x => x.Status == status);
            }

            // Priority Filter
            if (!string.IsNullOrEmpty(priority))
            {
                tasks = tasks.Where(x => x.Priority == priority);
            }

            // Deadline Filter
            if (deadline == "Today")
            {
                var today = DateTime.Today;

                tasks = tasks.Where(x =>
                    x.Deadline.HasValue &&
                    x.Deadline.Value >= today &&
                    x.Deadline.Value < today.AddDays(1));
            }
            else if (deadline == "Upcoming")
            {
                tasks = tasks.Where(x =>
                    x.Deadline.HasValue &&
                    x.Deadline.Value >= DateTime.Today);
            }
            else if (deadline == "Overdue")
            {
                tasks = tasks.Where(x =>
                    x.Deadline.HasValue &&
                    x.Deadline.Value < DateTime.Today &&
                    x.Status != "Done");
            }

            var totalTasks = tasks.Count();

            var todoTasks = tasks.Count(x => x.Status == "To Do");

            var inProgressTasks = tasks.Count(x => x.Status == "In Progress");

            var doneTasks = tasks.Count(x => x.Status == "Done");

            ViewBag.TotalTasks = totalTasks;
            ViewBag.ToDoTasks = todoTasks;
            ViewBag.InProgressTasks = inProgressTasks;
            ViewBag.DoneTasks = doneTasks;

            return View();
        }






        //public ActionResult Index()
        //{

        //    var totalTasks = db.Tasks.Count();

        //    var todoTasks = db.Tasks.Count(x => x.Status == "To Do");

        //    var inProgressTasks = db.Tasks.Count(x => x.Status == "In Progress");

        //    var doneTasks = db.Tasks.Count(x => x.Status == "Done");

        //    ViewBag.TotalTasks = totalTasks;
        //    ViewBag.ToDoTasks = todoTasks;
        //    ViewBag.InProgressTasks = inProgressTasks;
        //    ViewBag.DoneTasks = doneTasks;


        //    return View();
        //}
    }
}