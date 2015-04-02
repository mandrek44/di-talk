using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Owin.Hosting;

using Owin;

using ServiceStack;

namespace Procent.DependencyInjection.app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (WebApp.Start("http://+:12345", Startup))
            {
                Console.WriteLine("Listening at port 12345");
                Console.ReadLine();
            }
        }

        private static void Startup(IAppBuilder app)
        {
            var webServer = new WebServer();

            // Logging
            app.Use(
                async (context, next) =>
                {
                    Console.WriteLine(context.Request.Method + " " + context.Request.Path + context.Request.QueryString);
                    try
                    {
                        await next();
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.ToString());
                        throw;
                    }
                });

            // Request handling
            app.Use((context, next) =>
            {
                return Task.Run(() => webServer.RegisterUser(context.Request.Body.ReadLines().First()));
            });
        }
    }
}