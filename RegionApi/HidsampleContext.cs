using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RegionApi;

public partial class HidsampleContext : DbContext
{
    public HidsampleContext() { }

    public HidsampleContext(DbContextOptions<HidsampleContext> options)
        : base(options) { }

    public virtual DbSet<Region> Regions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).IsClustered(false);

            entity.HasIndex(e => e.Name, "IX_Regions_Name");

            entity.Property(e => e.RegionId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.ToTable("Regions", tb => tb.ExcludeFromMigrations());
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
