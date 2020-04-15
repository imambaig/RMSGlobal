

using Identity.API;
using Identity.Application.User;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Indentity.API.IntegrationTests.Controllers
{
    public class UserTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UserTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenInvalidEmailRegisterCommand_ReturnsBadRequest()
        {
            var client =  _factory.GetAnonymousClient();

            var command = new Register.Command()
            {
                Email="imam@imam.com",
                UserName="imam",
                Password="Password1",
                DisplayName="imam"
               
            };
            client.BaseAddress = new System.Uri("http://localhost:5001");
            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/user/register", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

    }
}
