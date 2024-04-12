using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public partial class RobotContext : DbContext
{
    public RobotContext()
    {
    }

    public RobotContext(DbContextOptions<RobotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<Robotcommand> Robotcommands { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=sit331;Username=postgres;Password=password");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_map");

            entity.ToTable("map");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Columns).HasColumnName("columns");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description)
                .HasMaxLength(800)
                .HasColumnName("description");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Rows).HasColumnName("rows");
        });

        modelBuilder.Entity<Robotcommand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_robotcommand");

            entity.ToTable("robotcommand");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description)
                .HasMaxLength(800)
                .HasColumnName("description");
            entity.Property(e => e.Ismovecommand).HasColumnName("ismovecommand");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
