using Program.Utils;
using Program.DAO;
using Program.Model;

public class NoteService
{
    private NoteDAO NoteDAO = null!;
    private Screen Screen = null!;
    public NoteService()
    {
        NoteDAO = new NoteDAO();
        Screen = Screen.GetInstance();
    }

    // Retrieves notes and displays them.
    public void PrintNotes(String user)
    {
        // Get all the notes for this user.
        List<Note>? notes = NoteDAO.GetAllNotesByUserName(user);

        // If no notes, then inform the user.
        if(notes is null || notes.Count == 0)
        {
            Screen.UpdateScreenContent(["Empty.", "", "", "Try: new [note_title]"],[ConsoleColor.DarkGray]);
            return;
        }

        // We then proceed to create a string array with all the info we want to print out.

        // First, we allocate an aray to contain as many strings as notes we have.
        String[] notesArray = new String[notes.Count];

        // Then, for each note we create an array that contains the note info in a readable way.
        for(int i = 0; i < notesArray.Length; i++)
        {
            notesArray[i] = $"[{notes[i].Id}] {notes[i].Title}";
        }

        // Finally, we print that array to the console.
        Screen.UpdateScreenContent(notesArray);
    }

    public void OpenNote(int noteId)
    {
        User? currentUser = Session.GetInstance().User;

        if(currentUser == null)
        {
            Screen.SetMessage("You must be logged in to open a note.", Screen.MessageType.Error);
            return;
        }

        Note? note = NoteDAO.GetNoteById(noteId, currentUser);

        if(note is null)
        {
            Screen.SetMessage($"No note with ID [{noteId}] found.", Screen.MessageType.Error);
            return;
        }

        StartEditor(note);
    }

    public void OpenNote(String noteTitle)
    {
        User? currentUser = Session.GetInstance().User;

        if(currentUser == null)
        {
            Screen.SetMessage("You must be logged in to open a note.", Screen.MessageType.Error);
            return;
        }

        Note? note = NoteDAO.GetNoteByTitle(noteTitle, currentUser);

        if(note is null)
        {
            Screen.SetMessage($"No note with title [{noteTitle}] found.", Screen.MessageType.Error);
            return;
        }

        StartEditor(note);
    }

    public void CreateNote(String noteTitle)
    {
        StartEditor(new Note{ Title = noteTitle });
    }

    public void StartEditor(Note note/* String title, String? contents = null, int noteId = -1 */)
    {

        if(note.Title is null)
        {
            Screen.SetMessage("You must provide a title for the note like this: new [note_title].", Screen.MessageType.Error);
            return;
        }


        TextEditor editor = new TextEditor(note.Title, note.Content);
        TextEditor.ExitCode exitCode = TextEditor.ExitCode.CONTINUE;
        String? loopMessage = null;


        // Run the app
        while(exitCode != TextEditor.ExitCode.EXIT && exitCode != TextEditor.ExitCode.SAVE_AND_EXIT)
        {
            exitCode = editor.Run(loopMessage);

            // Update our note with the new contents. Leave the ID intact as the text editor doesnt touch that.
            String content = "";
            foreach (char c in editor.Text) content += c;
            note.Title = editor.Title;
            note.Content = content;

            // Reset the loop message.
            loopMessage = null;

            // If the user wants to save the note, attempt to save it:
            if(exitCode == TextEditor.ExitCode.SAVE || exitCode == TextEditor.ExitCode.SAVE_AND_EXIT)
            {
                // If the note is a new note, then create a new record with it.
                if(note.User == null)
                {
                    note = NoteDAO.AddNote(note.Title, note.Content!, Session.GetInstance().User!)!;

                    if(note is null)
                    {
                        Screen.SetMessage("Oops. Something went wrong on the database while creating the note.", Screen.MessageType.Error);
                        return;
                    }
                }
                else
                {
                    note = NoteDAO.UpdateNote(note)!;

                    if(note is null)
                    {
                        Screen.SetMessage("Oops. Something went wrong on the database while updating the note.", Screen.MessageType.Error);
                        return;
                    }
                }

                if(exitCode == TextEditor.ExitCode.SAVE) loopMessage = "Note saved.";
            }
        }
    }

    public void RemoveNote(int noteId)
    {

        User? currentUser = Session.GetInstance().User;

        if(currentUser == null)
        {
            Screen.SetMessage("You must be logged in to open a note.", Screen.MessageType.Error);
            return;
        }

        Note? note = NoteDAO.RemoveNoteById(noteId, currentUser);

        if(note is null)
        {
            Screen.SetMessage($"No note with ID [{noteId}] found.", Screen.MessageType.Error);
            return;
        }
    }
    
    public void RemoveNote(String noteTitle)
    {

        User? currentUser = Session.GetInstance().User;

        if(currentUser == null)
        {
            Screen.SetMessage("You must be logged in to open a note.", Screen.MessageType.Error);
            return;
        }

        Note? note = NoteDAO.RemoveNoteByTitle(noteTitle, currentUser);

        if(note is null)
        {
            Screen.SetMessage($"No note with title [{noteTitle}] found.", Screen.MessageType.Error);
            return;
        }
    }
}