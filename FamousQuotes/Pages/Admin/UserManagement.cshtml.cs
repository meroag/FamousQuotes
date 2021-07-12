using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FamousQuotes.Helpers;
using FamousQuotes.Models;
using FamousQuotes.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamousQuotes.Pages.Admin
{
    public class UserManagementModel : PageModel
    {
        private HttpClient client = new();
        public List<Users> Data { get; set; }
        public async Task OnGet()
        {
            client.BaseAddress = MyToolKit.GetBaseUrl(Request);
            Data = await client.GetFromJsonAsync<List<Users>>("/api/Users");
        }

        public async Task<IActionResult> OnPost(LoginUserModel model)
        {
            client.BaseAddress = MyToolKit.GetBaseUrl(Request);
            if(model.IdUsers == 0) 
                await client.PostAsJsonAsync("/api/Users",model);
            else
                await client.PutAsJsonAsync("/api/Users",model);
            return Redirect("/Admin/UserManagement");
        }
    }
}
