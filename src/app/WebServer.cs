using System;
using System.Linq;
using System.Text.RegularExpressions;

using ServiceStack.OrmLite;

namespace Procent.DependencyInjection.app
{
    public class WebServer
    {
        const string EMAIL_REGEX = @"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}";

        public void RegisterUser(string email)
        {
            OrmLiteConfig.DialectProvider = SqliteDialect.Provider;
            var dbFactory = new OrmLiteConnectionFactory("local.db", SqliteDialect.Provider);
            var db = new OrmLiteConnection(dbFactory);
            db.Open();

            if (!db.TableExists<User>())
                db.CreateTable<User>();

            // check if email is valid
            if (Regex.IsMatch(email, EMAIL_REGEX) == false)
            {
                throw new ArgumentException("Invalid email address");
            }
            
            // check if email is not taken
            if (db.Select<User>(u => u.Email == email).Any())
            {
                throw new InvalidOperationException("Email already taken");
            }

            // create new user
            var newUser = new User
            {
                Email = email,
                Name = Guid.NewGuid().ToString(),
            };

            // insert user
            db.Insert(newUser);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}