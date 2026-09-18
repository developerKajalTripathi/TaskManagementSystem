using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using TaskManagementSystem.Data;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.ApiControllers
{

    [RoutePrefix("api/AuthApi")]
    public class AuthApiController : ApiController
    {

        AppDbContext db = new AppDbContext();


        //[Route("Login")]
        //[HttpPost]
        //public IHttpActionResult Login(LoginModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    PasswordService service = new PasswordService();

        //    var user = db.Users.FirstOrDefault(x => x.Email == model.Email && x.IsActive == true);

        //    if (user == null)
        //        return Unauthorized();

        //    bool passwordValid = service.VerifyPassword(
        //        model.Password,
        //        user.PasswordHash
        //    );

        //    if (!passwordValid)
        //        return Unauthorized();

        //    return Ok(new
        //    {
        //        Message = "Login successful",
        //        UserId = user.Id,
        //        Name = user.Name,
        //        Email = user.Email,
        //        Role = user.Role,
        //        Password = user.PasswordHash
        //    });
        //}


        //[HttpPost]
        //[Route("Login")]
        //public IHttpActionResult Login(LoginModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var user = db.Users.FirstOrDefault(x =>
        //        x.Email == model.Email &&
        //        x.PasswordHash == model.Password &&
        //        x.IsActive == true
        //    );

        //    if (user == null)
        //        return Unauthorized();

        //    return Ok(new
        //    {
        //        Message = "Login successful",
        //        UserId = user.Id,
        //        Name = user.Name,
        //        Email = user.Email,
        //        Role = user.Role
        //    });
        //}



        //jwt
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Database se Email + Password match
            var user = db.Users.FirstOrDefault(x =>
                x.Email == model.Email &&
                x.PasswordHash == model.Password &&
                x.IsActive == true
            );

            if (user == null)
                return Unauthorized();


            // JWT Settings
            string key = ConfigurationManager.AppSettings["JwtKey"];
            string issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            string audience = ConfigurationManager.AppSettings["JwtAudience"];

            int expiryMinutes = Convert.ToInt32(
                ConfigurationManager.AppSettings["JwtExpiryMinutes"]
            );


            // Secret Key
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256Signature
            );


            // JWT ke andar user information
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };


            // Token create
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),

                Issuer = issuer,

                Audience = audience,

                SigningCredentials = credentials
            };


            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);


            // Response
            return Ok(new
            {
                Message = "Login successful",
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = tokenString,
                ExpiresInMinutes = expiryMinutes
            });
        }




    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }





}
