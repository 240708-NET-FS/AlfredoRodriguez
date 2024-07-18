namespace Program.ApplicationController;

using Program.Utils;


// This would be the place where we define all the commands
public partial class Controller
{
    private Screen ConsoleScreen;

    public Controller() => ConsoleScreen = new Screen();

    [Command(name:"TEST", description:
    @"[C]test arg1
    [E]This is the test Description.")]
    public int SomeMethod(String[] args)
    {
        ConsoleScreen.PrintScreen("You just executed the method related to the TEST command !");
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