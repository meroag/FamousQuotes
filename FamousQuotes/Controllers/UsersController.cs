using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuotes.Data;
using FamousQuotes.Helpers;
using FamousQuotes.Models;
using FamousQuotes.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FamousQuotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public UsersController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
        {
            return await _dbContext.Users
                .Select(x=>new Users()
                {
                    IdUsers = x.IdUsers,
                    DisplayName = x.DisplayName,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                    IsAdmin = x.IsAdmin
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Users> Get(long id)
        {
            return await _dbContext.Users
                .Select(x=>new Users()
                {
                    IdUsers = x.IdUsers,
                    DisplayName = x.DisplayName,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                    IsAdmin = x.IsAdmin
                })
                .FirstOrDefaultAsync(x=>x.IdUsers == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginUserModel model)
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
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(Post)}");
                return Problem();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] LoginUserModel model)
        {
            try
            {
                var oldUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdUsers == model.IdUsers);
                if (oldUser == null)
                    return NotFound();
                oldUser.DisplayName = model.DisplayName;
                oldUser.Email = model.User;
                oldUser.IsEnabled = model.IsEnabled;
                oldUser.IsAdmin = model.IsAdmin;
                if(!string.IsNullOrEmpty(model.Password))
                    oldUser.PasswordHash = LoginHelper.GetSaltedPassword(model.Password, oldUser.PasswordSalt);
                _dbContext.Users.Update(oldUser);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(Put)}");
                return Problem();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var oldUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdUsers == id);
                if (oldUser == null)
                    return NotFound();

                _dbContext.Users.Remove(oldUser);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(Delete)}");
                return Problem();
            }
        }
    }
}
