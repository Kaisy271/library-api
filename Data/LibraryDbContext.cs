using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data
{
    public partial class LibraryDbContext : DbContext
    {
        public LibraryDbContext()
        {
        }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Borrowrecord> Borrowrecords { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<LoginRequest> LoginRequest { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {

                entity.ToTable("books");

                entity.Property(e => e.Author).HasMaxLength(100);

                entity.Property(e => e.Id).HasColumnType("int(10)");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(20)
                    .HasColumnName("ISBN");

                entity.Property(e => e.PublishYear).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<Borrowrecord>(entity =>
            {
                entity.ToTable("borrowrecords");

                entity.Property(e => e.Id).HasColumnType("int(10)");

                entity.Property(e => e.BookId).HasColumnType("int(10)");

                entity.Property(e => e.BorrowDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(10)");
            });
            //modelBuilder.Entity<BorrowRecord>()
            //.HasOne(b => b.User)
            //.WithMany()
            //.HasForeignKey(b => b.UserId);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnType("int(10)");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Username).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
