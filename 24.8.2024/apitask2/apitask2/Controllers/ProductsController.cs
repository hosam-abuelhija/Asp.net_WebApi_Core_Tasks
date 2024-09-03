using apitask2.DTOs;
using apitask2.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _dbContext.Products.ToList();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult addProduct([FromForm] productrequestDTO add)
        {
            var catId = _dbContext.Categories.Find(add.CategoryId);
            if (catId != null)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                var imgFile = Path.Combine(uploadFolder, add.ProductImage.FileName);
                using (var stream = new FileStream(imgFile, FileMode.Create))
                {
                    add.ProductImage.CopyToAsync(stream);
                }
                var newproduct = new Product()
                {
                    ProductName = add.ProductName,
                    Price = add.Price,
                    ProductImage = add.ProductImage.FileName,
                    CategoryId = add.CategoryId,
                    Description = add.Description,
                };
                _dbContext.Products.Add(newproduct);
                _dbContext.SaveChanges();
                return Ok(newproduct);
        }
            return BadRequest();
    }


        [HttpPut("{id}")]
        public IActionResult editProduct(int id, [FromForm] productrequestDTO edit)
        {
            var product1 = _dbContext.Products.Find(id);
            if (product1 == null)
            {
                return BadRequest();
            }
            var catId = _dbContext.Categories.Find(edit.CategoryId);
            if (catId != null)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                var imgFile = Path.Combine(uploadFolder, edit.ProductImage.FileName);
                using (var stream = new FileStream(imgFile, FileMode.Create))
                {
                    edit.ProductImage.CopyToAsync(stream);
                }
                product1.ProductName = edit.ProductName;
                product1.Price = edit.Price;
                product1.ProductImage = edit.ProductImage.FileName;
                product1.CategoryId = edit.CategoryId;
                product1.Description = edit.Description;
                
                _dbContext.Products.Update(product1);
                _dbContext.SaveChanges();
                return Ok(product1);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("GetAllProductsDesc")]
        public IActionResult GetAllProductsDesc()
        {
            var products = _dbContext.Products.OrderByDescending(p => Convert.ToDecimal(p.Price)).ToList();
            return Ok(products);
        }


        [Authorize(Roles = "Admin")]
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


        [HttpGet]
        [Route  ("/potato")]
        public IActionResult getfive()
        {
          var  pros = _dbContext.Products.OrderBy(p => p.ProductName).ToList().TakeLast(5);
            return Ok(pros);
        }

    }
}
