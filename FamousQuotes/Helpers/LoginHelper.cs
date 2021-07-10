using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace FamousQuotes.Helpers
{
    internal class LoginHelper
    {
        private const string Salt = "G4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKx";
        private const string tokenKey = "GLGOld7PpQpXJR62VEi9pWkU691tqhoj";

        public static string GetSaltedPassword(string password,string privateSalt)
        {
            return GetHash(password + Salt + privateSalt);
        }

        public static string GetToken(string userId,string userName,string displayName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, userId),
                    new(ClaimTypes.Name, userName),
                    new(ClaimTypes.GivenName, displayName),
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));;
        }

        static string GetHash(string input)
        {
            var algorithm = MD5.Create();

            byte[] encodedPassword = new UTF8Encoding().GetBytes(input);
            byte[] hash = algorithm.ComputeHash(encodedPassword);
            var res = new StringBuilder();
            foreach (byte b in hash)
            {
                res.Append(b.ToString("X2"));
            }
            return res.ToString();
        }
    }
}
