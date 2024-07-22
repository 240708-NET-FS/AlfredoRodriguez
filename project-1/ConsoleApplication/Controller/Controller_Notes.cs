namespace Program.ApplicationController;

using Program.Utils;

// Here we define method handlers for the NOTES contex commands.
public partial class Controller
{
    
    // ENTRY POINT
    [Command(context:Command.CommandContext.NOTES, name:"NOTES", description:
    @"[C]notes
    [E]Displays all of user's notes.")]
    public int NotesNotes(String[] args)
    {
        // Validate input.
        if(!ValidateInput("NOTES", [], args)) return -1;

        // Get the user.
        String? user = Session.GetInstance().User?.Name;

        // Make sure we are logged in.
        if(user is null) return Error(["You must be logged in to execute the [ notes ] command."]);

        // Conditions for entering the NOTES context met, so we enter it.
        Session.GetInstance().CommandContext = Command.CommandContext.NOTES;

        // Print all the notes for that user
        NoteService.PrintNotes(user);

        return 1;
    }

    // Opens the text editor.
    [Command(context:Command.CommandContext.NOTES, name:"NEW", description:
    @"[C]new [note_title]
    [E]Creates a new note with a given title. You can rename the title later in the editor.")]
    public int NotesNew(String[] args)
    {
        // Validate input.
        if(!ValidateInput("NEW", ["note_title"], args)) return -1;

        // Create note.
        NoteService.CreateNote(args[0]);

        // Print the list of notes.
        NotesNotes([]);

        return 1;
    }

    // Attempts to load a saved note into the text editor.
    [Command(context:Command.CommandContext.NOTES, name:"open", description:
    @"[C]open [note_title]
    [E]Attempts to open a note by name.")]
    public int NotesOpen(String[] args)
    {
        // Validate input.
        if(!ValidateInput("OPEN", ["note_title"], args)) return -1;

        // Open note.
        NoteService.OpenNote(args[0]);

        // Print the list of notes.
        NotesNotes([]);
        return 1;
    }
}