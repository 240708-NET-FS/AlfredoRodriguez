namespace Program.Utils;

// Attribute used with an attribute's child class that allows you to define some properties for the attribute class you are trying to define.
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class Command : Attribute
{
    public String Name { get; }
    public String? Description { get; } = null;

    public Command(String name, String description)
    {
        Name = name;
        Description = description;
    }

    public Command(String name)
    {
        Name = name;
    }
}