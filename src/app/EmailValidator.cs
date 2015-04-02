using System;
using System.Linq;
using System.Text.RegularExpressions;

using Procent.DependencyInjection.app;

using ServiceStack.OrmLite;

public interface IEmailValidator
{
    void AssertEmailIsValid(string email);
}

public class EmailValidator : IEmailValidator
{
    private readonly ConnectionProvider _connectionProvider;

    public EmailValidator(ConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public const string EMAIL_REGEX = @"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}";

    public void AssertEmailIsValid(string email)
    {
        // check if email is valid
        if (Regex.IsMatch(email, EMAIL_REGEX) == false)
        {
            throw new ArgumentException("Invalid email address");
        }

        // check if email is not taken
        if (_connectionProvider.GetConnection().Select<User>(u => u.Email == email).Any())
        {
            throw new InvalidOperationException("Email already taken");
        }
    }
}