using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuotes.Models.Helpers
{
    public class LoginUserModel
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}
