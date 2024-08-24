using apitask2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apitask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private MyDbContext _dbContext;
        public CategoriesController(MyDbContext db)
        {
            _dbContext = db;
        }


        [Route("api/categories/GetAllCategories")]
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _dbContext.Categories.ToList();
            return Ok(categories);
        }


        [Route("/api/categories/GetCategoryById/{id:int:min(5)}")]
        [HttpGet]
        public IActionResult GetCategoryById(int id)
        {
            var Category = _dbContext.Categories.Where(p => p.CategoryId == id).FirstOrDefault();
            if (Category == null)
            {
                return BadRequest();
            }
            return Ok(Category);
        }


        [Route("/api/Categories/GetCategoryByName/{name}")]
        [HttpGet]
        public IActionResult GetCategoryByName(string name)
        {
            var category = _dbContext.Categories.Where(p => p.CategoryName == name).FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }


        [Route("/api/categories/DeleteCategory/{id}")]
        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var products = _dbContext.Products.Where(p => p.CategoryId == id).ToList();
            _dbContext.Products.RemoveRange(products);
            _dbContext.SaveChanges();

            var category = _dbContext.Categories.FirstOrDefault(p => p.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return Ok("the category has been deleted");
        }


    }
}
