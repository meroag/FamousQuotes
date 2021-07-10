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

        /// <summary>
        /// Checks if token is valid
        /// </summary>
        /// <param name="token">User token</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckToken(string token)
        {
            try
            {
                var season = await _dbContext.UsersSession.FirstOrDefaultAsync(x => x.Token == token);
                if (season == null) 
                    return Unauthorized();
                if (DateTime.Now < season.LoginTime.AddHours(12))
                    return Unauthorized();
                return Ok(season);
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(CheckToken)}");
                return Problem();
            }
        }

        /// <summary>
        /// Login to our system using email and password
        /// </summary>
        /// <param name="model">User and password model</param>
        /// <returns></returns>
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

        /// <summary>
        /// Logout from our system
        /// </summary>
        /// <param name="token">User token</param>
        /// <returns></returns>
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
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(LogOut)}");
                return Problem();
            }
        }

    }
}
