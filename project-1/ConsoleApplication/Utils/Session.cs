namespace Program.Utils;

// Holds data about the current session.
public class Session
{
    // Fields
    private static Session SessionInstance = null!;
    public String? User { get; set; } = null;
    public String CommandContext { get; set; } = "HOME";

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