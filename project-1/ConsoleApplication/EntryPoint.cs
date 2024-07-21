using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Program.Data;
using Program.Model;

namespace Program;

public class EntryPoint
{
    public static void Main(String[] args)
    {
        DatabaseContext c = Connection.Get();
        List<User> users = c.Users.ToList();

        System.Console.WriteLine("Users:");
        foreach (var u in users)
        {
            System.Console.WriteLine($"id: {u.Id}, name: {u.Name}");
        }
        Console.ReadKey();
        
        ConsoleApp app = new ConsoleApp();
        app.Run();

        //String text = @"This is a bare-bones text editor. You cannot add new lines. You can move around the text, write to it and delete from it. TODO: I still need to connect this to a command, finish TextEditor UI, do databases, do tests. FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFAAAAAAAAAAAAAAAAAAAAAAAA1212313213213212321FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFgggggggggggggggaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaccccccccccccssssssssssssssssssss. One two three four five siz seven eight nine ten 12345678910...?><}{:_+}`~~~~`";
        //TextEditor textEditor = new TextEditor("MyTitle", text);
        //textEditor.PrintText();
        //textEditor.Run();
    }
}
