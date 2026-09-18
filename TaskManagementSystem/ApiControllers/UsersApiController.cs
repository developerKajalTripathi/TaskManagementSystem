using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagementSystem.Data;
using TaskManagementSystem.Filters;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.ApiControllers
{
    [RoutePrefix("api/UsersApi")]
    [JwtAuthorize]
    [JwtRoleAuthorize("Admin", "Manager", "User")]
    public class UsersApiController : ApiController
    {


        AppDbContext db = new AppDbContext();

        // GET: api/UsersApi
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var data = db.Users.ToList();

            return Ok(data);
        }

        // GET: api/UsersApi/3
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(int id)
        {
            var data = db.Users.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        // POST: api/UsersApi
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = db.Users
                .FirstOrDefault(x => x.Email == model.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            PasswordService service = new PasswordService();

            model.PasswordHash = service.HashPassword(model.PasswordHash);
            model.IsActive = true;
            model.CreatedAt = DateTime.Now;

            db.Users.Add(model);
            db.SaveChanges();

            return Ok(model);
        }

        // PUT: api/UsersApi
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(User model)
        {
            var data = db.Users.Find(model.Id);

            if (data == null)
            {
                return NotFound();
            }

            data.Name = model.Name;
            data.Email = model.Email;
            data.Role = model.Role;
            data.IsActive = model.IsActive;

            db.SaveChanges();

            return Ok(data);
        }

        // DELETE: api/UsersApi/3
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            var data = db.Users.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            db.Users.Remove(data);
            db.SaveChanges();

            return Ok("User deleted successfully");
        }




    }
}
