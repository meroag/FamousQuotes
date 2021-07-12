using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FamousQuotes.Helpers;
using FamousQuotes.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamousQuotes.Pages.Admin
{
    public class ReviewUserAchievementsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public DateTime DateFrom { get; set; } = DateTime.Now.AddDays(-10);
        
        [BindProperty(SupportsGet = true)]
        public DateTime DateTo { get; set; }= DateTime.Now.AddDays(1);

        public List<UserQuizViewModel> Data { get; set; }
        private HttpClient client = new();

        public async Task OnGet()
        {
            client.BaseAddress = MyToolKit.GetBaseUrl(Request);

            var model = new DateFilterModel()
            {
                DateFrom = DateFrom,
                DateTo = DateTo
            };
            var response = await client.PostAsJsonAsync("/api/UsersQuiz/Get",model);
            Data = await response.Content.ReadFromJsonAsync<List<UserQuizViewModel>>();
        }

        public IActionResult OnPost()
        {
            return Redirect("/Admin/ReviewUserAchievements");
        }
    }
}
