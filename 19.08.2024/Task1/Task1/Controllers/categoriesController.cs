using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task1.Models;

namespace Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class categoriesController : ControllerBase
    {
        private MyDbContext _dbContext;
        public categoriesController(MyDbContext db)
        {
            _dbContext = db;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            var data = _dbContext.Categories.ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Categories(int id)
        {
            var data = _dbContext.Categories.Find(id);
            return Ok(data);
        }




    }
}
