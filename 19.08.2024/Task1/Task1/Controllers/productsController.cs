using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var product = _dbContext.Products
                                    .Include(p => p.Category) // Ensure Category is included
                                    .FirstOrDefault(p => p.ProductId == id);

            var product1 = new
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                ProductImage = product.ProductImage,
                CategoryName = product.Category.CategoryName
            };

            return Ok(product1);
        }


        [HttpGet("{id}/{price}")]
        public IActionResult Producttt(int id, int price)
        {
            var data = _dbContext.Products.Where(c => c.CategoryId == id && Convert.ToDecimal(c.Price) > price).Count();
            return Ok(data);
        }
    }
}
