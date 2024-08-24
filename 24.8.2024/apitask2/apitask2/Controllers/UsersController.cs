using apitask2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
