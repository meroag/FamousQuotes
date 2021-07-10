using Microsoft.VisualStudio.TestTools.UnitTesting;
using FamousQuotes.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamousQuotes.Data;
using FamousQuotes.Models;
using FamousQuotesTests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuotes.Controllers.Tests
{
    [TestClass()]
    public class QuotesControllerTests
    {
        private readonly QuotesController controller;
        public QuotesControllerTests()
        {
            controller = new QuotesController(TestDbContext.db);
        }


        [TestMethod()]
        public async Task GetQuotesTest()
        {
            var list = await controller.GetQuotes();
            foreach (var item in list)
            {
                Console.WriteLine(item.Header);
            }
        }

        [TestMethod()]
        public async Task GetQuoteTest()
        {
            var model = (await controller.GetQuotes()).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesTest();
                model = (await controller.GetQuotes()).FirstOrDefault();
            }
            var item = await controller.GetQuote(model.IdQuotes);
            if(item == null)
                Assert.Fail();
            else
                Console.WriteLine(item.Header);
        }

        [TestMethod()]
        public async Task PostQuotesTest()
        {
            var model = new Quotes()
            {
                CreateDate = DateTime.Now,
                CreatedBy = "Merab",
                Header = "Test Q",
                QuoteText = "This is long story"
            };
            var res = await controller.PostQuotes(model);
            if (res is OkObjectResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }

        [TestMethod()]
        public async Task PutQuotesTest()
        {
            var model = (await controller.GetQuotes()).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesTest();
                model = (await controller.GetQuotes()).FirstOrDefault();
            }
                
            model.Header = "Update Tested";
            var res = await controller.PutQuotes(model);
            if (res is OkResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }

        [TestMethod()]
        public async Task DeleteQuotesTest()
        {
            var model = (await controller.GetQuotes()).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesTest();
                model = (await controller.GetQuotes()).FirstOrDefault();
            }
            var res = await controller.DeleteQuotes(model.IdQuotes);
            if (res is OkResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }

        [TestMethod()]
        public async Task GetQuoteAuthorsTest()
        {
            var list = await controller.GetQuoteAuthors(1);
            foreach (var item in list)
            {
                Console.WriteLine(item.AuthorName);
            }
        }

        [TestMethod()]
        public async Task GetQuotesAuthorTest()
        {
            var master = (await controller.GetQuotes()).FirstOrDefault();
            if (master == null)
            {
                await PostQuotesTest();
                master = (await controller.GetQuotes()).FirstOrDefault();
            }

            var model = (await controller.GetQuoteAuthors(master.IdQuotes)).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesAuthorsTest();
                model = (await controller.GetQuoteAuthors(master.IdQuotes)).FirstOrDefault();
            }
            var item = await controller.GetQuotesAuthor(model.IdQuotesAuthors);
            if(item == null)
                Assert.Fail();
            else
                Console.WriteLine(item.AuthorName);
        }

        [TestMethod()]
        public async Task PostQuotesAuthorsTest()
        {
            var master = (await controller.GetQuotes()).FirstOrDefault();
            if (master == null)
            {
                await PostQuotesTest();
                master = (await controller.GetQuotes()).FirstOrDefault();
            }
            var model = new QuotesAuthors()
            {
                IdQuotes = master.IdQuotes,
                AuthorName = "Vaja",
            };
            var res = await controller.PostQuotesAuthors(model);
            if (res is OkObjectResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }

        [TestMethod()]
        public async Task PutQuotesAuthorsTest()
        {
            var master = (await controller.GetQuotes()).FirstOrDefault();
            if (master == null)
            {
                await PostQuotesTest();
                master = (await controller.GetQuotes()).FirstOrDefault();
            }

            var model = (await controller.GetQuoteAuthors(master.IdQuotes)).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesAuthorsTest();
                model = (await controller.GetQuoteAuthors(master.IdQuotes)).FirstOrDefault();
            }

            model.IsCorrectAnswer = true;
            model.AuthorName = "Ilia chavchavadze";
            var res = await controller.PutQuotesAuthors(model);
            if (res is OkResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }

        [TestMethod()]
        public async Task DeleteQuotesAuthorsTest()
        {
            var master = (await controller.GetQuotes()).FirstOrDefault();
            if (master == null)
            {
                await PostQuotesTest();
                master = (await controller.GetQuotes()).FirstOrDefault();
            }

            var model = (await controller.GetQuoteAuthors(master.IdQuotes)).FirstOrDefault();
            if (model == null)
            {
                await PostQuotesAuthorsTest();
                model = (await controller.GetQuoteAuthors(master.IdQuotes)).FirstOrDefault();
            }
            var res = await controller.DeleteQuotesAuthors(model.IdQuotesAuthors);
            if (res is OkResult)
                Console.WriteLine(res.ToString());
            else
                Assert.Fail();
        }
    }
}