using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthPractice.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly UserContext context;

        public ServiceController(UserContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public IActionResult Index(string name)
        {
            return new JsonResult(new { data = new { text = $"Hello, {name}" } });
        } 
    }
}