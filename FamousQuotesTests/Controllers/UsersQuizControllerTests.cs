using Microsoft.VisualStudio.TestTools.UnitTesting;
using FamousQuotes.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamousQuotes.Models;
using FamousQuotesTests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuotes.Controllers.Tests
{
    [TestClass()]
    public class UsersQuizControllerTests
    {
        private UsersQuizController controller = new(TestDbContext.db);
        private UsersController UserController = new (TestDbContext.db);
        private QuotesController quotesController = new (TestDbContext.db);


        [TestMethod()]
        public async Task GetQuotesTest()
        {
            var list = await controller.Get();
            foreach (var item in list)
            {
                Console.WriteLine(item.WasCorrect);
            }
        }

        [TestMethod()]
        public async Task GetQuoteTest()
        {
            var model = (await controller.Get()).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesTest();
                model = (await controller.Get()).FirstOrDefault();
            }
            var item = await controller.Get(model.IdUsersQuzi);
            if(item == null)
                Assert.Fail();
            else
                Console.WriteLine(item.WasCorrect);
        }

        [TestMethod()]
        public async Task PostQuotesTest()
        {
            var user = (await UserController.Get()).FirstOrDefault();
            if (user == null)
                Assert.Fail("No Users defined");
            var quiz = (await quotesController.GetQuotes()).FirstOrDefault();
            if(quiz == null)
                Assert.Fail("No Quiz defined");
            var quizAnswer = (await quotesController.GetQuoteAuthors(quiz.IdQuotes)).FirstOrDefault();
            if(quizAnswer == null)
                Assert.Fail("No Quiz answers defined");

            var model = new UsersQuzi()
            {
                Date = DateTime.Now,
                IdUsers = user.IdUsers,
                IdQuotes = quiz.IdQuotes,
                IdQuotesAuthors = quizAnswer.IdQuotesAuthors,
                StartTime = new TimeSpan?(),
                WasCorrect = false,
            };
            var res = await controller.Post(model);
            if (res is OkObjectResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }

        [TestMethod()]
        public async Task PutQuotesTest()
        {
            var model = (await controller.Get()).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesTest();
                model = (await controller.Get()).FirstOrDefault();
            }
                
            model.WasCorrect = true;
            model.EndTime = new TimeSpan();
            var res = await controller.Put(model);
            if (res is OkResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }

        [TestMethod()]
        public async Task DeleteQuotesTest()
        {
            var model = (await controller.Get()).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesTest();
                model = (await controller.Get()).FirstOrDefault();
            }
            var res = await controller.Delete(model.IdUsersQuzi);
            if (res is OkResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }
    }
}