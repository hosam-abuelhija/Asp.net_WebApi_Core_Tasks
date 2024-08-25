using apitask2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apitask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private MyDbContext _dbContext;
        public ProductsController(MyDbContext db)
        {
            _dbContext = db;
        }


        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _dbContext.Products.ToList();
            return Ok(products);
        }

        [HttpGet("GetAllProductsByCatId/{CID}")]
        public IActionResult GetAllProductsByCatId(int CID)
        {
            var products = _dbContext.Products.Where(p => p.CategoryId == CID).ToList();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _dbContext.Products.Where(p => p.ProductId == id).FirstOrDefault();
            return Ok(product);
        }


        [HttpGet("{id:int:max(10)}/{name}")]
        public IActionResult GetProductByName(int id, string name)
        {
            var product = _dbContext.Products.Where(p => p.ProductName == name && p.ProductId == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return Ok("the product has been deleted");

        }

    }
}
