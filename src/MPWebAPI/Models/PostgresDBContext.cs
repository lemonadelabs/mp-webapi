using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenIddict;

namespace MPWebAPI.Models
{
    public class PostgresDBContext : OpenIddictDbContext<MerlinPlanUser>
    {
        public PostgresDBContext(DbContextOptions<PostgresDBContext> options) :base(options)
        {
        }

        // public DbSet<TodoList> TodoLists { get; set; }
        // public DbSet<TodoItem> TodoItems { get; set; }

        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     // relations
        //     builder.Entity<TodoList>().HasKey(l => l.TodoListId);
        //     builder.Entity<TodoList>()
        //         .HasMany(l => l.Items)
        //         .WithOne();

        //     // property constraints
        //     builder.Entity<TodoList>()
        //         .Property(l => l.Name)
        //         .HasMaxLength(250)
        //         .HasDefaultValue("New List");
            
        //     builder.Entity<TodoItem>()
        //         .Property(i => i.ItemText)
        //         .HasMaxLength(500)
        //         .HasDefaultValue("New Item");

        //     builder.Entity<TodoItem>()
        //         .Property(i => i.Completed)
        //         .HasDefaultValue(false);

        //     base.OnModelCreating(builder);
        // }
    }
}
