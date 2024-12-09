using System.Collections.Concurrent;

public interface IRepository
{
    IEnumerable<User> GetUsers();
    void AddUser(User user);

    User? GetUser(Guid id);
    User? GetUser(string username);
};

public class Repository : IRepository
{
    private readonly ConcurrentBag<User> _users = new ConcurrentBag<User>();

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public User? GetUser(Guid id)
    {
        var user = _users.FirstOrDefault(x => x.Id == id);
        return user;
    }

    public User? GetUser(string username)
    {
        var user = _users.FirstOrDefault(x => x.UserName == username);
        return user;
    }

    public IEnumerable<User> GetUsers()
    {
        return _users;
    }

    

    
}