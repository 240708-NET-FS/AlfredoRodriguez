namespace Program.Utils;

using Program.Model;

// Holds data about the current session.
public class Session
{
    // Fields
    private static Session SessionInstance = null!;
    public User? User { get; set; } = null;
    public Command.CommandContext CommandContext { get; set; } = Command.CommandContext.HOME;

    // Singleton
    public static Session GetInstance()
    {
        if(SessionInstance == null)
        {
            SessionInstance = new Session();
        }

        return SessionInstance;
    }


}