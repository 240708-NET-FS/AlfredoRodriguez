namespace Program.DAO;

using Program.Data;
using Program.Model;

public class NoteDAO
{
    // Retrieves all the notes of a given user.
    public List<Note> GetAllNotesByUserName(String username)
    {
        if(username is null || username.Length == 0) throw new ArgumentException("Invalid username");

        DatabaseContext c = Connection.Get();

        //return c.Notes.Where(u => u.User.Name == username).ToList();
        List<Note> notes = c.Notes.Where(u => u.User.Name == username).ToList();

        foreach(var i in notes) c.Entry(i).Reload();
        
        return notes;
    }

    // Adds a note for the given user.
    public Note? AddNote(String title, String contents, User user)
    {
        // The only requirement we have for adding a note is that it needs to have a title.
        if(title == null || title.Length == 0 || user is null) throw new ArgumentException("Invalid note.");

        DatabaseContext c = Connection.Get();

        Note newNote = new Note{Title = title, Content = contents, User = user};

        c.Notes.Add(newNote);
        c.SaveChanges();

        return newNote;
    }

    // Attempts to find a note by Id, null if none.
    // Optionally lets you filter by User.
    public Note? GetNoteById(int noteId, User user = null!)
    {
        DatabaseContext c = Connection.Get();

        if(user is null)
            return c.Notes.Find(noteId);
        
        //return c.Notes.FirstOrDefault(n => n.Id == noteId && n.User == user);
        Note? n = c.Notes.FirstOrDefault(n => n.Id == noteId && n.User == user);

        if(n != null) c.Entry(n).Reload();

        return n;
    }

    // Attempts to find a note by title, null if none.
    // Optionally lets you filter by User.
    public Note? GetNoteByTitle(String noteTitle, User user = null!)
    {
        if(noteTitle is null || noteTitle.Length == 0) throw new ArgumentException("Invalid note title");

        DatabaseContext c = Connection.Get();
        
        //return c.Notes.FirstOrDefault<Note>(n => n.Title.Equals(noteTitle) && (user != null ? n.User == user : true));
        Note? n = c.Notes.FirstOrDefault<Note>(n => n.Title.Equals(noteTitle) && (user != null ? n.User == user : true));

        // Because I am using the same context, I need to reload the ent to discard unsaved changed to the object.
        if(n != null) c.Entry(n).Reload();
        
        return n;
    }

    // Attempts to update a note by Id.
    public Note? UpdateNote(Note note)
    {
        if(note == null || note.Title is null || note.User is null) throw new ArgumentException("Invalid note.");

        // The only requirement we have for adding a note is that it needs to have a title.
        if(note.Title == null || note.Title.Length == 0) return null;

        DatabaseContext c = Connection.Get();

        c.Notes.Update(note);
        c.SaveChanges();

        return note;
    }

    public Note? RemoveNoteByTitle(String title, User user = null!)
    {
        if(title is null || title.Length == 0) throw new ArgumentException("Invalid note title");

        // Get connection
        DatabaseContext c = Connection.Get();

        // Find note to remove
        Note? toRemove = c.Notes.FirstOrDefault(n => n.Title == title && (user != null ? n.User == user : true));

        // If it doesnt exists, return null
        if(toRemove is null) return null;

        // If it does exists, then remove it
        c.Notes.Remove(toRemove);

        // Persist changes
        c.SaveChanges();

        // Return just removed noted
        return toRemove;
    }

    public Note? RemoveNoteById(int noteId, User user = null!)
    {
        // Get connection
        DatabaseContext c = Connection.Get();

        // Find note to remove
        Note? toRemove = c.Notes.FirstOrDefault(n => n.Id == noteId && (user != null ? n.User == user : true));
            
        // If it doesnt exists, return null
        if(toRemove is null) return null;

        // If it does exists, then remove it
        c.Notes.Remove(toRemove);

        // Persist changes
        c.SaveChanges();

        // Return just removed noted
        return toRemove;
    }
}