using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddAggregatorDataController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddData(Product product)
        {
            Simulation.AggregatorProducts.Enqueue(product);
            return Ok();
        }
    }
}
