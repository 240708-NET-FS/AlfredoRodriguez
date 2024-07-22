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
            Screen.UpdateScreenContent(["You must be logged in on the account you whish to delete."]);
            return;
        }

        User? user = UserDAO.GetUserByName(userName);

        // This sould never happen.
        if(user is null)
        {
            Screen.UpdateScreenContent(["Something went wrong. Please log in again."]);
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
        Screen.UpdateScreenContent(["Deleting..."], null, false);
        Screen.PrintScreen(Screen.InputState.FORBIDDEN);

        // Remove user.
        UserDAO.RemoveUser(user);
        
        // Locally logs the user out.
        Session.GetInstance().User = null!;

        Screen.UpdateScreenContent(["Account deleted."]);
    }

    public void RegisterUser(string name, string password)
    {
        // Check that we are not logged in already.
        if(Session.GetInstance().User != null)
        {
            Screen.UpdateScreenContent(["You cannot try to register while logged in."]);
            return;
        }
        if(name.Length < 3)
        {
            Screen.UpdateScreenContent(["The name must be at least 3 characters long."]);
            return;
        }
        if(password.Length < 5)
        {
            Screen.UpdateScreenContent(["The password must be at least 5 characters long."]);
            return;
        }

        // Update user that we will attempt to register now.
        Screen.UpdateScreenContent(["Registering..."], null, false);
        Screen.PrintScreen(Screen.InputState.FORBIDDEN);

        // Attempt to register
        User? user = UserDAO.AddUser(new User{Name = name, Password = password});

        // If we failed to register, it means username was already taken. (do Exception in the
        // future)
        if(user is null)
        {
            Screen.UpdateScreenContent(["Username already taken."]);
            return;
        }
        
        LoginUser(user.Name, user.Password);

        // Inform the user that we are registered and logged in.
        Screen.UpdateScreenContent(["Account created and logged in."]);
    }

    public void LoginUser(String name, String password)
    {
        // Check that we are not logged in already.
        if(Session.GetInstance().User != null)
        {
            Screen.UpdateScreenContent(["You are already logged in with an account."]);
            return;
        }

        // Inform user that we are attempting to login.
        Screen.UpdateScreenContent(["Logging you in..."], null, false);
        Screen.PrintScreen(Screen.InputState.FORBIDDEN);


        User? user = UserDAO.GetUserByName(name);

        if(user is null)
        {
            Screen.UpdateScreenContent(["Incorrect user."]);
            return;
        }

        if(!user.Password.Contains(password))
        {
            Screen.UpdateScreenContent(["Incorrect password."]);
            return;
        }

        // Record that the user is logged in.
        Session.GetInstance().User = user;

        Screen.UpdateScreenContent(["Logged in."]);
    }


    public void LogoutUser()
    {
        // Check that we are not logged in already.
        if(Session.GetInstance().User == null)
        {
            Screen.UpdateScreenContent(["You must be logged in in order to log out."]);
            return;
        }

        // This effectively logs the user out.
        Session.GetInstance().User = null!;

        Screen.UpdateScreenContent(["Bye."]);

    }
}