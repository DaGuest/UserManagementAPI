using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVaultWebApp.Controllers;
using SafeVaultWebApp.Data;
using NUnit.Framework.Legacy;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SafeVaultWebApp.Tests
{
    [TestFixture]
    public class HomeControllerSqlInjectionTests
    {
        [Test]
        public void Submit_ShouldRejectSqlInjectionAttempt()
        {
            // Arrange: Use in-memory database
            var options = new DbContextOptionsBuilder<SafeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: "SqlInjectionTestDb")
                .Options;
            var controller = new HomeController();

            // Simulate SQL injection input
            string maliciousUsername = "admin'; DROP TABLE Users; --";
            string email = "attacker@example.com";
            string password = "strong123456!";

            // Act
            var result = controller.Register(maliciousUsername, email, password);

            // Assert: Should return BadRequest
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value.ToString(), Does.Contain("Username"));
        }
    }
}
