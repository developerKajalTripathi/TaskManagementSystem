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

    [RoutePrefix("api/NotificationsApi")]
    [JwtAuthorize]
    [JwtRoleAuthorize("Admin", "Manager", "User")]
    public class NotificationsApiController : ApiController
    {
        AppDbContext db = new AppDbContext();

        // GET: api/NotificationsApi
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var data = db.Notifications.ToList();

            return Ok(data);
        }

        // GET: api/NotificationsApi/1
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(int id)
        {
            var data = db.Notifications.Find(id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        // POST: api/NotificationsApi
        [HttpPost]
        public IHttpActionResult Insert(Notification model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.IsRead = false;
            model.CreatedAt = System.DateTime.Now;

            db.Notifications.Add(model);
            db.SaveChanges();

            return Ok(model);
        }

        // PUT: api/NotificationsApi
        [HttpPut]
        public IHttpActionResult Update(Notification model)
        {
            var data = db.Notifications.Find(model.Id);

            if (data == null)
                return NotFound();

            data.UserId = model.UserId;
            data.Message = model.Message;
            data.IsRead = model.IsRead;

            db.SaveChanges();

            return Ok(data);
        }

        // DELETE: api/NotificationsApi/1
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var data = db.Notifications.Find(id);

            if (data == null)
                return NotFound();

            db.Notifications.Remove(data);
            db.SaveChanges();

            return Ok("Notification deleted successfully");
        }

    }
}
