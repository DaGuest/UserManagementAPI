using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVaultWebApp.Controllers;
using SafeVaultWebApp.Data;
using SafeVaultWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using System.Linq;

namespace SafeVaultWebApp.Tests
{
    [TestFixture]
    public class HomeControllerAuthTests
    {
        [Test]
        public async Task Register_ShouldSucceed_WithValidInput()
        {
            // Arrange
            var userStore = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStore.Object, null, null, null, null, null, null, null, null);
            var dbContextOptions = new DbContextOptionsBuilder<SafeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: "RegisterTestDb").Options;
            var dbContext = new SafeVaultDbContext(dbContextOptions);
            var tokenService = new Mock<TokenService>(null);
            var controller = new HomeController(dbContext, userManager, tokenService.Object);

            // Act
            var result = await controller.Register("testuser", "test@example.com", "StrongPass123!");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Register_ShouldFail_WithInvalidUsername()
        {
            // Arrange
            var userStore = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStore.Object, null, null, null, null, null, null, null, null);
            var dbContextOptions = new DbContextOptionsBuilder<SafeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: "RegisterFailTestDb").Options;
            var dbContext = new SafeVaultDbContext(dbContextOptions);
            var tokenService = new Mock<TokenService>(null);
            var controller = new HomeController(dbContext, userManager, tokenService.Object);

            // Act
            var result = controller.Register("bad!user", "test@example.com", "StrongPass123!").Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Login_ShouldFail_WithInvalidCredentials()
        {
            // Arrange
            var userStore = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStore.Object, null, null, null, null, null, null, null, null);
            var dbContextOptions = new DbContextOptionsBuilder<SafeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: "LoginFailTestDb").Options;
            var dbContext = new SafeVaultDbContext(dbContextOptions);
            var tokenService = new Mock<TokenService>(null);
            var controller = new HomeController(dbContext, userManager, tokenService.Object);

            // Act
            var result = controller.Login("notfound", "wrongpass");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
