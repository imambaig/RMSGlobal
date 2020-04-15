using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Identity.Persistence;
using System;


namespace Identity.Application.UnitTests.Common
{



    public class CommandTestBase : IDisposable
    {
        public CommandTestBase()
        {
            Context = ApplicationDbContextFactory.Create();
        }

        public DataContext Context { get; }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }

    public class FakeUserManager : UserManager<AppUser>
    {
        //IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators,
        //IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger
        public FakeUserManager()
            : base(new Mock<IUserStore<AppUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<AppUser>>().Object,
                  new IUserValidator<AppUser>[0],
                  new IPasswordValidator<AppUser>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<AppUser>>>().Object)//,                  new Mock<IHttpContextAccessor>().Object)
        {
            
        }

        public override Task<AppUser> FindByEmailAsync(string email)
        {
            return Task.FromResult(new AppUser { Email = email });
        }

        public override Task<bool> IsEmailConfirmedAsync(AppUser user)
        {
            return Task.FromResult(user.Email == "test@test.com");
        }

        public override Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            return Task.FromResult("---------------");
        }
        public override Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
