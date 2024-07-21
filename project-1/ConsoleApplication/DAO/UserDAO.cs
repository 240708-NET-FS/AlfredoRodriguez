namespace Program.DAO;

using Program.Data;
using Program.Model;

public class UserDAO
{
    // Attempts to add a new user to the User table. Returns added user or null if username taken.
    public User? AddUser(User u)
    {
        DatabaseContext c = Connection.Get();

        // If user name taken, return null
        if(GetUserByName(u.Name) != null) return null;

        // Add user
        c.Add<User>(u);
        c.SaveChanges();

        return u;
    }

    // Removes a user. Returns removed user or null if user doesn't match.
    public User? RemoveUser(User u)
    {
        DatabaseContext c = Connection.Get();

        User? toRemove = GetUserByName(u.Name);

        // If the user name is incorrect, return null
        if(toRemove == null) return null;

        // if the password doesnt match, return null
        if(!toRemove.Password.Contains(u.Password)) return null;

        // Remove user.
        c.Users.Remove(toRemove);

        return toRemove;
    }


    // Retrieves a user by name
    public User? GetUserByName(string name)
    {
        DatabaseContext c = Connection.Get();
        
        return c.Users.FirstOrDefault(u => u.Name == name);
    }
}