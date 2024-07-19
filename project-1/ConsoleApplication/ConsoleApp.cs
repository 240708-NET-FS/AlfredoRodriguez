namespace Program;

using System.Reflection;
using Program.ApplicationController;
using Program.Utils;


// Initializes the program and starts the execution loop.
public class ConsoleApp
{
    private Controller AppController = null!;
    private MethodInfo[] ControllerMethods = null!;

    public ConsoleApp()
    {
        AppController = new Controller();
        ControllerMethods = typeof(Controller).GetMethods();
    }

    public void Run()
    {
        // Holds the command exit code.
        int commandExitCode = 1;

        // Displays the welcome console interface.
        Init();

        // Application loop.
        while(commandExitCode != 0)
        {
            // Holds the command.
            String command = null!;
            // Holds the arguments for the command.
            String[] args = null!;
            // Set the command context

            // Get and validate input syntax.
            ParseInput(Console.ReadLine(), out command, out args);

            // Execute proper method handler for the command.
            commandExitCode = ExecuteMethodHandler(command, args);

            Session.GetInstance().CommandContext = "HOME";
            Screen.GetInstance().PrintScreen();
        }
    }

    private void Init()
    {
        AppController.Welcome();
    }

    // Retrieves and validates user input.
    private void ParseInput(String? input, out String outCommand, out String[] outArgs)
    {
        // Input validation.
        if(input is null || input.Length == 0)
        {
            PrepareErrorCommand("Input cannot be null. Type [ help ] for a list of commands.", out outCommand, out outArgs);
            return;
        }

        // Get words.
        String[] words = input!.Split(' ');

        // Record first word as the command.
        outCommand = words[0];

        // Allocate space for the args.
        outArgs = new string[words.Length - 1];

        // Populate args.
        Array.Copy(words, 1, outArgs, 0, outArgs.Length);
    }

    // Prepares the outCommand and outArgs fields to contain an error command with the given message.
    private void PrepareErrorCommand(String message, out String outCommand, out String[] outArgs)
    {
        outCommand = "ERROR";
        outArgs = new string[]{message};
    }

    private int ExecuteMethodHandler(String command, String[] args)
    {
        // For each method of the Controller class.
        foreach (var m in ControllerMethods)
        {
            // Try to get the Command Attribute.
            var commandAtt = m.GetCustomAttribute<Command>();

            // If none, then skip it.
            if(commandAtt is null) { continue; }
            else
            {
                // If the method's command equals the user requested command:
                if(commandAtt.Name.ToUpper().Equals(command.ToUpper()))
                {
                    // Set the command context (the txt at the bottom right that tells you with what you are interacting with)
                    Session.GetInstance().CommandContext = command.ToUpper();

                    // Prepare method arguments.
                    Object[] methodArgs = {args};

                    // Invoke the method using the controller instance as the instance to invoke it on.
                    return (int) m.Invoke(AppController, methodArgs)!;
                }
            }
        }

        // If command not found, execute the error command.
        return AppController.Error(["Command not found. Type [ help ] for a list of commands"]);
    }
}