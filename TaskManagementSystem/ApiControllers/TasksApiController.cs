using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagementSystem.Data;
using TaskManagementSystem.Filters;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.ApiControllers
{

    [RoutePrefix("api/TasksApi")]
    [JwtAuthorize]
    [JwtRoleAuthorize("Admin", "Manager", "User")]
    public class TasksApiController : ApiController
    {


        AppDbContext db = new AppDbContext();

        // GET: api/TasksApi
      
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var data = db.Tasks.ToList();

            return Ok(data);
        }

        // GET: api/TasksApi/1
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(int id)
        {
            var data = db.Tasks.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        // POST: api/TasksApi
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(TaskItem model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    model.CreatedAt = System.DateTime.Now;

        //    db.Tasks.Add(model);
        //    db.SaveChanges();

        //    //return Ok(model);

        //    return Ok(new
        //    {
        //        Message = "Task saved successfully",

        //    });
        //}



        // POST: api/TasksApi
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(TaskItem model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CreatedAt = DateTime.Now;

            db.Tasks.Add(model);
            db.SaveChanges();

            // Create notification if task is assigned to a user
            if (model.AssignedToUserId.HasValue)
            {
                db.Notifications.Add(new Notification
                {
                    UserId = model.AssignedToUserId.Value,
                    Message = "New task assigned: " + model.Title,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                });

                db.SaveChanges();
            }

            return Ok(new
            {
                Message = "Task saved successfully"
            });
        }


        // PUT: api/TasksApi
        //[HttpPut]
        //public IHttpActionResult Update(TaskItem model)
        //{
        //    var data = db.Tasks.Find(model.Id);

        //    if (data == null)
        //    {
        //        return NotFound();
        //    }

        //    //data.Title = model.Title;
        //    //data.Description = model.Description;
        //    //data.AssignedToUserId = model.AssignedToUserId;
        //    //data.TeamId = model.TeamId;
        //    //data.Status = model.Status;
        //    //data.Priority = model.Priority;
        //    //data.Deadline = model.Deadline;

        //    db.SaveChanges();

        //    //return Ok(data);

        //    return Ok(new
        //    {
        //        Message = "Task update successfully"
        //    });
        //}



        // PUT: api/TasksApi
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(TaskItem model)
        {
            var data = db.Tasks.Find(model.Id);

            if (data == null)
            {
                return NotFound();
            }

            // Old values
            int? oldAssignedTo = data.AssignedToUserId;
            string oldStatus = data.Status;

            // Update task
            data.Title = model.Title;
            data.Description = model.Description;
            data.AssignedToUserId = model.AssignedToUserId;
            data.TeamId = model.TeamId;
            data.Status = model.Status;
            data.Priority = model.Priority;
            data.Deadline = model.Deadline;

            db.SaveChanges();

            // Notification
            if (data.AssignedToUserId.HasValue)
            {
                if (oldAssignedTo != data.AssignedToUserId)
                {
                    db.Notifications.Add(new Notification
                    {
                        UserId = data.AssignedToUserId.Value,
                        Message = "New task assigned: " + data.Title,
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    });

                    db.SaveChanges();
                }
                else if (oldStatus != data.Status)
                {
                    db.Notifications.Add(new Notification
                    {
                        UserId = data.AssignedToUserId.Value,
                        Message = "Task status updated: " + data.Title + " - " + data.Status,
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    });

                    db.SaveChanges();
                }
            }

            return Ok(new
            {
                Message = "Task updated successfully"
            });
        }




        // DELETE: api/TasksApi/1
        [HttpDelete]
        [Route("Delete/ {id}")]
        public IHttpActionResult Delete(int id)
        {
            var data = db.Tasks.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            db.Tasks.Remove(data);
            db.SaveChanges();

            return Ok("Task deleted successfully");
        }



    }
}
