namespace Program.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Program.Model;


public class DatabaseContext : DbContext
{
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Todo> Todo { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get a configuration object.
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("config.json")
        .Build();

        // Tell the DbContext where to find the connection string.
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // USER table set up
        //------------------
        // Id: Make it a primary key.
        modelBuilder.Entity<User>().HasKey(ent => ent.Id);
        // Id: Make it autoincrement on add.
        modelBuilder.Entity<User>().Property(ent => ent.Id).ValueGeneratedOnAdd();
        // Name and Password: Make them required.
        modelBuilder.Entity<User>().Property(ent => ent.name).IsRequired();
        modelBuilder.Entity<User>().Property(ent => ent.password).IsRequired();


        // TODO table set up
        //------------------
        // Id: Make it a primary key.
        modelBuilder.Entity<Todo>().HasKey(ent => ent.Id);
        // Id: Make it autoincrement on add.
        modelBuilder.Entity<Todo>().Property(ent => ent.Id).ValueGeneratedOnAdd();
        // Content and UserId: Make them required.
        modelBuilder.Entity<Todo>().Property(ent => ent.content).IsRequired();
        modelBuilder.Entity<Todo>().Property(ent => ent.UserId).IsRequired();

        // Create non-clustered index on UserId.
        modelBuilder.Entity<Todo>().HasIndex(ent => ent.UserId);

        // Table relationships:
        //---------------------
        modelBuilder.Entity<Todo>()
        .HasOne(ent => ent.User)
        .WithMany(parent => parent.Todos)
        .HasForeignKey(ent => ent.UserId);
    }
}