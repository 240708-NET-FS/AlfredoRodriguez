namespace Program.Utils;

// Attribute used with an attribute's child class that allows you to define some properties for the attribute class you are trying to define.
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class Command : Attribute
{
    public enum CommandContext
    {
        HOME,
        NOTE,
        ANY,
        EXIT
    };
    public String Name { get; }
    public CommandContext Context { get; }

    public String? Description { get; } = null;

    public Command(String name, String? description = null, CommandContext context = CommandContext.HOME)
    {
        Context = context;
        Name = name;
        Description = description;
    }
}