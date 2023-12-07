using BlogAspNet.Data;
using BlogAspNet.Extensions;
using BlogAspNet.Models;
using BlogAspNet.Services;
using BlogAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BlogAspNet.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1/accounts/")]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = new User
            {
                Name = model.Email,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-")
            };

            var password = PasswordGenerator.Generate(25, true, false);

            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(400, new ResultViewModel<string>("05X99 - Este Email já está cadastrado!"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X85 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/accounts/login")]
        public IActionResult login([FromServices] TokenService tokenService)
        {
            var token = tokenService.GenerateToken(null);

            return Ok(token);
        }

       

    }
}
