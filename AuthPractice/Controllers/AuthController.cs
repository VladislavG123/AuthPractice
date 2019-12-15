using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthPractice.DataAccess;
using AuthPractice.DTO;
using AuthPractice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthPractice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext context;

        public AuthController(UserContext context)
        {
            this.context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp(AuthDTO auth)
        {
            var user = new User { Name = auth.Name, Password = auth.Password };
            context.Add(user);
            await context.SaveChangesAsync();
            return new JsonResult(new { data = new { secureCode = user.SecureCode } });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AuthDTO auth)
        {
            var user = (await context.Users.Where(user => user.Name == auth.Name && user.Password == auth.Password).ToListAsync())[0];
            if (!(user is null))
            {
                return new JsonResult(new { data = new { secureCode = user.SecureCode } });
            }
            
            return StatusCode(401);
        }
    }
}