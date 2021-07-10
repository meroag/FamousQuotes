using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamousQuotes.Data;

namespace FamousQuotesTests.Helpers
{
    internal static class TestDbContext
    {
        public static MyDbContext db = new("Data Source=.;Initial Catalog=FamousQuotes;Integrated Security=True");
    }
}
