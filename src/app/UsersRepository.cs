using Procent.DependencyInjection.app;

using ServiceStack.OrmLite;

public interface IUsersRepository
{
    void AddUser(User user);
}

public class UsersRepository : IUsersRepository
{
    private readonly ConnectionProvider _connectionProvider;

    public UsersRepository(ConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public void AddUser(User user)
    {
        var db = _connectionProvider.GetConnection();
        db.Insert(user);
    }
}