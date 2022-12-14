using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddProducerDataController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddData(Product product)
        {
            Simulation.ProducerProducts.Enqueue(product);
            return Ok();
        }
    }
}
