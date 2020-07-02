using MediaApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.Controllers
{
    public class StatusController : ControllerBase
    {
        ISystemTime Clock;

        public StatusController(ISystemTime clock)
        {
            Clock = clock;
        }

        // GET /status
        [HttpGet("status")]
        public ActionResult<StatusResponse> GetStatus()
        {
            var response = new StatusResponse
            {
                Message = "Everything is groovy!",
                CreatedAt = Clock.GetCurrent()
            };
            return Ok(response); // this will return a 200
        }

        // GET /sayhi/Jeff
        [HttpGet("sayhi/{name:minlength(3)}")]
        public IActionResult SayHi(string name)
        {
            return Ok($"Hello, {name}!");
        }

        // Get /employees?department=DEV        // filtering the employees by their department
        [HttpGet("employees")]
        public IActionResult GetEmployees(string department = "All")
        {
            return Ok($"Getting you the employees in department {department}");
        }
        
        // I want to hire somebody
        [HttpPost("employees")]
        public IActionResult HireEmployee([FromBody] HiringRequest employeeToHire)
        {
            // Validation, etc. we'll talk about this afternoon
            return Ok($"Hiring {employeeToHire.FirstName} as a {employeeToHire.Department}");
        }        
    }

    public class StatusResponse
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class HiringRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal StartingSalary { get; set; }
        public string Department { get; set; }
    }
}
