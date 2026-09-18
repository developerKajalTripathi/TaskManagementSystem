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
    public class TeamMembersController : ApiController
    {

        [RoutePrefix("api/TeamMembersApi")]
        [JwtAuthorize]
        [JwtRoleAuthorize("Admin", "Manager")]
        public class TeamMembersApiController : ApiController
        {
            AppDbContext db = new AppDbContext();

            // GET: api/TeamMembersApi
            [HttpGet]
            [Route("GetAll")]
            public IHttpActionResult GetAll()
            {
                var data = db.TeamMembers.ToList();

                return Ok(data);
            }

            // GET: api/TeamMembersApi/1
            [HttpGet]
            [Route("GetById/{id}")]
            public IHttpActionResult GetById(int id)
            {
                var data = db.TeamMembers.Find(id);

                if (data == null)
                {
                    return NotFound();
                }

                return Ok(data);
            }

            // POST: api/TeamMembersApi
            [HttpPost]
            [Route("")]
            public IHttpActionResult Insert(TeamMember model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.TeamMembers.Add(model);
                db.SaveChanges();

                return Ok(new
                {
                    Message = "Team member saved successfully"
                });
            }

            // PUT: api/TeamMembersApi
            [HttpPut]
            [Route("Update")]
            public IHttpActionResult Update(TeamMember model)
            {
                var data = db.TeamMembers.Find(model.Id);

                if (data == null)
                {
                    return NotFound();
                }

                data.TeamId = model.TeamId;
                data.UserId = model.UserId;

                db.SaveChanges();

                return Ok(new
                {
                    Message = "Team member updated successfully"
                });
            }

            // DELETE: api/TeamMembersApi/1
            [HttpDelete]
            [Route("{id:int}")]
            public IHttpActionResult Delete(int id)
            {
                var data = db.TeamMembers.Find(id);

                if (data == null)
                {
                    return NotFound();
                }

                db.TeamMembers.Remove(data);
                db.SaveChanges();

                return Ok(new
                {
                    Message = "Team member deleted successfully"
                });
            }


        }

        }
}
