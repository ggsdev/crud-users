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
        public async Task<IActionResult> GetAsync([FromServices] DataContext context)
        {
            var users = await context.Users.ToListAsync();
            var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
                userDTOs.Add(userDTO);
            }

            return Ok(userDTOs);

        }
        #endregion

        #region Create
        [HttpPost("users")]
        public async Task<IActionResult> PostAsync([FromBody] EditorUserViewModel body, [FromServices] DataContext context)
        {
            var userInDatabase = await context.Users.FirstOrDefaultAsync((x) => x.Email == body.Email);


            if (userInDatabase != null)
                return StatusCode(409, new {message = "User with this e-mail already exists."});

            try
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var user = new User
                {
                    Name = body.Name,
                    Email = body.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword( body.Password, salt),
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
        #endregion

        #region Get By Id
        [HttpGet("users/{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, [FromServices] DataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync((x) => x.Id == id);
            if (user == null)
                return NotFound();

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,

            };
           

            return Ok(userDTO);
        }
        #endregion

        #region Update PATCH
        [HttpPatch("users/{id:Guid}")]
        public async Task<IActionResult> UpdatePartialAsync([FromRoute] Guid id, [FromBody] EditorUserViewModel body, [FromServices] DataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync((x) => x.Id == id);

            if (user == null)
                return NotFound();

            try
            {
                if (body.Name != null) user.Name = body.Name;

                if (body.Password != null) user.Password = BCrypt.Net.BCrypt.HashPassword(body.Password);

                if (body.Email != null) {



                    user.Email = body.Email;
                 };

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(user);
            }
            catch
            {
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }
        #endregion

        #region Update PUT
        [HttpPut("users/{id:Guid}")]
        public async Task<IActionResult> UpdateAllAsync([FromRoute] Guid id ,[FromBody] EditorUserViewModel body, [FromServices] DataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync((x) => x.Id == id);

            if (user == null)
                return NotFound();

            try
            {
                user.Name = body.Name;
                user.Email = body.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(body.Password);

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(user);

            } catch
            {
                return StatusCode(500, new { message = "Internal Server Error" });

            }
        }
        #endregion

        #region Delete
        [HttpDelete("users/{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, [FromServices] DataContext context)
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
        #endregion

        #region Login
        [HttpPost("users/login")]
        public async Task<IActionResult> Login(
        [FromBody] LoginViewModel body,
        [FromServices] DataContext context,
        [FromServices] TokenService tokenService)
        {
            var user = await context
                .Users
                .FirstOrDefaultAsync(x => x.Email == body.Email);

            if (user == null)
                return StatusCode(401, new { message = "E-mail or password invalid" });

            var passwordVerificationResult = BCrypt.Net.BCrypt.Verify(body.Password, user.Password);

            if (passwordVerificationResult == true)
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