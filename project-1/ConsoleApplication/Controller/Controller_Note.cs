namespace Program.ApplicationController;

using Program.Utils;


// This second part of the class defines.
// method handlers made for pagination commands.
public partial class Controller
{
    [Command(context:Command.CommandContext.NOTE, name:"NEW", description:
    @"[C]new [note title]
    [E]Creates a new note with a given title.")]
    public int NoteTest(String[] args)
    {
        ConsoleScreen.UpdateScreenContent(["NOTE_TEST Executed !"]);

        Session.GetInstance().CommandContext = Command.CommandContext.HOME;
        
        return 1;
    }
}