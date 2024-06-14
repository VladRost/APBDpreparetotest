using APBDPrepare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APBDPrepare.Context;

public class BoatDbContext: DbContext
{
    public BoatDbContext()
    {
        
    }

    public BoatDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<BoatStandard> BoatStandards { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientCategory> ClientCategories { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Sailboat> Sailboats { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=master;User Id=sa;Password=SDFis2394Sfns;Trusted_Connection=False;Encrypt=False;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoatStandard>(opt =>
            {
                opt.HasKey(e => e.IdBoatStandard);
                opt.Property(e => e.Name).HasMaxLength(100).IsRequired();
                opt.HasMany(e => e.Reservations).WithOne(e => e.BoatStandard).HasForeignKey(e => e.IdBoatStandard).OnDelete(DeleteBehavior.NoAction);
            }
            );
        modelBuilder.Entity<Client>(opt =>
        {
            opt.HasKey(e => e.IdClient);
            opt.Property(e => e.Name).HasMaxLength(100).IsRequired();
            opt.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            opt.Property(e => e.Pesel).HasMaxLength(100).IsRequired();
            opt.Property(e => e.Email).HasMaxLength(100).IsRequired();
        });
        modelBuilder.Entity<ClientCategory>(opt =>
        {
            opt.HasKey(e => e.IdClientCategory);
            opt.Property(e => e.Name).HasMaxLength(100).IsRequired();
            opt.HasMany(e => e.Clients).WithOne(e => e.ClientCategory).HasForeignKey(e => e.IdClientCategory);
            
        });
        modelBuilder.Entity<Reservation>(opt =>
        {
            opt.HasKey(e => e.IdReservation);
            opt.Property(e => e.IdReservation).ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy",
                SqlServerValueGenerationStrategy.IdentityColumn);
            opt.Property(e => e.CancelReason).HasMaxLength(200);
            opt.HasOne(e => e.Client).WithMany(e => e.Reservations).HasForeignKey(e => e.IdClient);
            opt.HasMany(e => e.Sailboats).WithMany(e => e.Reservations);
        });
        modelBuilder.Entity<Sailboat>(opt =>
        {
            opt.HasKey(e => e.IdSailboat);
            opt.Property(e => e.Name).HasMaxLength(100).IsRequired();
            opt.Property(e => e.Description).HasMaxLength(100).IsRequired();
            opt.HasOne(e => e.BoatStandard).WithMany(e => e.Sailboats).HasForeignKey(e => e.IdBoatStandard);
        });

    }
}