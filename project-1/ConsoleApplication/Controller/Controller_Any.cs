namespace Program.ApplicationController;

using Program.Utils;


// This second part of the class defines.
// method handlers made for pagination commands.
public partial class Controller
{
    [Command(context:Command.CommandContext.ANY, name:">", description:
    @"[C]> <
    [E]Call those two to navigate pages on the console when availible.")]
    public int Next(String[] args)
    {
        ConsoleScreen.ChangePage(1);
        return 1;
    }

    [Command(context:Command.CommandContext.ANY, name:"<")]
    public int Previous(String[] args)
    {
        ConsoleScreen.ChangePage(-1);
        return 1;
    }

    [Command(context:Command.CommandContext.ANY, name:"LOGOUT", description:
    @"[C]logout
    [E]Logs you out.")]
    public int Logout(String[] args)
    {
        // Check that args count is exactly 0.
        if(args.Length != 0) return Error([$"The [ logout ] command doesn't take any extra arguments."]);

        UserService.LogoutUser();

        // Enter the HOME context
        Session.GetInstance().CommandContext = Command.CommandContext.HOME;
        return 1;
    }

    // Displays an error on the console
    [Command(context:Command.CommandContext.ANY, name:"ERROR", description:
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

    [Command(context:Command.CommandContext.ANY, name:"HELP", description:
    @"[C]help
    [E]Prints out all availible commands.")]
    public int Help(String[] args)
    {
        ConsoleScreen.PrintCommands();
        return 1;
    }

    [Command(context:Command.CommandContext.ANY, name:"EXIT", description:
    @"[C]exit
    [E]Exits the application.")]
    public int Exit(String[] args)
    {
        return 0;
    }


    [Command(context:Command.CommandContext.ANY, name:"HOME", description:
    @"[C]home
    [E]Takes you to the home context.")]
    public int Home(String[] args)
    {
        ConsoleScreen.UpdateScreenContent(["You are now on the HOME context."]);
        Session.GetInstance().CommandContext = Command.CommandContext.HOME;
        return 1;
    }

    public int Welcome()
    {
        Session.GetInstance().CommandContext = Command.CommandContext.HOME;

        ConsoleScreen.UpdateScreenContent
        ([
            "Welcome.",
            "",
            "",
            "Login with -> login [username] [password]",
            "Register with -> register [username] [password]"
        ]);
        
        return 0;
    }


}