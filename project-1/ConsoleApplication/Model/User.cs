namespace Program.Model;

using System.ComponentModel.DataAnnotations;


public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public String name { get; set; } = null!;
    [Required]
    public String password { get; set; } = null!;
    public ICollection<Todo> Todos { get; set; } = null!;
}