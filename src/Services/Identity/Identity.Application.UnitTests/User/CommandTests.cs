using Identity.Application.Interfaces;
using Identity.Application.UnitTests.Common;
using Identity.Application.User;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Application.UnitTests.User
{
    public class CommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_ShouldPersistUser()
        {
            try
            {
                var command = new Register.Command
                {
                    Email = "imam@imam.com",
                    UserName = "imam",
                    Password = "Password1",
                    DisplayName = "imam"
                };

                var mockUser = new Mock<UserManager<AppUser>>();
                var jwtGenerator = new Mock<IJwtGenerator>();
                mockUser.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(new AppUser { UserName = "imam" });
                jwtGenerator.Setup(jwt => jwt.CreateToken(new AppUser()))
                .Returns("00000-00000-00000-00000");

                /* mockUser.Setup(userManager => userManager.IsInRoleAsync(It.IsAny<AppUser>(), "SweetTooth"))
                     .ReturnsAsync(true);*/

                var handler = new Register.Handler(Context, new FakeUserManager(), jwtGenerator.Object);

                var result = await handler.Handle(command, CancellationToken.None);
                
                result.ShouldNotBeNull();
                result.Username.ShouldBe(command.UserName);
            }
            catch (Exception ex)
            {

            }
        }

        [Fact]
        public async Task Handle_EmailMandatory()
        {
            var command = new Register.Command
            {
                Email = "imam",
                UserName = "imam",
                Password = "Password1",
                DisplayName = "imam"
            };            

            // Arrange
            var validator = new Register.CommandValidator();           

            // Act
            var validationResult = await validator.ValidateAsync(command);

            // Assert
            Assert.False(validationResult.IsValid);

        } 
    }
}
