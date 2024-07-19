namespace Program.ApplicationController;

using Program.Utils;
using Program.Service;
using Program.Model;

// Here we define the command's method handlers.
// The method handlers' job is to properly validate the input specific for the command
// and call the appropiate service action on them.
// It also takes care or returning the appropiate exit code for the command.

// This class is divided into two files.
// This first file defined method handlers for commands
public partial class Controller
{

    private Screen ConsoleScreen = null!;
    private UserService UserService = null!;

    public Controller()
    {
        ConsoleScreen = Screen.GetInstance();
        UserService = new UserService();
    }

    [Command(name:"REGISTER", description:
    @"[C]register [username] [password]
    [E]Attempts to register a new user.")]
    public int Register(String[] args)
    {
        // Check that args count is exactly two.
        if(args.Length != 2) return Error([$"The [ register ] command requires TWO arguments, not {args.Length}."]);

        // Give those arguments a name for readability.
        String name = args[0];
        String password = args[1];

        // validate arguments.
        

        // Attempt to register the new user.
        UserService.RegisterUser(name, password);

        return 1;
    }


    [Command(name:"DELETEME", description:
    @"[C]deleteme
    [E]Deletes the account you are logged at along all its data.")]
    public int DeleteAccount(String[] args)
    {
        if(args.Length != 0) return Error(["The [ deleteme ] command doesnt take any additional arguments."]);

        UserService.DeleteAccount();

        return 1;
    }

    [Command(name:"LOGIN", description:
    @"[C]login [username] [password]
    [E]Logs you in.")]
    public int Login(String[] args)
    {
        // Check that args count is exactly two.
        if(args.Length != 2) return Error([$"The [ login ] command requires TWO arguments, not {args.Length}."]);

        // Give those arguments a name for readability.
        String name = args[0];
        String password = args[1];

        UserService.LoginUser(name, password);
        
        return 1;
    }

    [Command(name:"LOGOUT", description:
    @"[C]logout
    [E]Logs you out.")]
    public int Logout(String[] args)
    {
        // Check that args count is exactly 0.
        if(args.Length != 0) return Error([$"The [ logout ] command doesn't take any extra arguments."]);

        UserService.LogoutUser();
        
        return 1;
    }

    // Displays an error on the console
    [Command(name:"ERROR", description:
    @"[C]error
    [E]Thrown when an error occurs. You can also call it and type something if you are bored, it will throw that message back.")]
    public int Error(String[] msg)
    {
        // We get all the error strings from the message and combine then into one.
        String errorMessage = "";
        foreach (String s in msg)
        {
            errorMessage += s + " ";
        }

        if(errorMessage.Length == 0)
        {
            errorMessage = "Error. Type [ Help ] for a list of commands.";
        }

        // Print the error

        ConsoleScreen.UpdateScreenContent([$"> {errorMessage}"], [ConsoleColor.Red], false);
        ConsoleScreen.PrintScreen(Screen.InputState.ALLOWED);
        return 1;
    }

    [Command(name:"HELP", description:
    @"[C]help
    [E]Prints out all availible commands.")]
    public int Help(String[] args)
    {
        ConsoleScreen.PrintCommands();
        return 1;
    }

    [Command(name:"EXIT", description:
    @"[C]exit
    [E]Exits the application.")]
    public int Exit(String[] args)
    {
        return 0;
    }

    public int Welcome()
    {
        ConsoleScreen.UpdateScreenContent([
            "Welcome.",
            "",
            "",
            "Login with -> login [username] [password]",
            "Register with -> register [username] [password]"
            ]);
        return 1;
    }
}