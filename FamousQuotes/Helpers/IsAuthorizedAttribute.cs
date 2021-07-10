using System.Net;
using System.Threading.Tasks;
using FamousQuotes.Controllers;
using FamousQuotes.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FamousQuotes.Helpers
{
    public class IsAuthorizedAttribute:AuthorizeAttribute,IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("AuthToken"))
            {
                context.Result = new StatusCodeResult((int) HttpStatusCode.Unauthorized);
                return;
            }

            var token = context.HttpContext.Request.Headers["AuthToken"];
            var dbContext = context.HttpContext.RequestServices.GetService<MyDbContext>();
            var authController = new AuthorizationController(dbContext);
            var user = await authController.CheckToken(token);
            if(!(user is OkResult))
                context.Result = new StatusCodeResult((int) HttpStatusCode.Unauthorized);

        }
    }
}
