namespace Program.Utils;

using System.Reflection;
using Program.ApplicationController;


// Manages the console's UI.
class Screen
{
    public enum InputState
    {
        ALLOWED, // Visuually display the console's input line as if it allowed input.
        FORBIDDEN // Visually display the console's input line as if not allowing input.
    };

    private static int lineTokenLen = 3;

    // Deines the screen's UI. Prints contents on the correct place of the UI.
    public void PrintScreen(String content, InputState state, String command, ConsoleColor color = ConsoleColor.White)
    {
        PrintScreen(new String[]{content}, state, command, new ConsoleColor[]{color});
    }

/*
Presentation takes from consolke
Service does logid
repo communicates with the database

*/
    public void PrintScreen(String[] lines, InputState state, String command, ConsoleColor[] colors)
    {
        Console.ResetColor();
        Console.Clear();
        PrintHeadLine();
        PrintInfoLine();

        Console.WriteLine();
        for(int i = 0; i < lines.Length; i++)
        {
            Console.ForegroundColor = colors[i < colors.Length ? i : colors.Length - 1];
            Console.Write(lines[i]);
        }
        Console.ForegroundColor = ConsoleColor.White;

        PrintInputLine(state, command);
    }

    // Prints out all the commands.
    public void PrintCommands()
    {
        List<String> lines = new List<String>();
        List<ConsoleColor> colors = new List<ConsoleColor>();
        MethodInfo[] methods = typeof(Controller).GetMethods();

        foreach (var m in methods)
        {
            // Try to get the Action Attribute
            var att = m.GetCustomAttribute<Command>();

            // If none, then skip it.
            if(att is null) { continue; }
            else
            {
                // Get description lines.
                String? commandDescription = att.Description;
                
                if(commandDescription is null) continue;

                String[] descLines = commandDescription.Split("\n");

                for(int i = 0; i < descLines.Length; i++)
                {
                    // Given a bunch of description lines, populate the lines and colors lists with the final lines to output and their corresponding colors
                    ProcessDescriptionLine(descLines[i], ref lines, ref colors);
                }
            }
        }

        // Prints all the lines on their corresponding colors.
        PrintScreen(lines.ToArray<String>(), InputState.ALLOWED, "HOME", colors.ToArray<ConsoleColor>());
    }

    // String descLine: A line composed of an initial token in the form of "[X]" and a sentence that follows.
    // List<String> lines: a list to populate with each individual line (after cutting out initial token at the beginning of it).
    // List<ConsoleColor> colors: a list to populate with console colors. These are supposed to represent the colors that each line is to be printed as.
    private void ProcessDescriptionLine(String descLine, ref List<String> lines, ref List<ConsoleColor> colors)
    {

        descLine = descLine.TrimStart();

        // If the line doesn't have at least "lineTypeStrLen" number of characters, we assign the [U] type for Unknown.
        if(descLine.Length < lineTokenLen)
        {
            descLine = "[U]" + descLine;
        }

        // Will hold the line token.
        char[] lineToken = new char[lineTokenLen];
        // Will hold an individual line of the description (without the token).
        char[] line = new char[descLine.Length - lineTokenLen];

        // Copy the token characters into the loneToken char array.
        Array.Copy(descLine.ToCharArray(), 0, lineToken, 0, lineTokenLen);

        // Copy the rest of the line into the line char array.
        Array.Copy(descLine.ToCharArray(), lineTokenLen, line, 0, line.Length);

        // Here we define the color and format for each line depending on its token.
        switch (new string(lineToken))
        {
            case "[C]":
            {
                colors.Add(ConsoleColor.Green);
                lines.Add(new string(line) + "\n");
                return;
            }
            case "[E]":
            {
                colors.Add(ConsoleColor.Blue);
                lines.Add("    " + new string(line) + "\n");
                return;
            }
            case "[U]":
            {
                colors.Add(ConsoleColor.White);
                lines.Add("    " + new string(line)! + "\n");
                return;
            }
            default:
            {
                // If the token is non of the ones listed here, we cannot assume that the initial characters were meant to be a toke nat all,
                // so we just add the whole description line as it is and set the color to white.
                colors.Add(ConsoleColor.White);
                lines.Add(descLine + "\n");
                return;
            }
        }
    }

    // Clears the screen.
    public void ClearScreen()
    {
        Console.Clear();
    }


    // Prints the info line (second line)
    private void PrintInfoLine()
    {
        String userName = Session.GetInstance().User;
        bool isOnline = Session.GetInstance().User != null;
        String status = isOnline ? "LOGGED IN" : "LOG IN";

        ConsoleColor statusColor = isOnline ? ConsoleColor.Green : ConsoleColor.Red;

        // TODO: place this at the right maybe.
        Console.SetCursorPosition(isOnline ? Console.WindowWidth - (9 + status.Length + userName.Length): Console.WindowWidth - (status.Length + 5),1);
        Console.Write("[ ");
        Console.ForegroundColor = statusColor;
        Console.Write($"{status}");
        Console.ResetColor();
        Console.Write(" ] " + (isOnline ? $"as {userName}." : ""));
    }

    // Takes care of printing the head line (first line)
    private void PrintHeadLine()
    {
        const String headTitle = " todo ";
        for(int i = 0; i < Console.WindowWidth; i++)
        {
            if(i == (Console.WindowWidth / 2) - (headTitle.Length / 2))
            {
                Console.Write(headTitle);
                i += headTitle.Length;
            }
            else
            {
                Console.Write("-");
            }
        }
        Console.WriteLine();
    }

    private void PrintContentPage()
    {

    }

    private void PrintInputLine(InputState state, String command, ConsoleColor color = ConsoleColor.DarkGray)
    {
        Console.SetCursorPosition(0, Console.WindowHeight);
        Console.ForegroundColor = state == InputState.ALLOWED ? ConsoleColor.White : ConsoleColor.DarkGray;
        Console.Write(">>>");
        Console.SetCursorPosition(Console.WindowWidth - command.Length, Console.WindowHeight);
        Console.ForegroundColor = color;
        Console.Write(command);
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(4, Console.WindowHeight);
    }

    public void PromptQuestion(string question)
    {
        Console.Clear();

    }
}