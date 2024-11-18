using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace SRMAgreement.Data_Base
{
    public class DataBaseContext : DbContext
    {
        public DbSet<_1D> D1 { get; set; }
        public DbSet<_2D> D2 { get; set; }
        public DbSet<_3D> D3 { get; set; }
        public DbSet<_4D> D4 { get; set; }
        public DbSet<_5D> D5 { get; set; }
        public DbSet<PathToFilesGuard> pathToFilesGuard { get; set; }

        public DbSet<Status> status { get; set; }
        public DbSet<_6D> D6 { get; set; }

        public DbSet<_4DBook> D4Bookk { get; set; }
        public DbSet<SubleaseDop> SubleaseDop { get; set; }

        public DbSet<PdfFilePath_Sublease> PdfFilePath_Sublease { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<_2D>()
                .HasOne(d => d.SubleaseDop)
                .WithOne(sd => sd._2D)
                .HasForeignKey<_2D>(d => d.SubleaseDopId);

            modelBuilder.Entity<_4D>()
               .HasMany(e => e.PathToPdfFiles_Sublease)
               .WithOne(e => e._4D)
               .HasForeignKey(e => e._4DId)
               .IsRequired();

            modelBuilder.Entity<_5D>()
                .HasMany(e => e.PathToFilesGuard)
                .WithOne(e => e.D5class)
                .HasForeignKey(e => e._5dId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class DataBaseArchive : DbContext
    {
        public DbSet<Archive_3D> Archive_3D { get; set; }
        public DbSet<Archive_4D> Archive_4D { get; set; }
        public DbSet<Archive_5D> Archive_5D { get; set; }
        public DbSet<Archive_6D> Archive_6D { get; set; }

        public DataBaseArchive(DbContextOptions<DataBaseArchive> options) : base(options)
        {
        }
    }

    public class DataBaseUser : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserStatus> UserStatus { get; set; }

        public DataBaseUser(DbContextOptions<DataBaseUser> options) : base(options)
        {
        }
    }

}
