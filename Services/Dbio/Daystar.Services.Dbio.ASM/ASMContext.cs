using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Jon.Services.Dbio.ASM
{
    public partial class ASMContext : DbContext
    {
        public ASMContext()
        {
        }

        public ASMContext(DbContextOptions<ASMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupPermission> GroupPermissions { get; set; }
        public virtual DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }
        public virtual DbSet<UserSessionPermission> UserSessionPermissions { get; set; }
        public virtual DbSet<vwApplicationsList> vwApplicationsLists { get; set; }
        public virtual DbSet<vwGroupPermissionsList> vwGroupPermissionsLists { get; set; }
        public virtual DbSet<vwGroupUsersList> vwGroupUsersLists { get; set; }
        public virtual DbSet<vwGroupsList> vwGroupsLists { get; set; }
        public virtual DbSet<vwPermissionsList> vwPermissionsLists { get; set; }
        public virtual DbSet<vwUserGroupsList> vwUserGroupsLists { get; set; }
        public virtual DbSet<vwUserPermissionsList> vwUserPermissionsLists { get; set; }
        public virtual DbSet<vwUserSessionPermissionsList> vwUserSessionPermissionsLists { get; set; }
        public virtual DbSet<vwUserSessionsList> vwUserSessionsLists { get; set; }
        public virtual DbSet<vwUsersList> vwUsersLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(local)\\SQLEXPRESS;Initial Catalog=ASM;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GroupPermission>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.IPAddress).HasMaxLength(50);

                entity.Property(e => e.LastAction).HasColumnType("datetime");

                entity.Property(e => e.Terminated).HasColumnType("datetime");

                entity.Property(e => e.UserAgent).HasMaxLength(200);

                entity.Property(e => e.UserData1).HasMaxLength(50);
            });

            modelBuilder.Entity<UserSessionPermission>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");
            });

            modelBuilder.Entity<vwApplicationsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwApplicationsList");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<vwGroupPermissionsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwGroupPermissionsList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<vwGroupUsersList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwGroupUsersList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<vwGroupsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwGroupsList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<vwPermissionsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwPermissionsList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<vwUserGroupsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwUserGroupsList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Expr2).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<vwUserPermissionsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwUserPermissionsList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<vwUserSessionPermissionsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwUserSessionPermissionsList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<vwUserSessionsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwUserSessionsList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.IPAddress).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LastAction).HasColumnType("datetime");

                entity.Property(e => e.Terminated).HasColumnType("datetime");

                entity.Property(e => e.UserAgent).HasMaxLength(200);
            });

            modelBuilder.Entity<vwUsersList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwUsersList");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
