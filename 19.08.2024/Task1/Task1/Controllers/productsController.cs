using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task1.Models;

namespace Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productsController : ControllerBase
    {
        private MyDbContext _dbContext;
        public productsController(MyDbContext db)
        {
            _dbContext = db;
        }


        [HttpGet]
        public IActionResult Products()
        {
            var data = _dbContext.Products.ToList();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public IActionResult Products(int id)
        {
            var data = _dbContext.Products.Find(id);
            return Ok(data);
        }
    }
}
