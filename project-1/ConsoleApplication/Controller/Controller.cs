namespace Program.ApplicationController;

using Program.Service;
using Program.Utils;

// This part of the Controller contains the fields and util methods.
public partial class Controller
{
    private readonly NoteService _noteService =null!;
    private readonly Screen _screen = null!;
    private readonly UserService _userService = null!;

    public Controller()
    {
        _noteService = new NoteService();
        _screen = Screen.GetInstance();
        _userService = new UserService();
    }

    // Validates input, takes care of informing the user if the input is incorrect.
    private bool ValidateInput(String command, String[] expected, String[]? provided)
    {
        if(provided is null) provided = [];

        if(provided.Length == expected.Length) return true;

        _screen.SetMessage($"Incorrect use of the [{command.ToUpper()}] command. Type [help] for more information.", Screen.MessageType.Error);
        
        return false;
    }

    // Executed by the application loop if the user's input fails validation at that level.
    public int CommandNotFound(String msg)
    {
        _screen.SetMessage(msg, Screen.MessageType.Error);
        return -1;
    }
}