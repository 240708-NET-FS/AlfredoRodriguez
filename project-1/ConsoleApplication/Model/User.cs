namespace Program.Model;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class User
{
    public int Id { get; set; }
    public String name { get; set; } = null!;
    public String password { get; set; } = null!;
    public ICollection<Todo> Todos { get; set; } = null!;
}