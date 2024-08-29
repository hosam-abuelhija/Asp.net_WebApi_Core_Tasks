using apitask2.DTOs;
using apitask2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace apitask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private MyDbContext _dbContext;
        public CartItemsController(MyDbContext db)
        {
            _dbContext = db;
        }

        [HttpGet("{cId}")]
        public IActionResult GetCartItems(int cId)
        {
            var cartItems = _dbContext.CartItems.Where( i => i.CartId == cId )
                .Select(c => new CartitemresponseDTO
            {
                CartItemId = c.CartItemId,
                CartId = c.CartId,
                Quantity = c.Quantity,
                Product = new productresponseDTO
                {
                    ProductName = c.Product.ProductName,
                    Price = c.Product.Price,
                }

            });

            return Ok(cartItems);

        }


        [HttpPost]
        public IActionResult PostCartItem([FromBody] cartItemRequestDTO item )
        {
            var newitem = new CartItem
            {
                CartId = item.CartId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            };
            _dbContext.CartItems.Add(newitem);
            _dbContext.SaveChanges();
            return Ok(newitem);
        }


        [HttpPut("Edit/{id}")]
        public IActionResult Edit([FromBody] cartitemputrequestDTO quantity , int id)
        {
            var item = _dbContext.CartItems.Find(id);
            if (item == null) { return BadRequest(); };
            item.Quantity = quantity.Quantity;
            _dbContext.CartItems.Update(item);
            _dbContext.SaveChanges();
            return Ok(item);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id) 
        {
            var item = _dbContext.CartItems.Find(id);
            _dbContext.CartItems.Remove(item);
            _dbContext.SaveChanges();
            return Ok(item);
        }



    }
}
