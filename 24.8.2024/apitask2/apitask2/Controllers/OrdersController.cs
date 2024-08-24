using apitask2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apitask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private MyDbContext _dbContext;
        public OrdersController(MyDbContext db)
        {
            _dbContext = db;
        }
        

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _dbContext.Orders.ToList();
            return Ok(orders);
        }


        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _dbContext.Orders.Where(p => p.OrderId == id).FirstOrDefault();
            return Ok(order);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _dbContext.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
            return Ok("the order has been deleted");

        }
    }
}
