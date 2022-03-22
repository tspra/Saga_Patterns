using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifier.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotifierController : ControllerBase
    {

        [HttpPost]
        public  int Post([FromBody] Notifier notifier)
        {
            Console.WriteLine($"Send notification for :{notifier.ProductName}");
            return 3;
        }

        [HttpDelete("{id}")]
        public  void Delete(int id)
        {
            Console.WriteLine($"Send rollback transaction for :{id}");
        }
    }
}
