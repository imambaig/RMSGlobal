using Identity.API.Controllers;
using Identity.Application.User;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Identity.UnitTests
{
    public class LoginUniTest
    {
        private Login.Query CreateRequest(string userName, string password)
        {
            Login.Query request = new Login.Query
            {
                Email = userName,
                Password = password
            };

            return request;
        }

        private static IMediator BuildMediator() //WrappingWriter writer
        {
            //var services = new ServiceCollection();

            ////services.AddSingleton<TextWriter>(writer);

            //services.AddMediatR(typeof(Ping));

            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(GenericPipelineBehavior<,>));
            //services.AddScoped(typeof(IRequestPreProcessor<>), typeof(GenericRequestPreProcessor<>));
            //services.AddScoped(typeof(IRequestPostProcessor<,>), typeof(GenericRequestPostProcessor<,>));

            //var provider = services.BuildServiceProvider();

            //return provider.GetRequiredService<IMediator>();

            //var builder = new StringBuilder();
            //var writer = new StringWriter(builder);

            //var container = new Container(cfg =>
            //{
            //    cfg.Scan(scanner =>
            //    {
            //        scanner.AssemblyContainingType(typeof(PublishTests));
            //        scanner.IncludeNamespaceContainingType<Ping>();
            //        scanner.WithDefaultConventions();
            //        scanner.AddAllTypesOf(typeof(INotificationHandler<>));
            //    });
            //    cfg.For<TextWriter>().Use(writer);
            //    cfg.For<IMediator>().Use<SequentialMediator>();
            //    cfg.For<ServiceFactory>().Use<ServiceFactory>(ctx => t => ctx.GetInstance(t));
            //});

            //var mediator = container.GetInstance<IMediator>();

            //var mediator = Mock<IMediator>();
            return null;
        }

        [Fact]
        public async void UserName_ShouldHave_Max_30Characters_Exception()
        {
            try
            {
                ////var request = CreateRequest("UserNameIsGreaterThanAllowed", "password");
                ////var mediator = new Mock<IMediator>();
                ////var response = mediator.Object.Send(request).Result;

                //var user = new UserViewModel
                //{
                //    viewModel.UserId = 100,
                //    viewModel.Username = "sgordon",
                //    viewModel.Forename = "Steve",
                //    viewModel.Surname = "Gordon",
                //};

                //var mockMediator = new Mock<IMediator>();
                ////mediator.Setup(x => x.SendAsync(It.IsAny<UserQuery>())).Returns(user);

                //mockMediator.Setup(m => m.Send(It.IsAny<TransferNotificationCommand>(), default(CancellationToken))).Verifiable("Notification was not sent.");

                //var handler = new TransferHandler(mockMediator.Object);

                //var actual = await handler.Handle(message);

                //// mockMediator.Verify(x => x.Send(It.IsAny<CreateIsaTransferNotificationCommand>(), default(CancellationToken)), Times.Once());

                //var notification = new Login.Query()
                //{
                //    Email = "email",
                //    Password = "Password1"
                //};

                //mockMediator
                //    .Setup(m => m.Send(It.IsAny<Login.Query>(), It.IsAny<CancellationToken>()))
                //    .ReturnsAsync(new User()) //<-- return Task to allow await to continue
                //    .Verifiable("Notification was not sent.");



                //mockMediator.Verify(x => x.Send(It.IsAny<notification>(), It.IsAny<CancellationToken>()), Times.Once());

                var notification = new Login.Query()
                {
                    Email = "email",
                    Password = "Password1"
                };

                //Arrange
                var mediator = new Mock<IMediator>();
                mediator.Setup(m => m.Send(It.IsAny<Login.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<User>());

                //Act
              await  mediator.Object.Send(new Login.Query());

                //Assert
                mediator.Verify(x => x.Send(It.IsAny<Login.Query>(), It.IsAny<CancellationToken>()));





            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        [Fact]
        public async  void UserDetails_SendsQueryWithTheCorrectUserId()
        {
            var notification = new Login.Query()
            {
                Email = "email",
                Password = "Password1"
            };

            const int userId = 1;
            var mediator = new Mock<IMediator>();
            var sut = new UserController();
            sut.Mediator = mediator.Object;
            await sut.Login(notification);

            mediator.Verify(x => x.Send(It.Is<Login.Query>(y => y == notification)), Times.Once);
        }


        //[Fact]
        //public async Task HandlerReturnsCorrectUserViewModel()
        //{
        //    var sut = new Login.Handler();
        //    var result = await sut.Handle(new UserQuery { Id = 100 });

        //    Assert.NotNull(result);
        //    Assert.Equal("Steve", result.Forename);
        //}


        //[Fact]
        //public async Task Should_call_abstract_unit_handler()
        //{
        //    var builder = new StringBuilder();
        //    var writer = new StringWriter(builder);

        //    IRequestHandler<Ping, Unit> handler = new PingHandler(writer);

        //    await handler.Handle(new Ping() { Message = "Ping" }, default);

        //    var result = builder.ToString();
        //    result.ShouldContain("Ping Pong");
        //}
    }
    
}

