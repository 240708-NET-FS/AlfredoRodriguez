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

    public User? AddUser(String name, String password)
    {
        return UserDAO.AddUser(new User(){Name = name, Password = password});
    }

    public void DeleteAccount()
    {
        String? userName = Session.GetInstance().User?.Name;

        // Check if the user is logged in.
        if(userName == null)
        {
            Screen.SetMessage("You must be logged in on the account you whish to delete.", Screen.MessageType.Error);
            return;
        }

        User? user = UserDAO.GetUserByName(userName);

        // This sould never happen.
        if(user is null)
        {
            Screen.SetMessage("Something went wrong. Please log in again.", Screen.MessageType.Error);
            return;
        }

        // This loop keeps on asking for confirmation to delete the account until Y or N is pressed.
        bool wasValidInput = true;
        while(true)
        {
            // Original prompt
            String[] prompt = ["Do you whish to delete your account and all its contents?","This action is not reversible", "Respond with Y / N"];

            // If the last response to the question was not a valid response (Y or N).
            if(!wasValidInput)
                // Add a red text at the top reiterating that we need etiher a Y or a N.
                Screen.UpdateScreenContent(new String[]{"You must enter Y or N."}.Concat(prompt).ToArray<String>(), [ConsoleColor.Red, ConsoleColor.White]);
            else
                // Ask for confirmation.
                Screen.UpdateScreenContent(prompt);

            Screen.PrintScreen();
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            
            // If Y, continue with deletion.
            if(keyInfo.Key == ConsoleKey.Y) break;
            // If N, cancel deletion.
            if(keyInfo.Key == ConsoleKey.N)
            {
                Screen.UpdateScreenContent(["Deletion cancelled."]);
                return;
            }

            wasValidInput = false;
        }

        // Update UI to let user know we are currently attempting to delete the account.
        //Screen.UpdateScreenContent(["Deleting..."]);
        Screen.SetMessage("Deleting...", Screen.MessageType.Info, true);

        Screen.inputState = Screen.InputState.FORBIDDEN;

        // Remove user.
        UserDAO.RemoveUser(user);
        
        // Locally logs the user out.
        Session.GetInstance().User = null!;

        Screen.SetMessage("Account deleted.", Screen.MessageType.Info);
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

        Screen.UpdateScreenContent(["Bye."]);
    }
}