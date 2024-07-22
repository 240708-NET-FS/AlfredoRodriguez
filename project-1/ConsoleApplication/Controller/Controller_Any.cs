namespace Program.ApplicationController;

using Program.Utils;


// Contains commands that can be accessed trough any command context.
// Global connext commands can set the command context at will.
public partial class Controller
{
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
        int validationCheck = ValidateInput("LOGOUT", [], args);
        if(validationCheck != 1) return validationCheck;

        // Log user out.
        UserService.LogoutUser();

        // Enter the HOME context.
        Session.GetInstance().CommandContext = Command.CommandContext.HOME;
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
        int validationCheck = ValidateInput("HELP", [], args);
        if(validationCheck != 1) return validationCheck;

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
        int validationCheck = ValidateInput("EXIT", [], args);
        if(validationCheck != 1) return validationCheck;

        // Signal to the program loop that we want to exit the application.
        return 0;
    }


    // Takes the user to the HOME command context.
    // This is a HOME contex's entry command.
    [Command(context:Command.CommandContext.ANY, name:"HOME", description:
    @"[C]home
    [E]Takes you to the home context.")]
    public int Home(String[] args)
    {
        // Validate input.
        int validationCheck = ValidateInput("HOME", [], args);
        if(validationCheck != 1) return validationCheck;

        // Call HOME's context entry point.
        return HomeHome(args);
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
        ]);
        
        return 0;
    }

}