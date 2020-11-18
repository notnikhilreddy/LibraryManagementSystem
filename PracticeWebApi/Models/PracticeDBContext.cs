using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PracticeWebApi.Models
{
    public partial class PracticeDBContext : DbContext
    {
        public PracticeDBContext()
        {
        }

        public PracticeDBContext(DbContextOptions<PracticeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookLibraryAssociation> BookLibraryAssociation { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Library> Library { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserBookAssociation> UserBookAssociation { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Users> BookWithStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=mysqlpracticeserver.database.windows.net;Database=PracticeDB;User ID=notnikhilreddy; Password=Pa$$w0rd;Integrated Security=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookLibraryAssociation>(entity =>
            {
                entity.HasKey(e => e.BookLibraryAsscId)
                    .HasName("PK__bookLibr__68E7DFF61C4E832C");

                entity.ToTable("bookLibraryAssociation");

                entity.Property(e => e.BookLibraryAsscId).HasColumnName("bookLibraryAsscID");

                entity.Property(e => e.BookId).HasColumnName("bookID");

                entity.Property(e => e.IsAvailable).HasColumnName("isAvailable");

                entity.Property(e => e.IsCheckedOut).HasColumnName("isCheckedOut");

                entity.Property(e => e.LibraryId).HasColumnName("libraryID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookLibraryAssociation)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__bookLibra__bookI__656C112C");

                entity.HasOne(d => d.Library)
                    .WithMany(p => p.BookLibraryAssociation)
                    .HasForeignKey(d => d.LibraryId)
                    .HasConstraintName("FK__bookLibra__libra__66603565");
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasKey(e => e.BookId)
                    .HasName("PK__books__8BE5A12D8D248B28");

                entity.ToTable("books");

                entity.Property(e => e.BookId).HasColumnName("bookID");

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Genre)
                    .HasColumnName("genre")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Library>(entity =>
            {
                entity.ToTable("library");

                entity.Property(e => e.LibraryId).HasColumnName("libraryID");

                entity.Property(e => e.LocationId).HasColumnName("locationID");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__roles__CD98460AFDCD632B");

                entity.ToTable("roles");

                entity.Property(e => e.RoleId).HasColumnName("roleID");

                entity.Property(e => e.RoleName)
                    .HasColumnName("roleName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserBookAssociation>(entity =>
            {
                entity.HasKey(e => e.UserBookAsscId)
                    .HasName("PK__userBook__6608C8891123319D");

                entity.ToTable("userBookAssociation");

                entity.Property(e => e.UserBookAsscId).HasColumnName("userBookAsscID");

                entity.Property(e => e.BookLibraryAsscId).HasColumnName("bookLibraryAsscID");

                entity.Property(e => e.DueDate)
                    .HasColumnName("dueDate")
                    .HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.BookLibraryAssc)
                    .WithMany(p => p.UserBookAssociation)
                    .HasForeignKey(d => d.BookLibraryAsscId)
                    .HasConstraintName("FK__userBookA__bookL__6A30C649");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBookAssociation)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__userBookA__userI__693CA210");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__users__CB9A1CDFEAA2A215");

                entity.ToTable("users");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.LocationId).HasColumnName("locationID");

                entity.Property(e => e.RoleId).HasColumnName("roleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__users__roleID__628FA481");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
