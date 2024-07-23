using Program.Data;
using Program.Model;
using Program.Utils;

namespace Program.DAO;
public class NoteDAO
{
    // Retrieves all the notes of a given user.
    public List<Note> GetAllNotesByUserName(String username)
    {        
        DatabaseContext c = Connection.Get();

        return c.Notes.Where(u => u.User.Name == username).ToList();
    }

    // Adds a note for the given user.
    public Note? AddNote(String title, String contents, User user)
    {
        // The only requirement we have for adding a note is that it needs to have a title.
        if(title == null || title.Length == 0 || user is null) return null;

        DatabaseContext c = Connection.Get();

        Note newNote = new Note{Title = title, Content = contents, User = user};

        c.Notes.Add(newNote);
        c.SaveChanges();

        return newNote;
    }

    // Attempts to find a note by Id, null if none.
    public Note? GetNoteById(int noteId)
    {
        DatabaseContext c = Connection.Get();

        return c.Notes.Find(noteId);
    }

    // Attempts to find a note by title, null if none.
    public Note? GetNoteByTitle(String noteTitle)
    {
        DatabaseContext c = Connection.Get();
        
        return c.Notes.FirstOrDefault<Note>(n => n.Title == noteTitle);
    }

    // Attempts to update a note by Id.
    public Note? UpdateNote(Note note)
    {
        // The only requirement we have for adding a note is that it needs to have a title.
        if(note.Title == null || note.Title.Length == 0) return null;

        DatabaseContext c = Connection.Get();

        c.Notes.Update(note);
        c.SaveChanges();

        return note;
    }
}