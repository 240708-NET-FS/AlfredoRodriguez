namespace Program.ApplicationController;

using Program.Utils;
using Program.Data;
using Program.Model;


// This would be the place where we define all the commands
public partial class Controller
{
    private Screen ConsoleScreen = null!;
    private UserService UserService = null!;

    public Controller()
    {
        ConsoleScreen = new Screen();
        UserService = new UserService();
    }

    [Command(name:"REGISTER", description:
    @"[C]register [username] [password]
    [E]Attempts to register a new user.")]
    public int SomeMethod(String[] args)
    {
        // do input val later, for now just focus on func

        ConsoleScreen.PrintScreen("Please wait...");
        User? newUser = new User{Name = args[0], Password = args[1]};

        newUser = UserService.addUser(newUser);

        if(newUser == null)
        {
            ConsoleScreen.PrintScreen("Username taken. Try another one.");
            return 1;
        }

        ConsoleScreen.PrintScreen($"User created. User ID is: {newUser.Id}.");
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
        ConsoleScreen.PrintScreen($"> {errorMessage}", ConsoleColor.Red);
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
        ConsoleScreen.ClearScreen();
        return 0;
    }

    public int Welcome()
    {
        ConsoleScreen.PrintScreen("Welcome.\n\nLogin via: login [username] [password]\nRegister via: register [username] [password]");
        return 1;
    }
}