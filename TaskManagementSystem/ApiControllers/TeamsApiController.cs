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


    [RoutePrefix("api/TeamsApi")]
    [JwtAuthorize]
    [JwtRoleAuthorize("Admin", "Manager")]
    public class TeamsApiController : ApiController
    {

        AppDbContext db = new AppDbContext();

        // GET: api/TeamsApi
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var data = db.Teams.ToList();

            return Ok(data);
        }

        // GET: api/TeamsApi/1
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(int id)
        {
            var data = db.Teams.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        // POST: api/TeamsApi
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(Team model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CreatedAt = DateTime.Now;

            db.Teams.Add(model);
            db.SaveChanges();

            return Ok(new
            {
                Message = "Team saved successfully"
            });

            //return Ok(model);
        }

        // PUT: api/TeamsApi
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(Team model)
        {
            var data = db.Teams.Find(model.Id);

            if (data == null)
            {
                return NotFound();
            }

            data.Name = model.Name;
            data.ManagerId = model.ManagerId;

            db.SaveChanges();

           // return Ok(data);
            return Ok(new
            {
                Message = "Team Update successfully"
            });
        }

        // DELETE: api/TeamsApi/1
        [HttpDelete]
        [Route("DELETE")]
        public IHttpActionResult Delete(int id)
        {
            var data = db.Teams.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            db.Teams.Remove(data);
            db.SaveChanges();

            return Ok("Team deleted successfully");
        }



    }
}
