using ANPCentral.Data;
using ANPCentral.ViewModels;
using ANPCentral.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ANPCentral.DTOS;
using ANPCentral.Services;
using Microsoft.AspNetCore.Identity;

namespace ANPCentral.Controllers
{
    [ApiController]
    public class UserControllers : ControllerBase
    {
        #region Get
        [HttpGet("users")]
        public async Task<IActionResult> GetAsync([FromServices] UserDataContext context, IMapper mapper)
        {
            var users = await context.Users.ToListAsync();
            var usersDTO = mapper.Map<List<UserDTO>>(users);
            return Ok(usersDTO);

        }
        #endregion

        #region Create
        [HttpPost("users")]
        public async Task<IActionResult> PostAsync([FromBody] EditorUserViewModel body, [FromServices] UserDataContext context)
        {
            var userInDatabase = await context.Users.FirstOrDefaultAsync(u => u.Email == body.Email);

            if (userInDatabase != null)
                return Conflict(new { message = "User Already exists" });

                var user = new User
                {
                    Name = body.Name,
                    Email = body.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(body.Password),
                };

                await context.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"users/{user.Id}", user);
         

        }
        #endregion

        #region Get By Id
        [HttpGet("users/{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, [FromServices] UserDataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync((x) => x.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        #endregion

        #region Update
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
        #endregion

        #region Soft Delete
        [HttpDelete("users/{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromServices] UserDataContext context)
        {

            if (HttpContext.Items.TryGetValue("user", out var userObj) && userObj is User user)
            {

                user.IsActive = false;
                user.UpdatedAt = DateTime.Now;
                await context.SaveChangesAsync();

                return NoContent();
            }

            return NotFound();
        }
        #endregion

        #region Login
        [HttpPost("users/login")]
        public async Task<IActionResult> Login(
        [FromBody] LoginViewModel body,
        [FromServices] UserDataContext context,
        [FromServices] TokenService tokenService)
        {
  
            var user = await context
                .Users
                .FirstOrDefaultAsync(x => x.Email == body.Email);

            if (user == null)
                return StatusCode(401, new { message = "E-mail or password invalid" });

            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, body.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                try
                {
                    var token = tokenService.GenerateToken(user);
                    return Ok(new { token });
                }
                catch
                {
                    return StatusCode(500, new { message = "Internal Server Error" });
                }
            }

            return StatusCode(401, new { message = "E-mail or password invalid" });
        }

        #endregion
    }
}