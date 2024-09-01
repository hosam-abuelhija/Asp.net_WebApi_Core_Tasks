using apitask2.DTOs;
using apitask2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace apitask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MyDbContext _dbContext;
        public UsersController(MyDbContext db)
        {
            _dbContext = db;
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
        public IActionResult Login([FromForm] userRequestDTO user)
        {
            var potato = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (potato == null || !PasswordHash.VerifyPasswordHash(user.Password, potato.PasswordHash, potato.PasswordSalt)) {
                return Unauthorized("bad potato!!!");
            }
            return Ok("good potato!!");

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


        [HttpGet]
        public IActionResult calculate(string calculation)
        {
            string[] opperation = calculation.Split(' ');
            var num1 = Convert.ToInt32(opperation[0]);
            var x = opperation[1];
            var num3 = Convert.ToInt32(opperation[2]);
            var result = 0;
            switch (x)
            {
                case ("+"):
                    result = num1 + num3;
                    break;
                case ("-"):
                    result = num1 - num3;
                    break;
            }
            return Ok(result);
        }

        [HttpGet("{num1} / {num2}")]
        public IActionResult calculation(int num1, int num2)
        {
            if (num1 == 30)
            {
                return Ok("True");
            }
            else if (num2 == 30)
            {
                return Ok("True");
            }
            else if (num1 + num2 == 30)
            {
                return Ok("True");
            }
            else
            {
                return Ok("False");
            }
        }

        [HttpGet("{num1}")]
        public IActionResult calculation1(int num1)
        {
            if (num1 > 0)
            {
                if (num1 % 3 == 0 || num1 % 7 == 0)
                {
                    return Ok("True");
                }
                else
                {
                    return Ok("False");
                }
            }
            return Ok("False");
            
        }



    }
}
