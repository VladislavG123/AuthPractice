using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthPractice.DataAccess;
using AuthPractice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext context;

        public AuthController(UserContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string name, string password)
        {
            var user = new User { Name = name, Password = password };
            context.Add(user);
            await context.SaveChangesAsync();
            return new JsonResult(new { data = new { secureCode = user.SecureCode } });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string name, string password)
        {
            var user = (await context.Users.Where(user => user.Name == name && user.Password == password).ToListAsync())[0];
            if (!(user is null))
            {
                return new JsonResult(new { data = new { secureCode = user.SecureCode } });
            }
            
            return StatusCode(401);
        }
    }
}