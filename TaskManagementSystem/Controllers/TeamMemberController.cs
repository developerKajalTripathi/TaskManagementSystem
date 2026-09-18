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


    [RoleAuthorize("Admin", "Manager")]
    public class TeamMemberController : Controller
    {
        //// GET: TeamMember
        //public ActionResult TeamMember()
        //{
        //    return View();
        //}
        AppDbContext db = new AppDbContext();

        // Team Members List
        public ActionResult TeamMember()
        {
            //var members = db.TeamMembers.ToList();

            //return View(members);

            var members = db.TeamMembers.ToList();

            ViewBag.Teams = db.Teams.ToList();
            ViewBag.Users = db.Users.ToList();
            return View(members);

        }

        // Assign Member Page
        [HttpGet]
        public ActionResult CreateTeamMember()
        {
            ViewBag.Teams = db.Teams.ToList();

            ViewBag.Users = db.Users
                .Where(x => x.Role == "User" && x.IsActive == true)
                .ToList();

            return View(new TeamMember());
        }

        // Save Member
        [HttpPost]
        public ActionResult CreateTeamMember(TeamMember model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teams = db.Teams.ToList();

                ViewBag.Users = db.Users
                    .Where(x => x.Role == "User" && x.IsActive == true)
                    .ToList();

                return View(model);
            }

            // Check duplicate assignment
            var existingMember = db.TeamMembers
                .FirstOrDefault(x =>
                    x.TeamId == model.TeamId &&
                    x.UserId == model.UserId);

            if (existingMember != null)
            {
                ViewBag.Message = "User is already assigned to this team";

                ViewBag.Teams = db.Teams.ToList();

                ViewBag.Users = db.Users
                    .Where(x => x.Role == "User" && x.IsActive == true)
                    .ToList();

                return View(model);
            }

            db.TeamMembers.Add(model);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Member assigned successfully!";

            return RedirectToAction("TeamMember");
        }

        // Remove Member
        public ActionResult Delete(int id)
        {
            var member = db.TeamMembers.Find(id);

            if (member == null)
            {
                return HttpNotFound();
            }

            db.TeamMembers.Remove(member);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Member removed successfully!";

            return RedirectToAction("Index");
        }




    }
}