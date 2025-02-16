namespace Program.ApplicationController;

using Program.Utils;


// Here we define method handlers for the NOTES contex commands.
public partial class Controller
{
    
    // ENTRY POINT for the NOTES context.
    // Displays the user's notes.
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
        if(user is null)
        {
            _screen.SetMessage("You must be logged in to execute the [ NOTES ] command.", Screen.MessageType.Error);
            return -1;
        }

        // Conditions for entering the NOTES context met, so we enter it.
        Session.GetInstance().CommandContext = Command.CommandContext.NOTES;

        // Print all the notes for that user
        _noteService.PrintNotes(user);

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
        _noteService.CreateNote(args[0]);

        // Print the list of notes.
        NotesNotes([]);

        return 1;
    }

    // Attempts to load a saved note into the text editor.
    [Command(context:Command.CommandContext.NOTES, name:"open", description:
    @"[C]open [flag] [note_title]
    [E] open -n [title] -> opens a note by title (first match).
    [E] open -id [id] -> opens a note by id.")]
    public int NotesOpen(String[] args)
    {
        // Validate input.
        if(!ValidateInput("OPEN", ["flag", "note_title"], args)) return -1;

        // Attempt to open anote.
        // If the user misspells something, we just print the notes again and throw no error to not break the execution flow.
        if(args[0].Equals("-n"))
            _noteService.OpenNote(args[1]);
        else if(args[0].Equals("-id"))
        {
            try
            {
                _noteService.OpenNote(int.Parse(args[1]));
            }
            catch(FormatException)
            {
                _screen.SetMessage($"{args[1]} is not a valid note ID.", Screen.MessageType.Error);
            }
        }
        else
        {
            _screen.SetMessage($"The flag [{args[0]}] is not a valid flag.", Screen.MessageType.Error);
        }

        // Print the list of notes.
        NotesNotes([]);
        return 1;
    }

    // Attempts to load a saved note into the text editor.
    [Command(context:Command.CommandContext.NOTES, name:"remove", description:
    @"[C]remove [flag] [note_title]
    [E] remove -n [title] -> removes a note by title (first match).
    [E] remove -id [id] -> removes a note by id.")]
    public int NotesRemove(String[] args)
    {
        // Validate input.
        if(!ValidateInput("REMOVE", ["flag", "note_title"], args)) return -1;

        // Attempt to remove anote.
        if(args[0].Equals("-n"))
            _noteService.RemoveNote(args[1]);
        else if(args[0].Equals("-id"))
        {
            try
            {
                _noteService.RemoveNote(int.Parse(args[1]));
            }
            catch(FormatException)
            {
                _screen.SetMessage($"{args[1]} is not a valid note ID.", Screen.MessageType.Error);
            }
        }
        else
        {
            _screen.SetMessage($"The flag [{args[0]}] is not a valid flag.", Screen.MessageType.Error);
        }

        // Print the list of notes.
        NotesNotes([]);
        return 1;
    }
}