namespace Program.Utils;

// Holds data about the current session.
public class Session
{
    private static Session SessionInstance = null;
    public static Session GetInstance()
    {
        if(SessionInstance == null)
        {
            SessionInstance = new Session();
        }

        return SessionInstance;
    }

    public String[] Content { get;set; } = null;
    public ConsoleColor[] ContentColor { get; set; } = null;
    public String User { get; set; } = null;
}