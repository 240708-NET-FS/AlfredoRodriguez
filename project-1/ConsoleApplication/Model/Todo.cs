namespace Program.Model;

using System.ComponentModel.DataAnnotations;


public class Todo
{
    public int Id { get; set; }
    public String content { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}