namespace Program.Service;

using Program.DAO;
using Program.Model;
using Program.Utils;

// The service layer here will take care of business logic.
// It will define the flow of the command while offloading
// the work of database interaction to the DAO and screen rendering
// to the Screen.
public class UserService
{
    private UserDAO UserDAO = null!;
    private Screen Screen = null!;
    public UserService()
    {
        UserDAO = new UserDAO();
        Screen = new Screen();
    }

    public User? AddUser(String name, String password)
    {
        return UserDAO.AddUser(new User(){Name = name, Password = password});
    }

    public void DeleteAccount()
    {
        // Check if the user is logged in.
        if(Session.GetInstance().User == null)
        {
            Screen.PrintScreen("You must be logged in on the account you whish to delete.", Screen.InputState.ALLOWED, "HOME");
            return;
        }

        User? user = UserDAO.GetUserByName(Session.GetInstance().User);

        // This sould never happen.
        if(user is null)
        {
            Screen.PrintScreen("Something went wrong. Please log in again.", Screen.InputState.ALLOWED, "HOME");
            return;
        }

        // Remove user
        UserDAO.RemoveUser(user);
        
        // Locally logs the user out.
        Session.GetInstance().User = null!;

        Screen.PrintScreen("Sorry to see you go :(", Screen.InputState.ALLOWED, "HOME");
    }

    public void RegisterUser(string name, string password)
    {
        // Check that we are not logged in already.
        if(Session.GetInstance().User != null)
        {
            Screen.PrintScreen("You cannot try to register while logged in.", Screen.InputState.ALLOWED, "HOME");
            return;
        }
        if(name.Length < 3)
        {
            Screen.PrintScreen("The name must be at least 3 characters long", Screen.InputState.ALLOWED, "HOME");
            return;
        }
        if(password.Length < 5)
        {
            Screen.PrintScreen("The password must be at least 5 characters long", Screen.InputState.ALLOWED, "HOME");
            return;
        }

        // Update user that we will attempt to register now.
        Screen.PrintScreen("Registering...", Screen.InputState.FORBIDDEN, "REGISTER");

        // Attempt to register
        User? user = UserDAO.AddUser(new User{Name = name, Password = password});

        // If we failed to register, it means username was already taken. (do Exception in the
        // future)
        if(user is null)
        {
            Screen.PrintScreen("Username already taken.", Screen.InputState.ALLOWED, "HOME");
            return;
        }
        
        LoginUser(user.Name, user.Password);

        // Inform the user that we are registered and logged in.
        Screen.PrintScreen("Account created and logged in.", Screen.InputState.ALLOWED, "HOME");
    }

    public void LoginUser(String name, String password)
    {
        // Check that we are not logged in already.
        if(Session.GetInstance().User != null)
        {
            Screen.PrintScreen("You are already logged in with an account.", Screen.InputState.ALLOWED, "HOME");
            return;
        }

        // Inform user that we are attempting to login.
        Screen.PrintScreen("Logging you in...", Screen.InputState.FORBIDDEN, "LOGIN");

        User? user = UserDAO.GetUserByName(name);

        if(user is null)
        {
            Screen.PrintScreen("Incorrect user.", Screen.InputState.ALLOWED, "HOME");
            return;
        }

        if(!user.Password.Contains(password))
        {
            Screen.PrintScreen("Incorrect password.", Screen.InputState.ALLOWED, "HOME");
            return;
        }

        // Record that the user is logged in.
        Session.GetInstance().User = name;

        Screen.PrintScreen("Logged in.", Screen.InputState.ALLOWED, "HOME");
    }

    public void LogoutUser()
    {
        // Check that we are not logged in already.
        if(Session.GetInstance().User == null)
        {
            Screen.PrintScreen("You must be logged in in order to log out.", Screen.InputState.ALLOWED, "HOME");
            return;
        }

        // This effectively logs the user out.
        Session.GetInstance().User = null!;

        Screen.PrintScreen("Bye.", Screen.InputState.ALLOWED, "HOME");
    }
}