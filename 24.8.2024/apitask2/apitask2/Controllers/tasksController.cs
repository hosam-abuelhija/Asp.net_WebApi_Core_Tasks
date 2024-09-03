using apitask2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apitask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tasksController : ControllerBase
    {
        private MyDbContext _dbContext;
        public tasksController(MyDbContext db)
        {
            _dbContext = db;
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

        [HttpGet("duplicates")]
        public IActionResult Getnumber(int num1, int num2, int num3, int num4, int num5)
        {
            int[] values = new[] { num1, num2, num3, num4, num5 };
            var groups = values.GroupBy(v => v).Where(g => g.Count() %2 != 0)
                .Select(y => new { number = y.Key, Count = y.Count() }).ToList();
            return Ok(groups);
        }

    }
}
