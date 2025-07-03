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
            var controller = new HomeController();

            // Simulate XSS input
            string maliciousUsername = "<script>alert('xss')</script>";
            string email = "user@example.com";
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
