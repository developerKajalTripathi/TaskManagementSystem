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
    public class TeamController : Controller
    {
        // GET: Team
        AppDbContext db = new AppDbContext();

        // Team List
        public ActionResult Index()
        {
            var teams = db.Teams.ToList();

            return View(teams);
        }

        // Add Team Page
        public ActionResult CreateTeam()
        {
            Team model = new Team();
            // Manager dropdown ke liye
            ViewBag.Managers = db.Users
                .Where(x => x.Role == "Manager" && x.IsActive == true)
                .ToList();

            return View(model);
        }

        // Save Team
        [HttpPost]
        public ActionResult CreateTeam(Team model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Managers = db.Users
                    .Where(x => x.Role == "Manager" && x.IsActive == true)
                    .ToList();

                return View(model);
            }

            // Duplicate Team Name check
            var existingTeam = db.Teams
                .FirstOrDefault(x => x.Name == model.Name);

            if (existingTeam != null)
            {
                ViewBag.Message = "Team name already exists";

                ViewBag.Managers = db.Users
                    .Where(x => x.Role == "Manager" && x.IsActive == true)
                    .ToList();

                return View(model);
            }

            model.CreatedAt = DateTime.Now;

            db.Teams.Add(model);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Team added successfully!";

            return RedirectToAction("Index");
        }

        // Edit Team
        public ActionResult Edit(int id)
        {
            var team = db.Teams.Find(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            ViewBag.Managers = db.Users
                .Where(x => x.Role == "Manager" && x.IsActive == true)
                .ToList();

            return View("Create", team);
        }

        // Update Team
        [HttpPost]
        public ActionResult Edit(Team model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Managers = db.Users
                    .Where(x => x.Role == "Manager" && x.IsActive == true)
                    .ToList();

                return View("Create", model);
            }

            var team = db.Teams.Find(model.Id);

            if (team == null)
            {
                return HttpNotFound();
            }

            team.Name = model.Name;
            team.ManagerId = model.ManagerId;

            db.SaveChanges();

            TempData["SuccessMessage"] = "Team updated successfully!";

            return RedirectToAction("Index");
        }

        // Delete Team
        public ActionResult Delete(int id)
        {
            var team = db.Teams.Find(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            db.Teams.Remove(team);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Team deleted successfully!";

            return RedirectToAction("Index");
        }
    }
}