using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AzureFunctionDemo;
using AzureFunctionDemo.Dal.Interfaces;
using AzureFunctionDemo.Dal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace UnitTests
{
    [TestClass]
    public class UserProfileTests
    {
        [TestMethod]
        public async Task Run_EmptyPayload_InternalServerErrorResponse()
        {
            // Arrange
            var service = Substitute.For<IUserProfileService>();
            var logger = Substitute.For<ILogger>();
            var profiles = new PostUserProfile(service);
            var request = Substitute.For<HttpRequest>();
            var expectedMessage = "\"Value cannot be null. (Parameter 'stream')\"";

            // Act
            var actual = await profiles.Run(request, logger);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(ObjectResult));
            Assert.AreEqual(expectedMessage, ((ObjectResult)actual).Value);
        }

        [DataTestMethod]
        [DataRow("InputFiles\\CorrectRequestBody.json")]
        public async Task Run_CorrectPayload_UpsertItem(string requestBodyPath)
        {
            // Arrange
            var json = await FileReader.ReadFromFileAsync(requestBodyPath);

            var service = Substitute.For<IUserProfileService>();
            var logger = Substitute.For<ILogger>();
            var profiles = new PostUserProfile(service);
            var request = Substitute.For<HttpRequest>();

            var stream = new MemoryStream(Encoding.ASCII.GetBytes(json));
            request.Body.Returns(stream);

            // Act
            var actual = await profiles.Run(request, logger);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
        }

        [DataTestMethod]
        [DataRow("InputFiles\\InvalidModelRequest.json")]
        public async Task Run_IncorrectPayloadFormat_BadRequestObjectResult(string requestBodyPath)
        {
            // Arrange
            var json = await FileReader.ReadFromFileAsync(requestBodyPath);

            var service = Substitute.For<IUserProfileService>();
            var logger = Substitute.For<ILogger>();
            var profiles = new PostUserProfile(service);
            var request = Substitute.For<HttpRequest>();

            var stream = new MemoryStream(Encoding.ASCII.GetBytes(json));
            request.Body.Returns(stream);

            // Act
            var actual = await profiles.Run(request, logger);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpsertAsync_EntityExist_ExecuteUpdateMethod()
        {
            // Arrange
            var entity = new UserProfile()
            {
                GlobalId = "p1"
            };

            var model = new UserProfile()
            {
                GlobalId = "p1"
            };
            var models = new List<UserProfile>() { model };

            var service = Substitute.For<IUserProfileService>();

            service.GetAsync(model.GlobalId).Returns(Task.FromResult(entity));

            var profiles = new PostUserProfile(service);

            // Act
            await profiles.UpsertAsync(models);

            // Assert
            await service.Received().GetAsync(model.GlobalId);
            await service.Received().UpdateAsync(entity, model);
        }

        [TestMethod]
        public async Task UpsertAsync_EntityDoesNotExist_ExecuteInsertMethod()
        {
            // Arrange
            var model = new UserProfile()
            {
                GlobalId = "p1"
            };
            var models = new List<UserProfile>() { model };

            var service = Substitute.For<IUserProfileService>();

            service.GetAsync(model.GlobalId).Returns(Task.FromResult((UserProfile)null));

            var profiles = new PostUserProfile(service);

            // Act
            await profiles.UpsertAsync(models);

            // Assert
            await service.Received().GetAsync(model.GlobalId);
            await service.Received().InsertAsync(model);
        }
    }
}