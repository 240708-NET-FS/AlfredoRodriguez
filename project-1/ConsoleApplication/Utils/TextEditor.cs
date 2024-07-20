using System.Net;
using System.Security.Cryptography.X509Certificates;

public class TextEditor
{
    public String Title { get; set; } = null!;
    private List<char> Text = new List<char>();
    //private (int x, int y) ContainerOffset = (0,1);
    private (int x, int y) ContainerOffset = (5,5);


    // WOW, I just tried doing => to generate the DynamicContainer's origin at runtime with the ContainerOffset and it just worked.
    private ((int x, int y) origin, (int w, int h) dimensions) DynamicContainer = ((0, 0),(Console.WindowWidth - 5 - /* 1 */ 200,Console.WindowHeight - 5 - 3));


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

        // Print the text inside the text container space.
        PrintText();

        // Place the cursor inside the text container space, at 0,0.
        Console.SetCursorPosition(ContainerOffset.x, ContainerOffset.y);

        for(ConsoleKeyInfo input = Console.ReadKey(true); input.Key != ConsoleKey.Escape; input = Console.ReadKey(true))
        {
            //System.Console.WriteLine("You just typed: " + input.Key);
            UpdateContainerSize();
            ParseInput(input);
        }
    }

    private void UpdateContainerSize()
    {
        // This keeps the container update after a console window's resize.
        DynamicContainer = (ContainerOffset, (Console.WindowWidth - ContainerOffset.x - /* 1 */ 200,Console.WindowHeight - ContainerOffset.y - 3));
    }

    // Parse input and call appropiate methods
    private void ParseInput(ConsoleKeyInfo keyInfo)
    {

        // ==========================================================================
        // NOTE: Make sure the cursor cannot travel over the boundaries of the list.
        // OR: Make the list adapt to wathever the cursor wants to do there, maybe
        // make the list add a bunch of empty spaces plus wathever key....
        // That reminds me... shifting only if text visible. How does an Enter evn...
        // Maybe an enter means to make empty spaces of wathever is left of the line, plus one space.
        // Then better to not let the cursor get out of bounds from the array.
        // ==========================================================================

        switch (keyInfo.Key)
        {
            case ConsoleKey.LeftArrow:
            case ConsoleKey.RightArrow:
            case ConsoleKey.DownArrow:
            case ConsoleKey.UpArrow:
                MoveCursor(keyInfo.Key);
                return;
            case ConsoleKey.Spacebar:

            break;
        }
        char key = keyInfo.KeyChar;
        // Add Key to the text.
    }

    private void SelectKey()
    {

    }

    private void GetCharAtConsolePos(int x, int y)
    {
        // console dimensions, we take achunk off there and stablish our rect.
        // When printing, we always display based on the 
    }

    private void MoveCursor(ConsoleKey direction)
    {
        // Cursor position on the console
        (int x, int y) pos = Console.GetCursorPosition();

        // Transform the position to cursor's position on the container
        pos.x -= ContainerOffset.x;
        pos.y -= ContainerOffset.y;

        // Will store the cursor's final position on container space
        (int x, int y) newPosContainerSpace = (0,0);

        // Update movement based on contianer space
        switch (direction)
        {
            case ConsoleKey.UpArrow:
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
            if(pos.y + 1 >= DynamicContainer.dimensions.h)
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
            if(pos.x - 1 < 0)
            {
                // Move cursor one level UP and far RIGHT
                newPosContainerSpace = (DynamicContainer.dimensions.w - 1, pos.y - 1);

                // If new position's Y value is OOB
                if(newPosContainerSpace.y < 0)
                {
                    // Reposition to be inside the vertical boundaries.
                    newPosContainerSpace = (0, 0);
                    // Ask the text to shift UPWARDS
                    MoveText(-1);
                }
            }
            else
                // Move cursor one spot to the left.
                newPosContainerSpace = (pos.x - 1, pos.y);
            break;

            case ConsoleKey.RightArrow:
            if(pos.x + 1 >= DynamicContainer.dimensions.w)
            {
                // Move cursor one level DOWN and far LEFT
                newPosContainerSpace = (0, pos.y + 1);

                // If new position's Y value is OOB
                if(newPosContainerSpace.y >= DynamicContainer.dimensions.h)
                {
                    // Reposition to be inside the vertical boundaries.
                    newPosContainerSpace = (DynamicContainer.dimensions.w - 1, DynamicContainer.dimensions.h - 1);
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
        //int oneDimPos = newPosContainerSpace.y * DynamicContainer.dimensions.w + newPosContainerSpace.x + 1;
        int oneDimPos = FromContainerToArray(newPosContainerSpace);


        // If thew new position would be outside of the text we have (+1), then we dont move. We need to create text.
        if(oneDimPos > Text.Count + 1)
        {
            newPosContainerSpace = ArrayToContainer(Text.Count);
        }


        // Convert the final position in container space to console space and set the cursor position to it.
        //Console.SetCursorPosition(newPosContainerSpace.x + ContainerOffset.x, newPosContainerSpace.y + ContainerOffset.y);
        (int x, int y) consoleSpace = FromContainerToConsole(newPosContainerSpace);
        Console.SetCursorPosition(consoleSpace.x, consoleSpace.y);

    }


    private (int x, int y) ArrayToContainer(int index)
    {
        return (index - (index / DynamicContainer.dimensions.w) * DynamicContainer.dimensions.w, index / DynamicContainer.dimensions.w);
    }

    private (int x, int y) FromConsoleToContainer((int x, int y) consoleCoords)
    {
        return (consoleCoords.x - DynamicContainer.origin.x, consoleCoords.y - DynamicContainer.origin.y);
    }

    private (int x, int y) FromContainerToConsole((int x, int y) containerCoords)
    {
        return (containerCoords.x + ContainerOffset.x, containerCoords.y + ContainerOffset.y);
    }

    private int FromContainerToArray((int x, int y) containerCoords)
    {
        return containerCoords.y * DynamicContainer.dimensions.w + containerCoords.x + 1;
    }

    private void Enter()
    {
        //An enter creates a new line, which in this single string means spli
        int cursorPosOnArray = FromContainerToArray(FromConsoleToContainer(Console.GetCursorPosition()));

        //Text.InsertRange()

        // Enter means add a new list.
        // When parsing the list to text, take each new list as a \n


    }


    private void MoveText(int shiftDirection)
    {

    }

    public void PrintText()
    {
        int w = DynamicContainer.dimensions.w;

        // For each line...
        // It also runs if the line it not completely filled with characters: ((y_line + 1) % Text.Count == 0 ? 0 : 1)
        for(int y_line = 0; y_line < (Text.Count / w) + ((y_line + 1) % Text.Count == 0 ? 0 : 1); y_line++)
        {
            Console.SetCursorPosition(ContainerOffset.x, ContainerOffset.y + y_line);
            // For each char on that line...
            for(int x_charIndex = 0; x_charIndex < w && (y_line * w) + x_charIndex < Text.Count; x_charIndex++)
            {
                System.Console.Write(Text[(y_line * w) + x_charIndex]);
            }

            //System.Console.WriteLine();
        }
    }
}