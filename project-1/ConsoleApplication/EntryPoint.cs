﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Program;

public class EntryPoint
{
    public static void Main(String[] args)
    {
        //ConsoleApp app = new ConsoleApp();
        //app.Run();

        String text = "This is a sample text that is supposed to be rendered on the jjjjjjjjjjjjjjscreen.";
        TextEditor textEditor = new TextEditor("MyTitle", text);
        //textEditor.PrintText();
        textEditor.Run();
    }
}
