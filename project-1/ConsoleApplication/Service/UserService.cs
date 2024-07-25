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
        Screen = Screen.GetInstance();
    }

    public bool DeleteAccount(User? u)
    {
        String? userName = u?.Name;

        // Check if the user is logged in.
        if(userName == null)
        {
            Screen.SetMessage("You must be logged in on the account you whish to delete.", Screen.MessageType.Error);
            return false;
        }

        User? user = UserDAO.GetUserByName(userName);

        // This sould never happen.
        if(user is null)
        {
            Screen.SetMessage("Something went wrong. Please log in again.", Screen.MessageType.Error);
            return false;
        }

        // This loop keeps on asking for confirmation to delete the account until Y or N is pressed.
        bool wasValidInput = true;
        while(true)
        {
            Screen.UpdateScreenContent(["Do you whish to delete your account and all its contents?","This action cannot be undone.", "Y / N"]);
            Screen.SetMessage("Please confirm.", Screen.MessageType.Info);

            if(!wasValidInput) Screen.SetMessage("You must respond with Y or N.", Screen.MessageType.Error);

            Screen.PrintScreen();

            String? input = Console.ReadLine();
            // If Y, continue with deletion.
            if(input != null && input.Trim().ToLower().Equals("y")) break;
            // If N, cancel deletion.
            else if(input != null && input.Trim().ToLower().Equals("n"))
            {
                Screen.SetMessage("Great! :)", Screen.MessageType.Info);
                return false;
            }

            wasValidInput = false;
        }

        // Update UI to let user know we are currently attempting to delete the account.
        Screen.SetMessage("Deleting...", Screen.MessageType.Info, true);
        Screen.inputState = Screen.InputState.FORBIDDEN;

        // Remove user.
        UserDAO.RemoveUser(user);
        
        // Locally logs the user out.
        Session.GetInstance().User = null!;

        Screen.SetMessage("Account deleted :(", Screen.MessageType.Info);
        return true;
    }

    public void RegisterUser(string name, string password)
    {
        // Check that we are not logged in already.
        if(Session.GetInstance().User != null)
        {
            Screen.SetMessage("You cannot try to register while logged in.", Screen.MessageType.Error);
            return;
        }
        if(name.Length < 3)
        {
            Screen.SetMessage("The name must be at least 3 characters long.", Screen.MessageType.Error);
            return;
        }
        if(password.Length < 5)
        {
            Screen.SetMessage("The password must be at least 5 characters long.", Screen.MessageType.Error);
            return;
        }

        // Update user that we will attempt to register now.
        Screen.SetMessage("Registering...", Screen.MessageType.Info, true);
        Screen.inputState = Screen.InputState.FORBIDDEN;

        // Attempt to register
        User? user = UserDAO.AddUser(new User{Name = name, Password = password});

        // If we failed to register, it means username was already taken. (do Exception in the
        // future)
        if(user is null)
        {
            Screen.SetMessage("Username already taken.", Screen.MessageType.Error);
            return;
        }
        
        LoginUser(user.Name, user.Password);

        // Inform the user that we are registered and logged in.
        Screen.SetMessage("Account created and logged in.", Screen.MessageType.Info);
    }

    public bool LoginUser(String name, String password)
    {
        // Check that we are not logged in already.
        // Inform user that we are attempting to login.
        Screen.SetMessage("Logging you in...", Screen.MessageType.Info, true);
        Screen.inputState = Screen.InputState.FORBIDDEN;

        if(Session.GetInstance().User != null)
        {
            Screen.SetMessage("You are already logged in with an account.", Screen.MessageType.Error);
            return false;
        }



        User? user = UserDAO.GetUserByName(name);

        if(user is null)
        {
            Screen.SetMessage("Incorrect user.", Screen.MessageType.Error);
            return false;
        }

        if(!user.Password.Contains(password))
        {
            Screen.SetMessage("Incorrect password.", Screen.MessageType.Error);
            return false;
        }

        // Record that the user is logged in.
        Session.GetInstance().User = user;

        return true;
    }


    public void LogoutUser()
    {
        // Check that we are not logged in already.
        if(Session.GetInstance().User == null)
        {
            Screen.SetMessage("You must be logged in in order to log out.", Screen.MessageType.Error);
            return;
        }

        // This effectively logs the user out.
        Session.GetInstance().User = null!;

        Screen.SetMessage("Logged out.", Screen.MessageType.Info);
    }
}