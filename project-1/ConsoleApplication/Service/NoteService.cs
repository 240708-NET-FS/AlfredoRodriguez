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

    // Retrieves notes and displays them
    public void PrintNotes(String user)
    {
        // Get all the notes for this user
        List<Note>? notes = NoteDAO.GetAllNotesByUserName(user);

        // If no notes, then inform the user.
        if(notes is null)
        {
            Screen.UpdateScreenContent(["You have no notes currently.", "Try the [ new ] command to enter the text editor."]);
            return;
        }

        // We then proceed to create a string array with all the info we want to print out.

        // First, we allocate an aray to contain as many strings as notes we have.
        String[] notesArray = new String[notes.Count];

        // Then, for each note we create an array that contains the note info in a readable way.
        for(int i = 0; i < notesArray.Length; i++)
        {
            notesArray[i] = $"{notes[i].Id} - {notes[i].Title}";
        }

        // Finally, we print that array to the console.
        Screen.UpdateScreenContent(notesArray);
    }

    public void OpenNote(int noteId)
    {
        Note? note = NoteDAO.GetNoteById(noteId);

        if(note is null)
        {
            Screen.UpdateScreenContent([$"No note with ID {noteId} exist."]);
            return;
        }

        StartEditor(note);
    }

    public void OpenNote(String noteTitle)
    {
        Note? note = NoteDAO.GetNoteByTitle(noteTitle);

        if(note is null)
        {
            Screen.UpdateScreenContent([$"The note {noteTitle} doesn't exist."]);
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
            Screen.UpdateScreenContent(["You must provide a title for the note like this: new [note_title]."]);
            return;
        }


        TextEditor editor = new TextEditor(note.Title, note.Content);
        TextEditor.ExitCode exitCode = TextEditor.ExitCode.CONTINUE;
        String? loopMessage = null;


        // Run the app
        while(exitCode != TextEditor.ExitCode.EXIT && exitCode != TextEditor.ExitCode.SAVE_AND_EXIT)
        {
            Note newContents;


            (exitCode, newContents) = editor.Run(loopMessage);


            // Update our note with the new contents. Leave the ID intact as the text editor doesnt touch that.
            note.Title = newContents.Title;
            note.Content = newContents.Content;
            loopMessage = null;


            // If the user wants to save the note, attempt to save it:
            if(exitCode == TextEditor.ExitCode.SAVE || exitCode == TextEditor.ExitCode.SAVE_AND_EXIT)
            {
                // If the note is a new note, then create a new record with it.
                if(note.User == null)
                {
                    note = NoteDAO.AddNote(note.Title, note.Content!)!;

                    if(note is null)
                    {
                        Screen.UpdateScreenContent(["Oops. Something went wrong on the database while creating the note."]);
                        return;
                    }

                }
                else
                {
                    note = NoteDAO.UpdateNote(note)!;

                    if(note is null)
                    {
                        Screen.UpdateScreenContent(["Oops. Something went wrong on the database while updating the note."]);
                        return;
                    }
                }

                if(exitCode == TextEditor.ExitCode.SAVE) loopMessage = "Note saved.";
            }
        }
    }
}