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
    public class CommentController : Controller
    {
        // GET: Comment
        //public ActionResult Index()
        //{
        //    return View();
        //}

        AppDbContext db = new AppDbContext();

        // Comment List
        public ActionResult Index()
        {
            var comments = db.Comments
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            ViewBag.Tasks = db.Tasks.ToList();
            ViewBag.Users = db.Users.ToList();

            return View(comments);
        }


        // Add Comment Page



       
        public ActionResult Create(int taskId)
        {
            var task = db.Tasks.Find(taskId);

            if (task == null)
            {
                return HttpNotFound();
            }

            Comment model = new Comment();

            model.TaskId = taskId;

            ViewBag.TaskTitle = task.Title;

            return View(model);
        }


        // Save Comment
        [HttpPost]
        public ActionResult Create(Comment model)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(model.CommentText))
            {
                ViewBag.Message = "Please enter comment";

                var task = db.Tasks.Find(model.TaskId);

                if (task != null)
                {
                    ViewBag.TaskTitle = task.Title;
                }

                return View(model);
            }

            model.UserId = Convert.ToInt32(Session["UserId"]);
            model.CreatedAt = DateTime.Now;

            db.Comments.Add(model);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Comment added successfully!";

            return RedirectToAction("Create", new { taskId = model.TaskId });
        }


        // Delete Comment
        public ActionResult Delete(int id)
        {
            var comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Comment deleted successfully!";

            return RedirectToAction("Index");
        }



    }
}