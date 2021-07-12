using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuotes.Data;
using FamousQuotes.Helpers;
using FamousQuotes.Models;
using FamousQuotes.Models.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FamousQuotes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersQuizController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public UsersQuizController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<UsersQuzi>> Get()
        {
            return await _dbContext.UsersQuzi.ToListAsync();
        }

        [HttpPost]
        public async Task<IEnumerable<UserQuizViewModel>> Get([FromBody] DateFilterModel model)
        {
            return await _dbContext.UsersQuzi
                .Where(x=>x.Date>=model.DateFrom && x.Date <= model.DateTo)
                .Include(x=>x.IdUsersNavigation)
                .Include(x=>x.IdQuotesAuthors)
                .Include(x=>x.IdQuotes)
                .Select(x=>new UserQuizViewModel()
                {
                    IdQuotes = x.IdQuotes,
                    Date = x.Date,
                    IdUsers = x.IdUsers,
                    IdQuotesAuthors = x.IdQuotesAuthors,
                    IdUsersQuzi = x.IdUsersQuzi,
                    QuotesAuthors = x.IdQuotesAuthorsNavigation.AuthorName,
                    Users = x.IdUsersNavigation.DisplayName,
                    WasCorrect = x.WasCorrect,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Quotes = x.IdQuotesNavigation.Header
                })
                .ToListAsync();
        }

        [HttpGet]
        public async Task<UsersQuzi> Get(long id)
        {
            return await _dbContext.UsersQuzi.FirstOrDefaultAsync(x=>x.IdUsersQuzi == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsersQuzi model)
        {
            try
            {
                _dbContext.UsersQuzi.Add(model);
                await _dbContext.SaveChangesAsync();
                return Ok(model.IdQuotes);
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(Post)}");
                return Problem();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UsersQuzi model)
        {
            try
            {
                var oldModel = await _dbContext.UsersQuzi.FirstOrDefaultAsync(x => x.IdUsersQuzi == model.IdUsersQuzi);
                if (oldModel== null)
                    return NotFound();
                MyToolKit.CopyModel(model,oldModel);
                _dbContext.UsersQuzi.Update(oldModel);
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
                var oldModel = await _dbContext.UsersQuzi.FirstOrDefaultAsync(x => x.IdUsersQuzi == id);
                if (oldModel == null)
                    return NotFound();

                _dbContext.UsersQuzi.Remove(oldModel);
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
