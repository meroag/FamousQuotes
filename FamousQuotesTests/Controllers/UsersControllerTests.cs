using Microsoft.VisualStudio.TestTools.UnitTesting;
using FamousQuotes.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FamousQuotes.Data;
using FamousQuotes.Models;
using FamousQuotes.Models.Helpers;
using FamousQuotesTests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuotes.Controllers.Tests
{
    [TestClass()]
    public class UsersControllerTests
    {
        private readonly UsersController controller;
        public UsersControllerTests()
        {
            controller = new UsersController(TestDbContext.db);
        }

        [TestMethod()]
        public async Task GetTest()
        {
            var list = await controller.Get();
            foreach (Users users in list)
            {
                Console.WriteLine(users.DisplayName);
            }
        }

        [TestMethod()]
        public async Task GetWithIdTest()
        {
            var user = await controller.Get(2);
            Console.WriteLine(user.DisplayName);
        }

        [TestMethod()]
        public async Task PostTest()
        {
            var model = new LoginUserModel()
            {
                DisplayName = "Merab ",
                Password = "TestPassword",
                User = "meroag@gmail.com"
            };
            var res = await controller.Post(model);
            if(!(res is OkObjectResult))
                Assert.Fail();
            else
            {
                var obj = res as OkObjectResult;
                Console.WriteLine("Created with id:" + obj);
            }
        }

        [TestMethod()]
        public async Task PutTest()
        {
            var model = (await controller.Get()).FirstOrDefault();
            if (model == null)
            {
                await PostTest();
                model = (await controller.Get()).FirstOrDefault();
            }

            model.DisplayName = "Merab Aghniashvili";
            var res = await controller.Put(model.IdUsers,new LoginUserModel()
            {
                DisplayName = model.DisplayName,
                Password = model.PasswordHash,
                User = model.Email
            });
            if(!(res is OkResult))
                Assert.Fail();
        }

        [TestMethod()]
        public async Task DeleteTest()
        {
            var model = (await controller.Get()).FirstOrDefault();
            if (model == null)
            {
                await PostTest();
                model = (await controller.Get()).FirstOrDefault();
            }
            var res = await controller.Delete(model.IdUsers);
            if(!(res is OkResult))
                Assert.Fail();
        }
    }
}