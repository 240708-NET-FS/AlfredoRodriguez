namespace Program.Utils;

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Program.ApplicationController;


// Manages the console's UI.
class Screen
{
    public enum InputState
    {
        ALLOWED, // Visually display the console's input line as if it allowed input.
        FORBIDDEN // Visually display the console's input line as if not allowing input.
    };

    private int lineTokenLen = 3;
    private const int ReservedLinesCount = 3;
    private String[] Contents = [""];
    private ConsoleColor[]? ContentColors = null;
    private int ContentsPageIndex = 0;
    static private Screen Instance = null!;

    private Screen(){}
    static public Screen GetInstance()
    {
        if(Instance is null)
        {
            Instance = new Screen();
        }

        return Instance;
    }

    // Updates the screen contents.
    public void UpdateScreenContent(String[] contents, ConsoleColor[]? colors = null, bool print = true)
    {
        Contents = contents;
        ContentColors = colors is null || colors.Length == 0 ? [ConsoleColor.White] : colors;
        ContentsPageIndex = 0;
        if(print) PrintScreen();
    }

    // Displays the screen contents. Also allows you to set the input state and the command
    public void PrintScreen(InputState state = InputState.ALLOWED)
    {
        Console.Clear();
        Console.ResetColor();
        PrintHeadLine();
        PrintInfoLine();
        PrintContentPage();
        PrintInputLine(state);
    }

    // Prints out all the commands.
    public void PrintCommands()
    {
        List<String> lines = new List<String>();
        List<ConsoleColor> colors = new List<ConsoleColor>();
        MethodInfo[] methods = typeof(Controller).GetMethods();

        lines.Add($"Commands for the { Session.GetInstance().CommandContext.ToString() } context.");
        colors.Add(ConsoleColor.White);

        foreach (var m in methods)
        {
            // Try to get the Action Attribute
            var att = m.GetCustomAttribute<Command>();

            // If none, then skip it.
            if(att is null) { continue; }
            else
            {
                // If this command atribute is not of type ANY or of the current command context type accepted, continue.
                if(att.Context != Command.CommandContext.ANY && att.Context != Session.GetInstance().CommandContext)
                {
                    continue;
                }

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
        UpdateScreenContent(lines.ToArray<String>(),colors.ToArray<ConsoleColor>());
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
                colors.Add(ConsoleColor.Blue);
                lines.Add(new string(line));
                return;
            }
            case "[E]":
            {
                colors.Add(ConsoleColor.DarkGray);
                lines.Add("    " + new string(line));
                return;
            }
            case "[U]":
            {
                colors.Add(ConsoleColor.White);
                lines.Add("    " + new string(line));
                return;
            }
            default:
            {
                // If the token is non of the ones listed here, we cannot assume that the initial characters were meant to be a toke nat all,
                // so we just add the whole description line as it is and set the color to white.
                colors.Add(ConsoleColor.White);
                lines.Add(descLine);
                return;
            }
        }
    }

    // Prints the info line (second line)
    private void PrintInfoLine()
    {
        Console.ResetColor();

        String? userName = Session.GetInstance().User?.Name;
        bool isOnline = Session.GetInstance().User != null;
        String status = isOnline ? "LOGGED IN" : "NOT LOGGED IN";

        ConsoleColor statusColor = isOnline ? ConsoleColor.Green : ConsoleColor.Red;

        // TODO: place this at the right maybe.
        Console.SetCursorPosition(isOnline ? Console.WindowWidth - (9 + status.Length + userName!.Length): Console.WindowWidth - (status.Length + 5),1);
        Console.Write("[ ");
        Console.ForegroundColor = statusColor;
        Console.Write($"{status}");
        Console.ResetColor();
        Console.Write(" ] " + (isOnline ? $"as {userName}." : ""));

        Console.WriteLine();
    }

    // Takes care of printing the head line (first line)
    private void PrintHeadLine()
    {
        Console.ResetColor();

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
        Console.ResetColor();
        // If there is no content to print, return.
        //if(Contents is null) return;// ----------------------------------------***********
        // If there is no defined color, use White.
        if(ContentColors is null) ContentColors = [ConsoleColor.White];

        int availibleLines = GetContentLineCount();

        // Find the starting index for the page in content.
        int copyStartAtIndex = ContentsPageIndex * availibleLines;
        // Find the ending index for the page in the content.
        int copyEndAfterIndex = copyStartAtIndex + availibleLines - 1;
        // Trim the ending index according to the content (in case a page can hold up to X items naturally but there are only (X - Y) availible).
        copyEndAfterIndex = copyEndAfterIndex >= Contents.Length ? Contents.Length - 1 : copyEndAfterIndex;

        for(int i = copyStartAtIndex; i <= copyEndAfterIndex; i++)
        {
            Console.ForegroundColor = ContentColors[i < ContentColors.Length ? i : ContentColors.Length - 1];
            Console.WriteLine(Contents[i]);
        }

        if(IsContentMultiPage())
        {
            Console.ResetColor();
            int totalPages = (Contents.Length / availibleLines);
            totalPages = (Contents.Length % availibleLines) > 0 ? totalPages + 1 : totalPages;
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"page{ContentsPageIndex + 1}/{totalPages}");
            Console.ResetColor();
        }
    }

    // Finds out how many lines we can use to print the content.
    private int GetContentLineCount()
    {

        bool isMultiPage = IsContentMultiPage();
        int availibleLines = Console.WindowHeight - (isMultiPage ? ReservedLinesCount + 1 : ReservedLinesCount);

        return availibleLines;
    }

    // Find out if the content takes more than one page to print.
    private bool IsContentMultiPage()
    {
        return Contents.Length > Console.WindowHeight - ReservedLinesCount;
    }

    public void ChangePage(int dir)
    {
        int availibleLines = GetContentLineCount();

        int totalPages = (Contents.Length / availibleLines);
        totalPages = (Contents.Length % availibleLines) > 0 ? totalPages + 1 : totalPages;

        int targetPage = ContentsPageIndex + 1 + dir;

        if(targetPage <= totalPages && targetPage > 0)
        {
            ContentsPageIndex += dir;
            PrintScreen(InputState.ALLOWED);
        }
    }

    private void PrintInputLine(InputState state, ConsoleColor color = ConsoleColor.DarkGray)
    {
        String contextMessage = "current context -> " + Session.GetInstance().CommandContext;
        Console.ResetColor();
        Console.SetCursorPosition(0, Console.WindowHeight);

        if(state == InputState.ALLOWED)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = true;
            System.Console.Write(">>>");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.CursorVisible = false;
            System.Console.Write("...");
        }

        Console.SetCursorPosition(Console.WindowWidth - contextMessage.Length, Console.WindowHeight);
        Console.ForegroundColor = color;
        Console.Write(contextMessage);
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(4, Console.WindowHeight);
    }

    public void ClearScreen()
    {
        Console.Clear();
    }
}