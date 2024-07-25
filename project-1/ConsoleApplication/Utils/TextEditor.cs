namespace Program.Utils;

public class TextEditor
{
    // Exit codes for the Run method.
    public enum ExitCode
    {
        EXIT,
        SAVE,
        SAVE_AND_EXIT,
        CONTINUE
    };

    // Contains the note's title
    private String _title = null!;
    public String Title
    {
        get => _title;
        set
        {
            // Makes sure the title doen't go over length limit we support.
            // If it does, we cut the extra.
            if(value.Length >= Container.w)
                _title = value.Substring(0,Container.w - 2);
            else
                _title = value;
        }
    }
    // Contains the note's text.
    private List<char> _text = new List<char>();
    public List<char> Text
    {
        get => _text;
    }
    // Contains the width and height of our note editor container.
    private (int w, int h) Container;
    // Contains an offset for our note editor container.
    private (int x, int y) ContainerOffset;
    // Contains an offset to be applies to the text when rendering it on the text container.
    private int TextLineOffset = 0;
    // Stores the current position of our cursor, in Console space.
    private (int x, int y) cursorPos;

    // We require a title and optionally a text.
    public TextEditor(String title, String? text = null!)
    {
        SetContainerUp();
        Title = title;
        cursorPos = ContainerOffset;
        LoadText(text);
    }

    // Here we try to set the container up, making sure we adapt those to the console's dimensions.
    private void SetContainerUp()
    {
        // Set container dimensions to 40, 10 if possible, else adjust to console space.
        Container.w = Console.WindowWidth > 40 ? 40: Console.WindowWidth;
        // We deduce one from the height becasue we want to make sure we have space for the editor header later on.
        Container.h = Console.WindowHeight > 10 ? 10: Console.WindowHeight - 1;

        // The container horizonta offset is set up to allows us to center the text editor in the console.
        ContainerOffset.x = Console.WindowWidth / 2 - Container.w / 2;
        ContainerOffset.y = 1;
    }

    // Loads any given String into the Text list.
    private void LoadText(String? text)
    {
        if(text == null) return;

        foreach (char c in text)
        {
            Text.Add(c);
        }
    }

    // Application loop. Returns an exit code indicating what the user whishes to do.
    public ExitCode Run(String? message = null!)
    {
        Console.Clear();

        // Set cursor's initial position.
        Console.SetCursorPosition(cursorPos.x, cursorPos.y);

        // Do initial print.
        PrintBackGround();
        PrintText();

        // Print any message from the caller if any.
        if(message != null)
            PrintToCommandModeErrorLine(message);

        ExitCode exitCode = ExitCode.CONTINUE;
        ConsoleKeyInfo input;
        // Run as long as we dont hit the Escape key.
        while(exitCode == ExitCode.CONTINUE)
        {
            input = Console.ReadKey(true);

            // Process input.
            exitCode = ProcessInput(input);

            // Print the latest state of the text.
            Console.Clear();
            PrintBackGround();
            PrintText();
        }

        return exitCode;
    }

    // Prints the background (and container too)
    private void PrintBackGround()
    {
        Console.SetCursorPosition(0,0);

        // Prints whole console.
        Console.BackgroundColor = ConsoleColor.Black;
        char[] bgLine = new char[Console.WindowWidth];
        for(int i = 0; i < bgLine.Length; i++) bgLine[i] = ' ';

        for(int y = 0; y < Console.WindowHeight - 1; y++)
        {
            System.Console.WriteLine(new string(bgLine));
        }

        // Prints container rect.
        Console.ResetColor();
        char[] containerLine = new char[Container.w];
        for(int i = 0; i < containerLine.Length; i++) containerLine[i] = ' ';

        for(int y = 0; y < Container.h; y++)
        {
            Console.SetCursorPosition(ContainerOffset.x, ContainerOffset.y + y);
            System.Console.Write(new string(containerLine));
        }

        Console.SetCursorPosition(cursorPos.x, cursorPos.y);
    }

    // Parse input and call appropiate methods
    private ExitCode ProcessInput(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            // We will not implement enter fow now.
            case ConsoleKey.Enter:
                break;
            case ConsoleKey.Escape:
                return CommandMode();
            case ConsoleKey.UpArrow:
            case ConsoleKey.DownArrow:
            case ConsoleKey.RightArrow:
            case ConsoleKey.LeftArrow:
                MoveCursor(keyInfo.Key);
                break;
            case ConsoleKey.Backspace:
                RemoveCharacter();
                break;
            default:
                AddCharacter(keyInfo.KeyChar);
                break;
        }

