namespace Program.ApplicationController;

using Program.Utils;
using Program.Service;
using Program.Model;

// Here we define method handlers for the HOME contex commands.
public partial class Controller
{
   


    // Attempts to register a new user.
    [Command(name:"REGISTER", description:
    @"[C]register [username] [password]
    [E]Attempts to register a new user.")]
    public int Register(String[] args)
    {
        // Validate input.
        if(!ValidateInput("REGISTER", ["username", "password"], args)) return -1;

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
        if(!ValidateInput("NOTES", [], args)) return -1;

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
        if(!ValidateInput("DELETEME", [], args)) return -1;

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
        if(!ValidateInput("LOGIN", ["username", "password"], args)) return -1;

        String name = args[0];
        String password = args[1];

        // Login user.
        if(UserService.LoginUser(name, password))
            HomeHome([]);
        
        return 1;
    }
}