namespace Program.ApplicationController;
using Program.Service;
using Program.Utils;

public partial class Controller
{
    private NoteService NoteService = new NoteService();
    private Screen Screen = Screen.GetInstance();
    private UserService UserService = new UserService();

    // Validates input, takes care of informing the user if the input is incorrect.
    // Returns:
    // 1 if pass
    private bool ValidateInput(String command, String[] expected, String[]? provided)
    {
        if(provided is null) provided = [];

        if(provided.Length == expected.Length) return true;

        command = command.ToUpper();

        Screen.SetMessage($"Incorrect use of the [{command}] command. Type [help] for more information.", Screen.MessageType.Error);
        
        return false;
    }

    // This command is not availible to the user, it's called by the application.
    public int CommandNotFound(String msg)
    {
        Screen.SetMessage(msg, Screen.MessageType.Error);
        return -1;
    }
}