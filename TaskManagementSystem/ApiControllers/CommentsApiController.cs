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

    [RoutePrefix("api/CommentsApi")]
    [JwtAuthorize]
    [JwtRoleAuthorize("Admin", "Manager", "User")]
    public class CommentsApiController : ApiController
    {

        AppDbContext db = new AppDbContext();

        // GET: api/CommentsApi
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var data = db.Comments.ToList();

            return Ok(data);
        }

        // GET: api/CommentsApi/1
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(int id)
        {
            var data = db.Comments.Find(id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        // POST: api/CommentsApi
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(Comment model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CreatedAt = DateTime.Now;

            db.Comments.Add(model);
            db.SaveChanges();

            return Ok(model);
        }

        // PUT: api/CommentsApi
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(Comment model)
        {
            var data = db.Comments.Find(model.Id);

            if (data == null)
                return NotFound();

            data.TaskId = model.TaskId;
            data.UserId = model.UserId;
            data.CommentText = model.CommentText;

            db.SaveChanges();

            return Ok(data);
        }

        // DELETE: api/CommentsApi/1
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            var data = db.Comments.Find(id);

            if (data == null)
                return NotFound();

            db.Comments.Remove(data);
            db.SaveChanges();

            return Ok("Comment deleted successfully");
        }


    }
}
