using Program.Data;
using Program.Model;

public class UserService
{
    private DatabaseContext Context = null!;

    public UserService()
    {
        Context = new DatabaseContext();
    }
    ~UserService()
    {
        Context.Dispose();
    }

    public User? addUser(User u)
    {
        // Check if the username is taken.
        if(usernameExists(u.Name))
        {
            return null;
        }

        // Add user
        Context.Add<User>(u);
        Context.SaveChanges();

        return u;
    }


    private bool usernameExists(string username)
    {
        User? u = Context.Users.FirstOrDefault(u => u.Name == username);
        return u != null;
    }
}