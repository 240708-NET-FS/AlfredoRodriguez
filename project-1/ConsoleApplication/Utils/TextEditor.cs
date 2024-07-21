using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public class TextEditor
{
    public String Title { get; set; } = null!;
    private List<char> Text = new List<char>();
    private (int x, int y) ContainerOffset = (1,1);
    private (int w, int h) DynamicContainer;
    private int TextVerticalOffset = 0;

    public TextEditor(String title, String text)
    {
        LoadContentToTextArray(text);
    }

    private void LoadContentToTextArray(String text)
    {
        foreach (char c in text)
        {
            Text.Add(c);
        }
    }

    public void Run()
    {
        Console.Clear();

        // Set cursor's initial position.
        Console.SetCursorPosition(ContainerOffset.x, ContainerOffset.y);

        // Do initial print.
        UpdateContainerSize();
        PrintText();

        // Run as long as we dont hit the Escape key.
        for(ConsoleKeyInfo input = Console.ReadKey(true); input.Key != ConsoleKey.Escape; input = Console.ReadKey(true))
        {
            // Updates the container's dimensions based on the current state of the console.
            UpdateContainerSize();

            // Process input.
            ProcessInput(input);

            // Print the latest state of the text.
            PrintText();
        }
    }

    private void UpdateContainerSize()
    {
        /* (int w, int h) oldValues = DynamicContainer;
        int hoveredCharIndex = FromContainerToArrayIndex(FromConsoleToContainer(Console.GetCursorPosition())); */
        // This keeps the container update after a console window's resize.
        DynamicContainer = (40, 10);

/*         // The console has changed shapes, so we will reposition the cursor based on its last position on the text array.
        if(DynamicContainer.w != oldValues.w || DynamicContainer.h != oldValues.h)
        {

            // find the column where our cursor should be standing at given the new container dimensions.
            int hoveredRowIndex = hoveredCharIndex / DynamicContainer.w;

            // If that column is hidden up from view due scroll, scroll up to it.
            if(TextVerticalOffset > hoveredRowIndex) TextVerticalOffset = hoveredRowIndex;

            // If that column is hidden down from view due scroll, scroll down to it.
            if(hoveredRowIndex > TextVerticalOffset + DynamicContainer.h) TextVerticalOffset -= hoveredRowIndex - (TextVerticalOffset + DynamicContainer.h);

            // Position the cursor to hover over the desired char again.
            (int x, int y) newPos = FromArrayToContainer(hoveredCharIndex);
            Console.SetCursorPosition(newPos.x, newPos.y);
        } */
    }

    // Parse input and call appropiate methods
    private void ProcessInput(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            // We will not implement enter fow now.
            case ConsoleKey.Enter:
                return;
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
                MoveText(-1);
            }
            else
            // Move cursor one spot down
            newPosContainerSpace = (pos.x, pos.y - 1);
            break;

            case ConsoleKey.DownArrow:
            // If moving down makes the cursor go OOB:
            if(pos.y + 1 >= DynamicContainer.h)
            {
                // OOB
                newPosContainerSpace = pos;
                MoveText(1);
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
                newPosContainerSpace = (DynamicContainer.w - 1, pos.y - 1);

                // If new position's Y value is OOB
                if(newPosContainerSpace.y < 0)
                {
                    // Reposition to be inside the vertical boundaries.
                    // "Position the cursor at (0, 0), unless we have text lines not rendered on top, on such a case position the cursor at (MAX, 0)"
                    newPosContainerSpace = (TextVerticalOffset > 0 ? DynamicContainer.w - 1 : 0, 0);
                    // Ask the text to shift UPWARDS
                    MoveText(-1);
                }
            }
            else
                // Move cursor one spot to the left.
                newPosContainerSpace = (pos.x - 1, pos.y);
            break;

            case ConsoleKey.RightArrow:
            // If moving right makes the cursor go OOB:
            if(pos.x + 1 >= DynamicContainer.w)
            {
                // Move cursor one level DOWN and far LEFT
                newPosContainerSpace = (0, pos.y + 1);

                // If new position's Y value is OOB
                if(newPosContainerSpace.y >= DynamicContainer.h)
                {
                    // Reposition to be inside the vertical boundaries.
                    newPosContainerSpace = (0, DynamicContainer.h - 1);
                    // Ask the text to shift UPWARDS
                    MoveText(1);
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
            newPosContainerSpace = FromArrayToContainer(Text.Count - (TextVerticalOffset * DynamicContainer.w));
        }


        // Convert the final position in container space to console space and set the cursor position to it.
        (int x, int y) consoleSpace = FromContainerToConsole(newPosContainerSpace);
        Console.SetCursorPosition(consoleSpace.x, consoleSpace.y);
    }

    private (int x, int y) FromArrayToContainer(int index)
    {
        return (index - (index / DynamicContainer.w) * DynamicContainer.w, index / DynamicContainer.w);
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
        return (TextVerticalOffset * DynamicContainer.w) + (containerCoords.y * DynamicContainer.w + containerCoords.x);
    }

    // given an array pos, return its row position in the text container (0 based).
    private int FromArrayToRowIndex(int arrayPos)
    {
        return arrayPos - (arrayPos / DynamicContainer.w) * DynamicContainer.w;
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


    // Properly updates the TextVerticalOffset variable which is used when displaying text on the container.
    private void MoveText(int shiftDirection)
    {
        TextVerticalOffset += shiftDirection;

        // 
        if(TextVerticalOffset < 0) TextVerticalOffset = 0;

    }

    // Given a container (DynamicContainer), an origin (ContainerOffset) and a vertical offset (TextVerticalOffset), it displays as much as the
    // that it can fit on the container rectangle.
    public void PrintText()
    {
        if(Text.Count == 0) return;

        // record cursor position before starting.
        (int x, int y) pos = Console.GetCursorPosition();

        // Clear the console to avoid ghosting.
        Console.Clear();

        // Container width.
        int w = DynamicContainer.w;

        // For each line...
        // It also runs if the line it not completely filled with characters: ((y_row + 1) % Text.Count == 0 ? 0 : 1)
        // Takes TextVerticalOffset into consideration.
        for(int y_row = TextVerticalOffset; y_row < + (Text.Count / w) + ((y_row + 1) % Text.Count == 0 ? 0 : 1) && y_row - TextVerticalOffset < DynamicContainer.h; y_row++)
        {
            Console.SetCursorPosition(ContainerOffset.x, ContainerOffset.y + y_row - TextVerticalOffset);

            // For each char on that row...
            for(int x_charIndex = 0; x_charIndex < w && (y_row * w) + x_charIndex < Text.Count; x_charIndex++)
            {
                System.Console.Write(Text[(y_row * w) + x_charIndex]);
            }
        }

        // put cursor back in place.
        Console.SetCursorPosition(pos.x, pos.y);
    }
}