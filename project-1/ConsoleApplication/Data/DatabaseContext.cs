namespace Program.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Program.Model;


public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get a configuration object.
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("config.json")
        .Build();

        // Tell the DbContext where to find the connection string.
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions => {

            // The line below Allows me to retry on failure.
            // Basically this handles any SQL exception thrown due to a "transient failure", which involve:
            // Database timeout (this happens to me every time I try to interact with the DB after not having done so in a while)
            // Network issues (connection lost or interrupted)
            // Service unavailability (any temporal error happening on the Azure side)
            // Rate limiting (Hitting a temporary rate limit imposed by Azure, if any. I imagine our trial accounts have those.)
            sqlOptions.EnableRetryOnFailure();
        });
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
        modelBuilder.Entity<User>().Property(ent => ent.Name).IsRequired();
        modelBuilder.Entity<User>().Property(ent => ent.Password).IsRequired();


        // NOTE table set up
        //------------------
        // Id: Make it a primary key.
        modelBuilder.Entity<Note>().HasKey(ent => ent.Id);
        // Id: Make it autoincrement on add.
        modelBuilder.Entity<Note>().Property(ent => ent.Id).ValueGeneratedOnAdd();
        // Content and UserId: Make them required.
        modelBuilder.Entity<Note>().Property(ent => ent.Title).IsRequired();
        modelBuilder.Entity<Note>().Property(ent => ent.UserId).IsRequired();

        // Create non-clustered index on UserId.
        modelBuilder.Entity<Note>().HasIndex(ent => ent.UserId);


        // Table relationships:
        //---------------------
        modelBuilder.Entity<Note>()
        .HasOne(ent => ent.User)
        .WithMany(parent => parent.Notes)
        .HasForeignKey(ent => ent.UserId)
        // This last line will allow a user to be deleted and further more, it will delete its own records assossiated with it. Which is exactly what we want.
        // The reason this is set within the child in the relationship is because not all tables dependant on the same primary key might be OK with this. So
        // It is a per-child thing.
        .OnDelete(DeleteBehavior.Cascade);
    }
}