using Microsoft.EntityFrameworkCore;

namespace Nodes.EfDbContext.Entities
{
    public sealed class NodesDbContext : DbContext
    {
        public NodesDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Edges> Edges { get; set; }
        public DbSet<Nodes> Nodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Edges>(entity => {
                entity.Property(e => e.Value).IsRequired();

                entity.Property(e => e.Version)
                      .IsRequired()
                      .IsRowVersion();

                entity.HasOne(d => d.FromNode)
                      .WithMany(p => p.EdgesFromNode)
                      .HasForeignKey(d => d.FromNodeId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_dbo.Edges_FromNodeId");

                entity.HasOne(d => d.ToNode)
                      .WithMany(p => p.EdgesToNode)
                      .HasForeignKey(d => d.ToNodeId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_dbo.Edges_ToNodeId");
            });

            modelBuilder.Entity<Nodes>(entity => {
                entity.HasIndex(e => new { e.Value, e.Uid })
                      .HasName("UX_Nodes_Uid")
                      .IsUnique();

                entity.Property(e => e.Sha256)
                      .IsRequired()
                      .HasMaxLength(256)
                      .IsFixedLength();

                entity.Property(e => e.Uid)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Value)
                      .IsRequired()
                      .HasColumnType("nvarchar(max)");

                entity.Property(e => e.Version)
                      .IsRequired()
                      .IsRowVersion();
            });
        }
    }
}
