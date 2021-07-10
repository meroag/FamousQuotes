using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FamousQuotes.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FamousQuotes.Auth
{
    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public MyDbContext DbContext { get; set; }
    }

    public class TokenAuthentication:AuthenticationHandler<TokenAuthenticationOptions>
    {
        public TokenAuthentication(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        public const string SchemeName = "FamousQuotesAuthorization";

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync () 
        {

            var headers = Request.Headers;

            if (!headers.ContainsKey("Auth-Token"))
                return AuthenticateResult.Fail("You are not authenticated");
            var token = headers["Auth-Token"];
            if(string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("You are not authenticated");
            var session = await Options.DbContext.UsersSession
                .Include(x=>x.IdUsersNavigation)
                .FirstOrDefaultAsync(x => x.Token == token);
            if(session == null)
                return AuthenticateResult.Fail("Token is invalid");

            var principal = new ClaimsPrincipal();
            return AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName));
        }


    }
}
