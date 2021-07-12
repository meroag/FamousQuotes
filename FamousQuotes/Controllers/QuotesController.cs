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
    public class QuotesController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public QuotesController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Master

        [HttpGet]
        public async Task<IEnumerable<Quotes>> GetQuotes()
        {
            return await _dbContext.Quotes.ToListAsync();
        }

        [HttpGet]
        public async Task<Quotes> GetQuote(long id)
        {
            return await _dbContext.Quotes.FirstOrDefaultAsync(x=>x.IdQuotes == id);
        }

        [HttpPost]
        public async Task<IActionResult> PostQuotes([FromBody] Quotes model)
        {
            try
            {
                model.CreateDate = DateTime.Now;
                _dbContext.Quotes.Add(model);
                await _dbContext.SaveChangesAsync();
                return Ok(model.IdQuotes);
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(PostQuotes)}");
                return Problem();
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutQuotes([FromBody] Quotes model)
        {
            try
            {
                var oldModel = await _dbContext.Quotes.FirstOrDefaultAsync(x => x.IdQuotes == model.IdQuotes);
                if (oldModel== null)
                    return NotFound();
                MyToolKit.CopyModel(model,oldModel);
                oldModel.ModifyDate = DateTime.Now;
                foreach (QuotesAuthors author in model.QuotesAuthors.ToList())
                {
                    oldModel.QuotesAuthors.Add(author);
                }

                _dbContext.Quotes.Update(oldModel);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(PutQuotes)}");
                return Problem();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuotes(long id)
        {
            try
            {
                var oldModel = await _dbContext.Quotes.FirstOrDefaultAsync(x => x.IdQuotes == id);
                if (oldModel == null)
                    return NotFound();

                _dbContext.Quotes.Remove(oldModel);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(DeleteQuotes)}");
                return Problem();
            }
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IEnumerable<QuotesAuthors>> GetQuoteAuthors(long masterId)
        {
            return await _dbContext.QuotesAuthors.Where(x=>x.IdQuotes == masterId).ToListAsync();
        }

        [HttpGet]
        public async Task<QuotesAuthors> GetQuotesAuthor(long id)
        {
            return await _dbContext.QuotesAuthors.FirstOrDefaultAsync(x=>x.IdQuotesAuthors == id);
        }

        [HttpPost]
        public async Task<IActionResult> PostQuotesAuthors([FromBody] QuotesAuthors model)
        {
            try
            {
                _dbContext.QuotesAuthors.Add(model);
                await _dbContext.SaveChangesAsync();
                return Ok(model.IdQuotesAuthors);
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(PostQuotes)}");
                return Problem();
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutQuotesAuthors([FromBody] QuotesAuthors model)
        {
            try
            {
                var oldModel = await _dbContext.QuotesAuthors.FirstOrDefaultAsync(x => x.IdQuotesAuthors == model.IdQuotesAuthors);
                if (oldModel== null)
                    return NotFound();
                MyToolKit.CopyModel(model,oldModel);
                _dbContext.QuotesAuthors.Update(oldModel);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(PutQuotes)}");
                return Problem();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuotesAuthors(long id)
        {
            try
            {
                var oldModel = await _dbContext.QuotesAuthors.FirstOrDefaultAsync(x => x.IdQuotesAuthors == id);
                if (oldModel == null)
                    return NotFound();

                _dbContext.QuotesAuthors.Remove(oldModel);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(AuthorizationController)}:{nameof(DeleteQuotes)}");
                return Problem();
            }
        }

        #endregion
    }
}
