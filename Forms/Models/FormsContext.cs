using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Forms.Models
{
    public partial class FormsContext : DbContext
    {
        public FormsContext()
        {
        }

        public FormsContext(DbContextOptions<FormsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attack> Attacks { get; set; }
        public virtual DbSet<ContentProvider> ContentProviders { get; set; }
        public virtual DbSet<CyberAttack> CyberAttacks { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<Purpose> Purposes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=Forms;Pooling=true; Connection Lifetime=0;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_Turkey.1254");

            modelBuilder.Entity<Attack>(entity =>
            {
                entity.ToTable("attacks");

                entity.Property(e => e.AttackId)
                    .HasColumnName("attackId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AttackName)
                    .HasMaxLength(50)
                    .HasColumnName("attackName");
            });

            modelBuilder.Entity<ContentProvider>(entity =>
            {
                entity.ToTable("contentProviders");

                entity.Property(e => e.ContentProviderId)
                    .HasColumnName("contentProviderId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.DatabasePassword)
                    .HasMaxLength(50)
                    .HasColumnName("databasePassword");

                entity.Property(e => e.DatabaseRequest).HasColumnName("databaseRequest");

                entity.Property(e => e.DatabaseUserName)
                    .HasMaxLength(30)
                    .HasColumnName("databaseUserName");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.DomainName)
                    .HasMaxLength(100)
                    .HasColumnName("domainName");

                entity.Property(e => e.PurposeId).HasColumnName("purposeId");

                entity.Property(e => e.UserId).HasColumnName("userId");
                entity.Property(e => e.AuthorizedId).HasColumnName("authorizedId");

                entity.HasOne(d => d.Purpose)
                    .WithMany(p => p.ContentProviders)
                    .HasForeignKey(d => d.PurposeId)
                    .HasConstraintName("purpose");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ContentProviders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("user");
            });

            modelBuilder.Entity<CyberAttack>(entity =>
            {
                entity.HasKey(e => e.CybetAttackId)
                    .HasName("cyberAttacks_pkey");

                entity.ToTable("cyberAttacks");

                entity.Property(e => e.CybetAttackId)
                    .HasColumnName("cybetAttackId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AttackId).HasColumnName("attackId");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.DetectionDate)
                    .HasColumnType("date")
                    .HasColumnName("detectionDate");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.Property(e => e.SystemOutage).HasColumnName("systemOutage");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Attack)
                    .WithMany(p => p.CyberAttacks)
                    .HasForeignKey(d => d.AttackId)
                    .HasConstraintName("attack");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CyberAttacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("user");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.ToTable("forms");

                entity.Property(e => e.FormId)
                    .HasColumnName("formId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.FormName)
                    .HasMaxLength(50)
                    .HasColumnName("formName");

                entity.Property(e => e.FormUrl)
                    .HasMaxLength(100)
                    .HasColumnName("formUrl");

                entity.Property(e => e.Passive).HasColumnName("passive");

                entity.Property(e => e.Sira).HasColumnName("sira");

                entity.Property(e => e.UstId).HasColumnName("ustId");
            });

            modelBuilder.Entity<Purpose>(entity =>
            {
                entity.ToTable("purposes");

                entity.Property(e => e.PurposeId)
                    .HasColumnName("purposeId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.PurposeName)
                    .HasMaxLength(50)
                    .HasColumnName("purposeName");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Authority).HasColumnName("authority");

                entity.Property(e => e.Passive).HasColumnName("passive");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(50)
                    .HasColumnName("userEmail");

                entity.Property(e => e.UserMobilePhone)
                    .HasMaxLength(20)
                    .HasColumnName("userMobilePhone");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(50)
                    .HasColumnName("userPassword");

                entity.Property(e => e.UserPhone)
                    .HasMaxLength(20)
                    .HasColumnName("userPhone");

                entity.Property(e => e.UserSurname)
                    .HasMaxLength(50)
                    .HasColumnName("userSurname");

                entity.Property(e => e.UserTask)
                    .HasMaxLength(20)
                    .HasColumnName("userTask");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
