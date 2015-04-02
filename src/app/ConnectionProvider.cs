using System;

using ServiceStack.OrmLite;

namespace Procent.DependencyInjection.app
{
    public class ConnectionProvider
    {
        private readonly Lazy<OrmLiteConnection> _cachedConnection;

        public ConnectionProvider(string connectionString)
        {
            _cachedConnection = new Lazy<OrmLiteConnection>(() =>
            {
                OrmLiteConfig.DialectProvider = SqliteDialect.Provider;
                var dbFactory = new OrmLiteConnectionFactory(connectionString, SqliteDialect.Provider);
                var db = new OrmLiteConnection(dbFactory);
                db.Open();

                if (!db.TableExists<User>())
                {
                    db.CreateTable<User>();
                }

                return db;
            });
        }

        public OrmLiteConnection GetConnection()
        {
            return _cachedConnection.Value;
        }
    }
}