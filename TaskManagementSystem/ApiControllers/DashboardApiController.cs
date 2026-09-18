using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagementSystem.Data;
using TaskManagementSystem.Filters;

namespace TaskManagementSystem.ApiControllers
{

    [RoutePrefix("api/DashboardApi")]
    [JwtAuthorize]
    [JwtRoleAuthorize("Admin", "Manager", "User")]
    public class DashboardApiController : ApiController
    {

        AppDbContext db = new AppDbContext();

        // GET: api/DashboardApi/GetDashboard
        [HttpGet]
        [Route("GetDashboard")]
        public IHttpActionResult GetDashboard()
        {
            var totalTasks = db.Tasks.Count();

            var todoTasks = db.Tasks
                .Count(x => x.Status == "ToDo");

            var inProgressTasks = db.Tasks
                .Count(x => x.Status == "InProgress");

            var completedTasks = db.Tasks
                .Count(x => x.Status == "Completed");

            return Ok(new
            {
                TotalTasks = totalTasks,
                ToDoTasks = todoTasks,
                InProgressTasks = inProgressTasks,
                CompletedTasks = completedTasks
            });
        }

        // GET: api/DashboardApi/Filter?status=ToDo&priority=High
        //[HttpGet]
        //[Route("Filter")]
        //public IHttpActionResult Filter(string status = null,string priority = null)
        //{
        //    var query = db.Tasks.AsQueryable();

        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        query = query.Where(x => x.Status == status);
        //    }

        //    if (!string.IsNullOrEmpty(priority))
        //    {
        //        query = query.Where(x => x.Priority == priority);
        //    }

        //    var data = query.ToList();

        //    return Ok(data);
        //}


        [HttpGet]
        [Route("Filter")]
        public IHttpActionResult Filter(string status = null, string priority = null)
        {
            var query = db.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrEmpty(priority))
            {
                query = query.Where(x => x.Priority == priority);
            }

            var data = query.ToList();

            if (data.Count == 0)
            {
                return Ok(new
                {
                    Message = "No record found"
                });
            }

            return Ok(data);
        }





    }
}
