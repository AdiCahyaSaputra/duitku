using Microsoft.EntityFrameworkCore;
using DuitKu.Domain;

namespace DuitKu.Persistance.Database
{
    public class ApplicationDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Transaction> Transactions { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Account> Account { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<SubCategory> SubCategory { get; set; } = default!;

        public ApplicationDBContext(
            IConfiguration configuration,
            DbContextOptions<ApplicationDBContext> options,
            ILogger<ApplicationDBContext> logger
        ) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(BaseDomain).IsAssignableFrom(t.ClrType));

            foreach (var entityType in entityTypes)
            {
                var idProperty = entityType.ClrType.GetProperty("Id");

                if (idProperty != null && idProperty.PropertyType == typeof(Guid))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("Id")
                        .HasDefaultValueSql("gen_random_uuid()");
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var _configurationString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(_configurationString);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseDomain>();

            foreach (var entry in entries)
            {
                var tableName = entry.Metadata.GetTableName();

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}