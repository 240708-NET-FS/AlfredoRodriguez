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
        String infoLine = $"{(expected.Length > provided.Length ? "Insuficient" : "Too many")} arguments provided for the {command} command:";
        String errorLine = "";
        String[] messageLines;

        // If too few arguments were provided.
        if(expected.Length > provided.Length)
        {
            for(int i = 0; i < expected.Length; i++)
            {
                String currProvided = provided.Length > i ? provided[i] : "???";
                errorLine += $"[{expected[i].ToUpper()}={currProvided}] ";
            }

            messageLines = ["Please, solve the -???- values."];
        }
        // If too many arguments were provided.
        else
        {
            errorLine += $"Correct usage: {command.ToLower()} ";
            for(int i = 0; i < expected.Length; i++)
            {
                errorLine += $"[{expected[i]}] ";
            }

            String actual = $"What you typed: {command.ToLower()} ";

            for(int i = 0; i < provided.Length; i++)
            {
                actual += $"{provided[i]} ";
            }

            messageLines = [actual, "Please remove any extra commands and try again."];
        }

        String[] fullMessage = [infoLine, errorLine];
        fullMessage = fullMessage.Concat(messageLines).ToArray<String>();

        //Error(fullMessage);
        Screen.ErrorMesage = $"Incorrect use of the [{command}] command. Type [help] for more information.";
        
        return false;
    }

    public int PrintError(String msg)
    {
        //ConsoleScreen.PrintErrorLine(msg);
        Screen.ErrorMesage = msg;
        return -1;
    }
}