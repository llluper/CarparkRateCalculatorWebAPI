using System;
using carparkRateCalculator.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace carparkRateCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        enum RateType { Early, Night, Weekend, Standard };

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Server running...";
        }

        // POST api/ticket
        [EnableCors("AllowSpecificOrigin")]
        [HttpPost]
        public ActionResult<Rate> Post([FromBody] Ticket tick)
        {
            if (tick == null)
            {
                throw new ArgumentNullException(nameof(tick));
            }
            else
            {
                TicketMachine ticket = new TicketMachine(tick);
                ticket.ProcessTicket();
                return ticket.GetRate();
            }
        }
    }
}
