using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuotes.Models.Helpers
{
    public class LoginUserModel
    {
        public long IdUsers { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string DisplayName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsAdmin { get; set; }
    }
}
