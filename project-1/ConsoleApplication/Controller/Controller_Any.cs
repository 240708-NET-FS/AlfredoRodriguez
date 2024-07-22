namespace Program.ApplicationController;

using Program.Utils;


// Contains commands that can be accessed trough any command context.
// Global connext commands can set the command context at will.
public partial class Controller
{

    // Attempts to register a new user.
    [Command(context:Command.CommandContext.ANY, name:"HOME", description:
    @"[C]home
    [E]Takes you to the home context.")]
    public int HomeHome(String[] args)
    {
        // Validate input.
        if(!ValidateInput("HOME", [], args)) return -1;

        // Set context to HOME.
        Session.GetInstance().CommandContext = Command.CommandContext.HOME;

        // Get the user if any.
        String? user = Session.GetInstance().User?.Name;

        // If there is no user, print welcome screen.
        if(user is null) return Welcome();

        // Print HOME screen
        ConsoleScreen.UpdateScreenContent
        ([
            $"Welcome, {user}.",
            "",
            "Type [ help ] for a list of commands."
        ], [ConsoleColor.White, ConsoleColor.DarkGray]);

        return 1;
    }

    // Move to the next page of the content, if any.
    [Command(context:Command.CommandContext.ANY, name:">", description:
    @"[C]> <
    [E]Call those two to navigate pages on the console when availible.")]
    public int Next(String[] args)
    {
        ConsoleScreen.ChangePage(1);
        return 1;
    }

    // Move to the previous page of the content, if any.
    [Command(context:Command.CommandContext.ANY, name:"<")]
    public int Previous(String[] args)
    {
        ConsoleScreen.ChangePage(-1);
        return 1;
    }

    // Logs the user out.
    // This is a HOME contex's entry command.
    [Command(context:Command.CommandContext.ANY, name:"LOGOUT", description:
    @"[C]logout
    [E]Logs you out.")]
    public int Logout(String[] args)
    {
        // Validate input.
        if(!ValidateInput("LOGOUT", [], args)) return -1;

        // Log user out.
        UserService.LogoutUser();

        HomeHome([]);
        return 1;
    }

    // Prints a message to the console in error format.
    [Command(context:Command.CommandContext.ANY, name:"ERROR", description:
    @"[C]error
    [E]Thrown when an error occurs. You can also call it and type something if you are bored, it will throw that message back.")]
    public int Error(String[] msg)
    {
        String[] err = ["Error"];
        err = err.Concat(msg).ToArray<String>();

        // Print the error
        ConsoleScreen.UpdateScreenContent(err, [ConsoleColor.Red], false);
        ConsoleScreen.PrintScreen(Screen.InputState.ALLOWED);

        return -1;
    }

    // Prints out all commands for the current command context.
    [Command(context:Command.CommandContext.ANY, name:"HELP", description:
    @"[C]help
    [E]Prints out all availible commands.")]
    public int Help(String[] args)
    {
        // Validate input.
        if(!ValidateInput("HELP", [], args)) return -1;

        // Print commands.
        ConsoleScreen.PrintCommands();
        return 1;
    }

    // Exits the application.
    [Command(context:Command.CommandContext.ANY, name:"EXIT", description:
    @"[C]exit
    [E]Exits the application.")]
    public int Exit(String[] args)
    {
        // Validate input.
        if(!ValidateInput("EXIT", [], args)) return -1;

        // Signal to the program loop that we want to exit the application.
        return 0;
    }

    // Prints a Welcome screen.
    // This command is not availible to the user.
    // This is a HOME contex's entry command.
    public int Welcome()
    {
        // Set command context to HOME.
        Session.GetInstance().CommandContext = Command.CommandContext.HOME;

        // Print Welcome screen.
        ConsoleScreen.UpdateScreenContent
        ([
            "Welcome to CloudNotes.",
            "",
            "",
            "Login: login [username] [password]",
            "Register: register [username] [password]",
            "More: help",
        ], [ConsoleColor.White, ConsoleColor.DarkGray]);
        
        return 1;
    }

}