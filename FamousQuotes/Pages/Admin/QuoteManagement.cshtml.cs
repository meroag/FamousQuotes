using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FamousQuotes.Helpers;
using FamousQuotes.Models;
using FamousQuotes.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FamousQuotes.Pages.Admin
{
    public class QuoteManagementModel : PageModel
    {
        private HttpClient client = new();
        public List<Quotes> Data { get; set; }

        public async Task OnGet()
        {
            client.BaseAddress = MyToolKit.GetBaseUrl(Request);
            Data = await client.GetFromJsonAsync<List<Quotes>>("/api/Quotes/GetQuotes");
        }

        public async Task<IActionResult> OnPost(Quotes model)
        {
            client.BaseAddress = MyToolKit.GetBaseUrl(Request);
            model.QuotesAuthors = await client.GetFromJsonAsync<ICollection<QuotesAuthors>>("/api/Quotes/GetQuoteAuthors?masterId="+model.IdQuotes);
            if (Request.Form.ContainsKey("newAuthorsArray"))
            {
                var newAuthors = Request.Form["newAuthorsArray"];
                var list = JsonConvert.DeserializeObject<List<string>>(newAuthors);
                foreach (string s in list)
                {
                    model.QuotesAuthors.Add(new QuotesAuthors() {AuthorName = s,IsCorrectAnswer = false});
                }
            }

            if (Request.Form.ContainsKey("indexOfCorrectAnswer"))
            {
                var indexOfCorrectAnswer = Convert.ToInt32(Request.Form["indexOfCorrectAnswer"]);
                var i = 0;
                foreach (QuotesAuthors author in model.QuotesAuthors)
                {
                    author.IsCorrectAnswer = i == indexOfCorrectAnswer;
                    i++;
                }
            }


            if(model.IdQuotes == 0) 
                await client.PostAsJsonAsync("/api/Quotes/PostQuotes",model);
            else
                await client.PutAsJsonAsync("/api/Quotes/PutQuotes",model);
            return Redirect("/Admin/QuoteManagement");
        }
    }
}
