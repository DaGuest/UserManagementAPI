using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVaultWebApp.Controllers;
using SafeVaultWebApp.Data;
using NUnit.Framework.Legacy;

namespace SafeVaultWebApp.Tests
{
    [TestFixture]
    public class HomeControllerXssInjectionTests
    {
        [Test]
        public void Submit_ShouldRejectXssInjectionAttempt()
        {
            // Arrange: Use in-memory database
            var options = new DbContextOptionsBuilder<SafeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: "XssInjectionTestDb")
                .Options;
            using var context = new SafeVaultDbContext(options);
            var controller = new HomeController(context);

            // Simulate XSS input
            string maliciousUsername = "<script>alert('xss')</script>";
            string email = "user@example.com";

            // Act
            var result = controller.Submit(maliciousUsername, email);

            // Assert: Should return BadRequest
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value.ToString(), Does.Contain("Username"));
        }
    }
}
