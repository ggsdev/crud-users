using ANPCentral.Data;
using ANPCentral.ViewModels;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ANPCentral.Controllers
{
    [ApiController]
    public class UserControllers : ControllerBase
    {
        [HttpGet("users")]
        public async Task<IActionResult> GetAsync([FromServices] UserDataContext context)
        {
            var users = await context.Users.ToListAsync();
            return Ok(users);

        }

        [HttpPost("users")]
        public async Task<IActionResult> PostAsync([FromBody] EditorUserViewModel body, [FromServices] UserDataContext context)
        {
            try
            {
                var user = new User
                {
                    Name = body.Name,
                    Email = body.Email,
                    //Password = BCrypt.Net.HashPassword(body.Password)
                    Password = body.Password
                };

                await context.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"users/{user.Id}", user);
            }
            catch(DbUpdateException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(500, new { message = "Internal Server Error" });
            }
           
        }

        [HttpGet("users/{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, [FromServices] UserDataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync((x) => x.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("users/{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id ,[FromBody] EditorUserViewModel body, [FromServices] UserDataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync((x) => x.Id == id);

            if (user == null)
                return NotFound();

            try
            {
                user.Name = body.Name;
                user.Email = body.Email;
                user.Password = body.Password;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(body);

            } catch
            {
                return StatusCode(500, new { message = "Internal Server Error" });

            }
        }

        [HttpDelete("users/{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, [FromServices] UserDataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync((x) => x.Id == id);

            if (user == null)
                return NotFound();

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, new { message = "Internal Server Error" });
            }

        }
    }
}