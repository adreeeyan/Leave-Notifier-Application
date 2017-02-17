using System;
using LeaveNotifierApplication.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace LeaveNotifierApplication.Api.Tests.UnitTests.Shared
{
    public class CommonMocks
    {
        public static Mock<UserManager<LeaveNotifierUser>> GetUserManagerMock()
        {
            return new Mock<UserManager<LeaveNotifierUser>>(new Mock<IUserStore<LeaveNotifierUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<LeaveNotifierUser>>().Object,
                new IUserValidator<LeaveNotifierUser>[0],
                new IPasswordValidator<LeaveNotifierUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<LeaveNotifierUser>>>().Object);
        }

        public static Mock<SignInManager<LeaveNotifierUser>> GetSignInManagerMock(Mock<UserManager<LeaveNotifierUser>> userManagerMock)
        {
            var context = new Mock<HttpContext>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);

            return new Mock<SignInManager<LeaveNotifierUser>>(userManagerMock.Object,
                contextAccessor.Object,
                new Mock<IUserClaimsPrincipalFactory<LeaveNotifierUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<LeaveNotifierUser>>>().Object);
        }
    }
}