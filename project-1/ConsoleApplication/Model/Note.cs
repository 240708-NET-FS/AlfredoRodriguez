namespace Program.Model;

using System.ComponentModel.DataAnnotations;


public class Note
{
    public int Id { get; set; }
    public String Title { get; set; } = null!;
    public String? Content { get; set; } = null;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}