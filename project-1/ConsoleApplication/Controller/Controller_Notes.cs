namespace Program.ApplicationController;

using Program.Model;
using Program.Utils;


// This second part of the class defines.
// method handlers made for pagination commands.
public partial class Controller
{
    private NoteService NoteService = new NoteService();


    // This is the entry command for the NOTES context.
    // It is in charge of validating that the conditions for entering the context
    // are met.
    [Command(context:Command.CommandContext.NOTES, name:"NOTES", description:
    @"[C]notes
    [E]Displays all of user's notes.")]
    public int NotesNotes(String[] args)
    {
        // Check that there are no extra args.
        if(args.Length > 0) return Error(["The [ notes ] command doesn't take any extra arguments."]);

        // Get the user.
        String? user = Session.GetInstance().User?.Name;

        // If by any chance the user is null, go home.
        if(user is null) return Error(["You must be logged in to execute the [ notes ] command."]);

        // Conditions for entering the NOTES context met, so we enter it.
        Session.GetInstance().CommandContext = Command.CommandContext.NOTES;

        // Print all the notes for that user
        NoteService.PrintNotes(user);

        return 1;
    }

    [Command(context:Command.CommandContext.NOTES, name:"NEW", description:
    @"[C]new [note_title]
    [E]Creates a new note with a given title. You can rename the title later in the editor.")]
    public int NotesNew(String[] args)
    {
        if(args == null || args.Length != 1) return Error(["The new command takes up one only argument: [note_title]"]);


        NoteService.CreateNote(args[0]);

        return 1;
    }

    [Command(context:Command.CommandContext.NOTES, name:"open", description:
    @"[C]open [note_title]
    [E]Attempts to open a note by name.")]
    public int NotesOpen(String[] args)
    {
        if(args == null || args.Length != 1) return Error(["The open command takes up one only argument: [note_title]"]);


        NoteService.OpenNote(args[0]);

        return 1;
    }
}