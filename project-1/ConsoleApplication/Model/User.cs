namespace Program.Model;

public class User
{
    public int Id { get; set; }
    public String Name { get; set; } = null!;
    public String Password { get; set; } = null!;
    //virtual public ICollection<Note> Notes { get; set; } = new List<Note>();
    public ICollection<Note> Notes { get; set; } = null!;
}