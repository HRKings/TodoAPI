using System;
using Core.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Infrastructure.Database.Context
{
    public partial class PostgresContext : DbContext
    {
        public PostgresContext()
        {
        }

        public PostgresContext(DbContextOptions<PostgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE") 
                                         ?? "Host=localhost;Database=postgres;Username=postgres;Password=password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.ToTable("todos");
                
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .UseSerialColumn();
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("name");
                
                entity.Property(e => e.Completed).HasColumnName("completed");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
