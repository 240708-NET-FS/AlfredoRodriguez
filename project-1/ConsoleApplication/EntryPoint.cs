using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Program;

public class EntryPoint
{
    public static void Main(String[] args)
    {
        //ConsoleApp app = new ConsoleApp();
        //app.Run();

        String text = @"This is a sample text that is supposed to be rendered on the jjjjjjjjjjjjjjscreen. sdddddddddddddddddddddddddddddddddasdgfsdhgsdrhdshdsjfdhfdhdfgh  hdh xdhdhxdhd5h41dz541h5zd1hj5zd1h";
        TextEditor textEditor = new TextEditor("MyTitle", text);
        //textEditor.PrintText();
        textEditor.Run();
    }
}
