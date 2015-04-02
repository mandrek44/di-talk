using System;

using NUnit.Framework;

using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace Procent.DependencyInjection.app
{
    public class WebServer
    {
        private readonly IEmailValidator _emailValidator;
        private readonly IUsersRepository _usersRepository;

        public WebServer(IEmailValidator emailValidator, IUsersRepository usersRepository)
        {
            _emailValidator = emailValidator;
            _usersRepository = usersRepository;
        }

        public void RegisterUser(string email)
        {
            _emailValidator.AssertEmailIsValid(email);
            _usersRepository.AddUser(User.Create(email));
        }
    }

    public class User
    {
        [AutoIncrement] 
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public static User Create(string email)
        {
            return new User() { Email = email, Name = Guid.NewGuid().ToString() };
        }
    }

    public class WebServerTests
    {
        [Test]
        public void CannotAddTheSameEmailTwice()
        {
            // given
            var connectionProvider = new ConnectionProvider(":memory:");
            var webServer = new WebServer(new EmailValidator(connectionProvider), new UsersRepository(connectionProvider));
            webServer.RegisterUser("MANDREK@GMAIL.COM");

            // then
            Assert.Throws<InvalidOperationException>(() => webServer.RegisterUser("MANDREK@GMAIL.COM"));
        }

        [Test]
        public void DoesntPermitInvalidEmail()
        {
            // given
            var connectionProvider = new ConnectionProvider(":memory:");
            var webServer = new WebServer(new EmailValidator(connectionProvider), new UsersRepository(connectionProvider));

            // then
            Assert.Throws<ArgumentException>(() => webServer.RegisterUser("MANDREK"));
        }
    }
}