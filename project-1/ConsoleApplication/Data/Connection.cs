namespace Program.Data;

// Since this is an application that only ever performs one query at a time, we can make all the classes
// use the same single DbContext object.
// With a singleton connection, we ensure that if we ever forget to close the connection, the singleton does it
// automatically at the end of the life of the program.
public class Connection
{
    static private DatabaseContext? dbContext = null;

    private Connection(){}

    // Ensures the connection gets closed once the app exits.
    ~Connection()
    {
        dbContext?.Dispose();
    }

    static public DatabaseContext Get()
    {
        if(dbContext is null)
        {
            dbContext = new DatabaseContext();
        }

        return dbContext;
    }

    // In case we want to disconnect manually (for the logout and deleteme commands)
    static public void Dispose()
    {
        dbContext?.Dispose();
        dbContext = null;
    }
}