namespace Program.DAO;

using Program.Data;
using Program.Model;

public class UserDAO
{
    // Attempts to add a new user to the User table. Returns added user or null if username taken.
    public User? AddUser(User u)
    {
        // Make sure that the reqwuired fields for a User record are met before even trying 
        if(u.Name == null || u.Name.Length == 0
        || u.Password == null || u.Password.Length == 0)
            throw new ArgumentException("Invalid user.");

        DatabaseContext c = Connection.Get();

            if(GetUserByName(u.Name) != null) return null;

            c.Add<User>(u);
            c.SaveChanges();

        return u;
    }

    // Removes a user. Returns removed user or null if user doesn't match.
    public User? RemoveUser(User u)
    {
        // Make sure that the reqwuired fields for a User record are met before even trying 
        if(u.Name == null || u.Name.Length == 0
        || u.Password == null || u.Password.Length == 0)
            throw new ArgumentException("Invalid user.");

        DatabaseContext c = Connection.Get();

        User? toRemove = GetUserByName(u.Name);

        // If the user name is incorrect, return null.
        if(toRemove == null) return null;

        // if the password doesnt match, return null.
        if(!toRemove.Password.Contains(u.Password)) return null;

        // Remove user.
        c.Users.Remove(toRemove);
        c.SaveChanges();

        return toRemove;
    }


    // Retrieves a user by name, null if none found.
    public User? GetUserByName(string name)
    {
        if(name is null || name.Length == 0) throw new ArgumentException("Invalid name");

        DatabaseContext c = Connection.Get();

        return c.Users.FirstOrDefault(u => u!.Name == name);
    }
}