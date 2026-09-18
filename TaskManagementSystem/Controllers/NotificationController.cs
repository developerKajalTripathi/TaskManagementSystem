using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Data;
using TaskManagementSystem.Filters;

namespace TaskManagementSystem.Controllers
{
    [RoleAuthorize("Admin", "Manager", "User")]
    public class NotificationController : Controller
    {
        // GET: Notification
        


        AppDbContext db = new AppDbContext();

        // Notification List
        public ActionResult Notification()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            var notifications = db.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View(notifications);
        }

        // Mark Notification as Read
        public ActionResult MarkAsRead(int id)
        {
            var notification = db.Notifications.Find(id);

            if (notification == null)
            {
                return HttpNotFound();
            }

            notification.IsRead = true;

            db.SaveChanges();

            return RedirectToAction("Notification");
        }

        // Delete Notification
        public ActionResult Delete(int id)
        {
            var notification = db.Notifications.Find(id);

            if (notification == null)
            {
                return HttpNotFound();
            }

            db.Notifications.Remove(notification);

            db.SaveChanges();

            TempData["SuccessMessage"] = "Notification deleted successfully!";

            return RedirectToAction("Index");
        }



    }
}