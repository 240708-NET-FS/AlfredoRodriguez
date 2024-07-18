namespace Program.Model;

using System.ComponentModel.DataAnnotations;


public class Todo
{
    [Key]
    public int Id { get; set; }
    [Required]
    public String content { get; set; } = null!;
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}