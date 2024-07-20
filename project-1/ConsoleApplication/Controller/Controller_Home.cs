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

    [Command(name:"NOTE", description:
    @"[C]note
    [E]Takes you to the note context. Once there type help to explore its commands.")]
    public int Note(String[] args)
    {
        Session.GetInstance().CommandContext = Command.CommandContext.NOTE;

        ConsoleScreen.UpdateScreenContent(["Note context", "Type help to see [ note ] commands."]);

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
}