        return ExitCode.CONTINUE;
    }

    // This here allows us to
    private ExitCode CommandMode()
    {
        ExitCode exitCode = ExitCode.CONTINUE;
        // record cursor's original position
        (int x, int y) cursorOriginalPos = Console.GetCursorPosition();

        bool done = false;
        // start command mode loop
        while(!done)
        {
            // Clear the command line
            Console.SetCursorPosition(0 + ContainerOffset.x, ContainerOffset.y + Container.h - 1);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            for(int i = 0; i < Container.w; i++) Console.Write(" ");

            // position cursor in the command line (last line)
            Console.SetCursorPosition(0 + ContainerOffset.x, ContainerOffset.y + Container.h - 1);
            System.Console.Write(">>> ");
            // Get input.
            String? input = Console.ReadLine();

            // If there is input, split it into words here.
            String[]? words = input?.Split(" ");

            // Lowe-case it to make sure we properly check for a match.
            if(input != null) input = input.ToLower();

            // Based on the input, throw the corresponding context.
            switch(input)
            {
                case "save":
                exitCode = ExitCode.SAVE;
                done = true;
                PrintToCommandModeErrorLine("Saving to the cloud...");
                break;

                case "exit save":
                case "save exit":
                exitCode = ExitCode.SAVE_AND_EXIT;
                done = true;
                break;

                case "exit":
                exitCode = ExitCode.EXIT;
                done = true;
                break;

                case "back":
                exitCode = ExitCode.CONTINUE;
                done = true;
                break;

                default:
                // Check if we used the rename [new_title] command.
                if(input != null && input.Contains("rename"))
                {
                    if(words!.Length == 2 && words[0].ToLower().Equals("rename"))
                    {
                        Title = words[1];
                        done = true;
                        break;
                    }
                }

                PrintToCommandModeErrorLine("Commands: save, exit, back and rename [new_title].");
                break;
            }
        }

        // Put cursor back to its original place.
        Console.ResetColor();
        Console.SetCursorPosition(cursorPos.x, cursorPos.y);

        return exitCode;
    }

    private void PrintToCommandModeErrorLine(String message)
    {
        Console.ResetColor();
        Console.SetCursorPosition(0 + ContainerOffset.x, ContainerOffset.y + Container.h - 2);
        Console.ForegroundColor = ConsoleColor.Red;
        System.Console.Write(message);
        Console.ResetColor();
        Console.SetCursorPosition(cursorPos.x, cursorPos.y);
    }

    // Defines to move over the text via the arrow keys.
    private void MoveCursor(ConsoleKey direction)
    {
        // Cursor position on the console.
        (int x, int y) pos = Console.GetCursorPosition();

        // Transform the position to cursor's position on the container.
        pos.x -= ContainerOffset.x;
        pos.y -= ContainerOffset.y;

        // Will store the cursor's final position on container space
        (int x, int y) newPosContainerSpace = (0,0);

        // Update movement based on contianer space
        switch (direction)
        {
            case ConsoleKey.UpArrow:
            // If moving up makes the cursor go OOB:
            if(pos.y - 1 < 0)
            {
                // OOB
                newPosContainerSpace = pos;
                ScrollText(-1);
            }
            else
            // Move cursor one spot down
            newPosContainerSpace = (pos.x, pos.y - 1);
            break;

            case ConsoleKey.DownArrow:
            // If moving down makes the cursor go OOB:
            if(pos.y + 1 >= Container.h)
            {
                // OOB
                newPosContainerSpace = pos;
                ScrollText(1);
            }
            else
            // Move cursor one spot down
            newPosContainerSpace = (pos.x, pos.y + 1);
            break;

            case ConsoleKey.LeftArrow:
            // If moving left makes the cursor go OOB:
            if(pos.x - 1 < 0)
            {
                // Move cursor one level UP and far RIGHT
                newPosContainerSpace = (Container.w - 1, pos.y - 1);

                // If new position's Y value is OOB
                if(newPosContainerSpace.y < 0)
                {
                    // Reposition to be inside the vertical boundaries.
                    // "Position the cursor at (0, 0), unless we have text lines not rendered on top, on such a case position the cursor at (MAX, 0)"
                    newPosContainerSpace = (TextLineOffset > 0 ? Container.w - 1 : 0, 0);
                    // Ask the text to shift UPWARDS
                    ScrollText(-1);
                }
            }
            else
                // Move cursor one spot to the left.
                newPosContainerSpace = (pos.x - 1, pos.y);
            break;

            case ConsoleKey.RightArrow:
            // If moving right makes the cursor go OOB:
            if(pos.x + 1 >= Container.w)
            {
                // Move cursor one level DOWN and far LEFT
                newPosContainerSpace = (0, pos.y + 1);

                // If new position's Y value is OOB
                if(newPosContainerSpace.y >= Container.h)
                {
                    // Reposition to be inside the vertical boundaries.
                    newPosContainerSpace = (0, Container.h - 1);
                    // Ask the text to shift UPWARDS
                    ScrollText(1);
                }
            }
            else
                // Move cursor one spot to the right.
                newPosContainerSpace = (pos.x + 1, pos.y);
            break;
        }


        // Check if the final position is out of bounds of the text.
        int cursorIndexPosOnArray = FromContainerToArrayIndex(newPosContainerSpace);


        // If thew new position would be outside of the text we have (+1), then we dont move. We need to create text.
        if(cursorIndexPosOnArray >= Text.Count)
        {
            newPosContainerSpace = FromArrayToContainer(Text.Count - (TextLineOffset * Container.w));
        }


        // Convert the final position in container space to console space and set the cursor position to it.
        cursorPos = FromContainerToConsole(newPosContainerSpace);
        Console.SetCursorPosition(cursorPos.x, cursorPos.y);
    }

    private (int x, int y) FromArrayToContainer(int index)
    {
        return (index - (index / Container.w) * Container.w, index / Container.w);
    }

    private (int x, int y) FromConsoleToContainer((int x, int y) consoleCoords)
    {
        return (consoleCoords.x - ContainerOffset.x, consoleCoords.y - ContainerOffset.y);
    }

    private (int x, int y) FromContainerToConsole((int x, int y) containerCoords)
    {
        return (containerCoords.x + ContainerOffset.x, containerCoords.y + ContainerOffset.y);
    }

    private int FromContainerToArrayIndex((int x, int y) containerCoords)
    {
        return (TextLineOffset * Container.w) + (containerCoords.y * Container.w + containerCoords.x);
    }

    // given an array pos, return its row position in the text container (0 based).
    private int FromArrayToRowIndex(int arrayPos)
    {
        return arrayPos - (arrayPos / Container.w) * Container.w;
    }

    private void AddCharacter(char charToAdd)
    {
        // Reject any null char.
        if(charToAdd == '\0') return;

        // Get the character index we are hovering at with the cursor.
        int cursorIndexPosOnArray = FromContainerToArrayIndex(FromConsoleToContainer(Console.GetCursorPosition()));

        // If we are just OOB, we need to add, not insert. And move the cursor to the right.
        if(cursorIndexPosOnArray + 1 == Text.Count)
        {
            Text.Add(charToAdd);
        }
        else
        {
            Text.Insert(cursorIndexPosOnArray, charToAdd);
        }

        // Move the cursor to the right.
        MoveCursor(ConsoleKey.RightArrow);
    }

    // Remove a character from the text based on cursor position.
    private void RemoveCharacter()
    {
        // Get the character index we are hovering at with the cursor.
        int cursorIndexPosOnArray = FromContainerToArrayIndex(FromConsoleToContainer(Console.GetCursorPosition()));

        // Remove previous char, if it exists.
        if(cursorIndexPosOnArray > 0)
        {
            Text.RemoveAt(cursorIndexPosOnArray - 1);
            MoveCursor(ConsoleKey.LeftArrow);
        }
    }


    // Scrolls text up or down inside the container.
    private void ScrollText(int shiftDirection)
    {
        TextLineOffset += shiftDirection;

        if(TextLineOffset < 0) TextLineOffset = 0;
    }

    // Given a container (DynamicContainer), an origin (ContainerOffset) and a vertical offset (TextVerticalOffset), it displays as much as the
    // that it can fit on the container rectangle.
    public void PrintText()
    {
        // Clear the console to avoid ghosting.
        //Console.Clear();

        PrintHeader();
        Console.SetCursorPosition(ContainerOffset.x, ContainerOffset.y);

        // Prints the header with the title.
        if(Text.Count == 0) return;

        // Container width.
        int w = Container.w;

        // For each line...
        // It also runs if the line it not completely filled with characters: ((y_row + 1) % Text.Count == 0 ? 0 : 1)
        // Takes TextVerticalOffset into consideration.
        int moduloFirst = Text.Count > 0 ? 1 : 0;
        for(int y_row = TextLineOffset; y_row < (Text.Count / w) + ((y_row + 1) % Text.Count == 0 ? moduloFirst : 1) && y_row - TextLineOffset < Container.h; y_row++)
        {
            Console.SetCursorPosition(ContainerOffset.x, ContainerOffset.y + y_row - TextLineOffset);

            // For each char on that row...
            for(int x_charIndex = 0; x_charIndex < w && (y_row * w) + x_charIndex < Text.Count; x_charIndex++)
            {
                System.Console.Write(Text[(y_row * w) + x_charIndex]);
            }
        }


        // put cursor back in place.
        Console.SetCursorPosition(cursorPos.x, cursorPos.y);
    }

    private void PrintHeader()
    {
        Console.SetCursorPosition(ContainerOffset.x, ContainerOffset.y - 1);
 
        Console.BackgroundColor = ConsoleColor.Gray;

        // Draw a white line
        for(int i = 0; i < Container.w; i++)
        {
            Console.Write(" ");
        }

        // Position cursor into palce to start printing the title.
        Console.SetCursorPosition(ContainerOffset.x + Container.w / 2 - _title.Length / 2, ContainerOffset.y - 1);

        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(_title);

        Console.ResetColor();
    }
}