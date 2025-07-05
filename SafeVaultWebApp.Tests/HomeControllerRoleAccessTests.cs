using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using SafeVaultWebApp.Controllers;
using SafeVaultWebApp.Data;
using SafeVaultWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SafeVaultWebApp.Tests
{
    [TestFixture]
    public class HomeControllerRoleAccessTests
    {
        private HomeController GetControllerWithRole(string role)
        {
            var userStore = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStore.Object, null, null, null, null, null, null, null, null);
            var dbContextOptions = new DbContextOptionsBuilder<SafeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: $"RoleTestDb_{role}").Options;
            var dbContext = new SafeVaultDbContext(dbContextOptions);
            var tokenService = new Mock<TokenService>(null);
            var controller = new HomeController(dbContext, userManager, tokenService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, role)
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }

        [Test]
        public void AdminPanel_ShouldAllow_AdminRole()
        {
            var controller = GetControllerWithRole("Admin");
            var result = controller.AdminPanel();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void AdminPanel_ShouldDeny_UserRole()
        {
            var controller = GetControllerWithRole("User");
            Assert.Throws<Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext>(() => controller.AdminPanel());
        }

        [Test]
        public void UserDashboard_ShouldAllow_UserRole()
        {
            var controller = GetControllerWithRole("User");
            var result = controller.UserDashboard();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void GuestInfo_ShouldAllow_GuestRole()
        {
            var controller = GetControllerWithRole("Guest");
            var result = controller.GuestInfo();
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
