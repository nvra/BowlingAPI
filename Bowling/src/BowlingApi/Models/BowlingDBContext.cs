using System;
using BowlingApi.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BowlingApi.Models
{
    public partial class BowlingDBContext : DbContext
    {
        public BowlingDBContext()
        {
        }

        public BowlingDBContext(DbContextOptions<BowlingDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Framescores> Framescores { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<Indivdualscore> Indivdualscore { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<GetScoresByGameResponse> GetScoresByGameResponse { get; set; }
        public virtual DbSet<SpResponse> SpResponse { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Server=(localdb)\\v11.0;Database=BowlingDB;Trusted_Connection=True;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Framescores>(entity =>
            {
                entity.ToTable("FRAMESCORES");

                entity.HasIndex(e => new { e.GameId, e.FrameNum })
                    .HasName("UNQ_SCORE")
                    .IsUnique();

                entity.Property(e => e.GameId).HasColumnName("Game_Id");

                entity.Property(e => e.TotalScore).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Framescores)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK__FRAMESCOR__Game___267ABA7A");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("GAME");

                entity.Property(e => e.PlayerId).HasColumnName("Player_Id");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK__GAME__Player_Id__1273C1CD");
            });

            modelBuilder.Entity<Indivdualscore>(entity =>
            {
                entity.HasKey(e => new { e.GameFrameId, e.ThrowNum })
                    .HasName("PK_SCORE");

                entity.ToTable("INDIVDUALSCORE");

                entity.Property(e => e.GameFrameId).HasColumnName("GameFrame_Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IsFoul).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSpare).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsStrike).HasDefaultValueSql("((0))");

                entity.Property(e => e.Score).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.GameFrame)
                    .WithMany(p => p.Indivdualscore)
                    .HasForeignKey(d => d.GameFrameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INDIVDUAL__GameF__2B3F6F97");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("PLAYER");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetScoresByGameResponse>().HasNoKey();
            modelBuilder.Entity<SpResponse>().HasNoKey();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
