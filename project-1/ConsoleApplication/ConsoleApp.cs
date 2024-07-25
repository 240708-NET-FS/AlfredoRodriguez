namespace Program;

using System.Reflection;
using Program.ApplicationController;
using Program.Utils;

// Initializes the program and starts the execution loop.
public class ConsoleApp
{
    private readonly Controller _appController = null!;
    private readonly MethodInfo[] _controllerMethods = null!;

    public ConsoleApp()
    {
        _appController = new Controller();
        _controllerMethods = typeof(Controller).GetMethods();
    }

    // Contains the application loop.
    public void Run()
    {
        // Holds the command exit code.
        int commandContext = 1;

        // Initializes application.
        Init();

        // Application loop.
        while(commandContext != 0)
        {
            // Holds the command.
            String? command = null;
            // Holds the arguments for the command.
            String[]? args = null;

            // Get and validate input syntax.
            ParseInput(Console.ReadLine(), ref command, ref args);

            // Execute proper method handler for the command.
            commandContext = ExecuteMethodHandler(command, args);

            // Prints the screen again in order to update the command context UI.
            Screen.GetInstance().inputState = Screen.InputState.ALLOWED;
            Screen.GetInstance().PrintScreen();
        }

        // De-initializes application.
        DeInit();
    }

    // Any initialization tasks go here.
    private void Init()
    {
        _appController.Welcome();
        Screen.GetInstance().PrintScreen();
    }

    // Any de-initialization tasks go here.
    private void DeInit()
    {
        Console.Clear();
        Console.ResetColor();
        Console.CursorVisible = true;
    }

    // Retrieves and validates user input.
    private void ParseInput(String? input, ref String? outCommand, ref String[]? outArgs)
    {
        // If no input, return;
        if(input is null || input.Length == 0) return;

        // Get words.
        String[] words = input!.Split(' ');
        List<String> wordsList = new List<String>();

        // This gets rid of empty string in case someone inputs a command followed by a space or too many spaces.
        for(int i = 0; i < words.Length; i++)
        {
            words[i] = words[i].Trim();

            if(!words[i].Equals("") || words.Equals("\0"))
                wordsList.Add(words[i]);
        }

        words = wordsList.ToArray<String>();

        // If we end up with no wods after trimming and filtering the blank stings and new lines out, return.
        if(words.Length == 0) return;

        // Record first word as the command.
        outCommand = words[0];

        // Allocate space for the args.
        outArgs = new string[words.Length - 1];

        // Populate args.
        Array.Copy(words, 1, outArgs, 0, outArgs.Length);
    }

    // Given a command and some arguments. Find the controller method responsible for it and execute it.
    private int ExecuteMethodHandler(String? command, String[]? args)
    {
        // If no command was provided, do nothing.
        if(command is null) return 1;

        // For each method in the Controller class.
        foreach (var m in _controllerMethods)
        {
            // Try to get the Command Attribute.
            var commandAtt = m.GetCustomAttribute<Command>();

            // If none, then skip it.
            if(commandAtt is null) continue;
            else
            {
                // If this command atribute is not of type ANY or of the current command context type accepted, continue.
                if(commandAtt.Context != Command.CommandContext.ANY && commandAtt.Context != Session.GetInstance().CommandContext)
                {
                    continue;
                }

                // If the method's command equals the user requested command:
                if(commandAtt.Name.ToUpper().Equals(command.ToUpper()))
                {
                    // Prepare method arguments.
                    Object[] methodArgs = {args!};

                    // Invoke the method using the controller instance as the instance to invoke it on.
                    return (int) m.Invoke(_appController, methodArgs)!;
                }
            }
        }

        // If command not found, execute the error command.
        return _appController.CommandNotFound("Command not found. Type [ help ] for a list of commands");
    }
}