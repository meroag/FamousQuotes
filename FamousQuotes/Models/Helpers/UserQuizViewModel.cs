using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace FamousQuotes.Models.Helpers
{
    public class UserQuizViewModel
    {
        public long IdUsersQuzi { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public long IdUsers { get; set; }
        public string Users { get; set; }
        public long IdQuotes { get; set; }
        public string Quotes { get; set; }
        public long IdQuotesAuthors { get; set; }
        public string QuotesAuthors { get; set; }
        public bool WasCorrect { get; set; }
    }
}
