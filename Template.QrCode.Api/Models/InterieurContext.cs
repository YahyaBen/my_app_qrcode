using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Template.QrCode.Api.ClassesDB;

namespace DAL.ClassesDB
{
    public partial class InterieurContext : DbContext
    {
        public InterieurContext()
        {
        }

        public InterieurContext(DbContextOptions<InterieurContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Certificats> Certificats { get; set; }
        public virtual DbSet<CodeServices> CodeServices { get; set; }
        public virtual DbSet<TraceMobileApp> TraceMobileApp { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=.;initial catalog=Interieur;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificats>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateNextSynchronization)
                    .HasColumnName("dateNextSynchronization")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dateinsertion)
                    .HasColumnName("dateinsertion")
                    .HasColumnType("datetime");

                entity.Property(e => e.PrivateKey).HasColumnName("privateKey");

                entity.Property(e => e.PublicKey).HasColumnName("publicKey");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<CodeServices>(entity =>
            {
                entity.HasKey(e => e.CodeService);

                entity.Property(e => e.CodeService)
                    .HasColumnName("codeService")
                    .ValueGeneratedNever();

                entity.Property(e => e.Libelle)
                    .HasColumnName("libelle")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TraceMobileApp>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateInsertion)
                    .HasColumnName("dateInsertion")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateNextSynchronization)
                    .HasColumnName("dateNextSynchronization")
                    .HasColumnType("datetime");

                entity.Property(e => e.KeyDecrypt)
                    .HasColumnName("keyDecrypt")
                    .HasMaxLength(2000);

                entity.Property(e => e.KeySign)
                    .HasColumnName("keySign")
                    .HasMaxLength(2000);

                entity.Property(e => e.LoginId)
                    .HasColumnName("loginId")
                    .HasMaxLength(50);

                entity.Property(e => e.Resultat).HasColumnName("resultat");

                entity.Property(e => e.Service).HasColumnName("service");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasMaxLength(2000);

                entity.HasOne(d => d.ServiceNavigation)
                    .WithMany(p => p.TraceMobileApp)
                    .HasForeignKey(d => d.Service)
                    .HasConstraintName("FK_TraceWebApiEchange_CodeServices");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
