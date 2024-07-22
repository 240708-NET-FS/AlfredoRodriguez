namespace Program.ApplicationController;

using Program.Utils;
using Program.Service;
using Program.Model;

// Here we define method handlers for the HOME contex commands.
public partial class Controller
{
   
    // Attempts to register a new user.
    [Command(name:"HOME", description:
    @"[C]home
    [E]The homescreen of the HOME context.")]
    public int HomeHome(String[] args)
    {
        // Validate input.
        int validationCheck = ValidateInput("HOME", [], args);
        if(validationCheck != 1) return validationCheck;

        // Set context to HOME.
        Session.GetInstance().CommandContext = Command.CommandContext.HOME;

        // Get the user if any.
        String? user = Session.GetInstance().User?.Name;

        // If there is no user, print welcome screen.
        if(user is null) return Welcome();

        // Print HOME screen
        ConsoleScreen.UpdateScreenContent
        ([
            $"Welcome {user}.",
            "",
            "Type [ help ] for a list of commands."
        ]);

        return 1;
    }

    // Attempts to register a new user.
    [Command(name:"REGISTER", description:
    @"[C]register [username] [password]
    [E]Attempts to register a new user.")]
    public int Register(String[] args)
    {
        // Validate input.
        int validationCheck = ValidateInput("REGISTER", ["username", "password"], args);
        if(validationCheck != 1) return validationCheck;


        String name = args[0];
        String password = args[1];

        // Attempt to register the new user.
        UserService.RegisterUser(name, password);

        return 1;
    }

    // Enter the NOTES command context.
    [Command(name:"NOTES", description:
    @"[C]notes
    [E]Takes you to the NOTES context. Once there type help to explore its commands.")]
    public int Notes(String[] args)
    {
        // Validate input.
        int validationCheck = ValidateInput("NOTES", [], args);
        if(validationCheck != 1) return validationCheck;

        // Enter the NOTES context.
        return NotesNotes([]);
    }

    // Deletes the user's account.
    [Command(name:"DELETEME", description:
    @"[C]deleteme
    [E]Deletes the account you are logged at along all its data.")]
    public int DeleteAccount(String[] args)
    {
        // Validate input.
        int validationCheck = ValidateInput("DELETEME", [], args);
        if(validationCheck != 1) return validationCheck;

        // Attempt to delete the account.
        UserService.DeleteAccount();

        return 1;
    }

    // Attempts to login a user.
    [Command(name:"LOGIN", description:
    @"[C]login [username] [password]
    [E]Logs you in.")]
    public int Login(String[] args)
    {
        // Validate input.
        int validationCheck = ValidateInput("LOGIN", ["username", "password"], args);
        if(validationCheck != 1) return validationCheck;

        String name = args[0];
        String password = args[1];

        // Login user.
        UserService.LoginUser(name, password);
        
        return 1;
    }
}