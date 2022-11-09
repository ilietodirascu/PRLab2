using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogInfoController : ControllerBase
    {
        private ILogger _logger;
        public LogInfoController(ILogger<LogInfoController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public IActionResult LogInfo(Product product)
        {
            _logger.LogInformation(product.Data);
            return Ok();
        }
    }
}