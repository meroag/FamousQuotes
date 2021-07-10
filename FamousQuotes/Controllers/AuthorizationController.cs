using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuotes.Data;
using FamousQuotes.Helpers;
using FamousQuotes.Models;
using FamousQuotes.Models.Helpers;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FamousQuotes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public AuthorizationController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> CheckToken(string token)
        {
            try
            {
                var season = await _dbContext.UsersSession.FirstOrDefaultAsync(x => x.Token == token);
                if (season == null) return NotFound();

                return Ok(season);
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(Login)}");
                return Problem();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == model.User);
                if (user == null) return Unauthorized();
                var passwordHash = LoginHelper.GetSaltedPassword(model.Password, user.PasswordSalt);
                if (passwordHash != user.PasswordHash) return Unauthorized();
                var token = LoginHelper.GetToken(user.IdUsers.ToString(), user.Email, user.DisplayName);
                _dbContext.UsersSession.Add(new UsersSession()
                {
                    Token = token,
                    IdUsers = user.IdUsers,
                    LoginTime = DateTime.Now
                });
                await _dbContext.SaveChangesAsync();
                return Ok(token);
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(Login)}");
                return Problem();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOut(string token)
        {
            try
            {
                var season = await _dbContext.UsersSession.FirstOrDefaultAsync(x => x.Token == token);
                if (season == null) return NotFound();
                _dbContext.UsersSession.Remove(season);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(Login)}");
                return Problem();
            }
        }

        public async Task<IActionResult> Register([FromBody] LoginUserModel model)
        {
            try
            {
                var salt = Guid.NewGuid().ToString();
                var nUser = new Users()
                {
                    DisplayName = model.DisplayName,
                    Email = model.User,
                    IsEnabled = true,
                    PasswordHash = LoginHelper.GetSaltedPassword(model.Password, salt),
                    PasswordSalt = salt
                };
                _dbContext.Users.Add(nUser);
                await _dbContext.SaveChangesAsync();
                return Ok(nUser.IdUsers);
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(Login)}");
                return Problem();
            }
        }
    }
}
