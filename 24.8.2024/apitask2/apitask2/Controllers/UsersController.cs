using apitask2.DTOs;
using apitask2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Data;
using System.Diagnostics;

namespace apitask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MyDbContext _dbContext;
        private TokenGenerator _tokenGenerator;
        public UsersController(MyDbContext db, TokenGenerator tokenGenerator)
        {
            _dbContext = db;
            _tokenGenerator = tokenGenerator;
        }



        [Route("api/Users/GetAllUsers")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var Users = _dbContext.Users.ToList();
            return Ok(Users);
        }


        [Route("/api/Users/GetUserById")]
        [HttpGet]
        public IActionResult GetUserById(int id)
        {
            var User = _dbContext.Users.Where(p => p.UserId == id).FirstOrDefault();
            if (User == null)
            {
                return BadRequest();
            }
            return Ok(User);
        }


        [Route("/api/Users/GetUserByName/{name}")]
        [HttpGet]
        public IActionResult GetUserByName(string name)
        {
            var user = _dbContext.Users.Where(p => p.Username == name).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [Route("/api/Users/DeleteUser/{id}")]
        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var orders = _dbContext.Orders.Where(p => p.UserId == id).ToList();
            _dbContext.Orders.RemoveRange(orders);
            _dbContext.SaveChanges();

            var user = _dbContext.Users.FirstOrDefault(p => p.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return Ok("the user has been deleted");
        }



        [HttpPost("Regester")]
        public IActionResult AddUser([FromForm] userRequestDTO add)
        {
            byte[] hash, salt;
            PasswordHash.CreatePasswordHash(add.Password, out hash, out salt);
            var newuser = new User()
            {
                Email = add.Email,
                Username = add.Username,
                Password = add.Password,
                PasswordSalt = salt,
                PasswordHash = hash

            };
            _dbContext.Users.Add(newuser);
            _dbContext.SaveChanges();
            return Ok(newuser);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromForm] LoginDTO user)
        {
            var dbuser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (dbuser == null || !PasswordHash.VerifyPasswordHash(user.Password, dbuser.PasswordHash, dbuser.PasswordSalt))
            {
                return Unauthorized("Login Unauthorized!");
            }
            var roles = _dbContext.UserRoles.Where(u => u.User.UserId == dbuser.UserId).Select(r =>  r.Role).ToList();
            var token = _tokenGenerator.GenerateToken(dbuser.Email, roles);

            return Ok(new { Token = token });

        }


        [HttpPut("{id}")]
        public IActionResult editUser(int id, [FromForm] userRequestDTO edit)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return BadRequest();
            }

            user.Email = edit.Email;
            user.Username = edit.Username;
            user.Password = edit.Password;

            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return Ok(user);
        }




        [HttpGet("get/{username}")]
        public IActionResult getuserbyname(string username)
        {

            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            return Ok(user);
        }

    }

}
