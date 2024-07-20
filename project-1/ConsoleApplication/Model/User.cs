namespace Program.Model;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class User
{
    public int Id { get; set; }
    public String Name { get; set; } = null!;
    public String Password { get; set; } = null!;
    public ICollection<Note> Todos { get; set; } = null!;
}