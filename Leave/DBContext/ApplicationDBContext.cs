using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Leave.DBContext;

public partial class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AllLeaf> AllLeaves { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllLeaf>(entity =>
        {
            entity.HasKey(e => e.LeaveId);

            entity.ToTable("All_Leaves");

            entity.Property(e => e.LeaveId).HasColumnName("LeaveID");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.AllLeaves)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AllLeaves_UserID_Users_UserID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
