using ElmiraFireRecall.Models;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElmiraFireRecall.Data
{
    public class FireDBContext : DbContext
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEncryptionProvider _provider;

        public FireDBContext(DbContextOptions<FireDBContext> options, IHttpContextAccessor httpContextAccessor)
    : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            this._provider = new GenerateEncryptionProvider("27b193e05548fccbfbeef58426bb44f1");
        }

        public FireDBContext(DbContextOptions<FireDBContext> options) : base(options) {
            this._provider = new GenerateEncryptionProvider("27b193e05548fccbfbeef58426bb44f1");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseEncryption(this._provider);

            builder.Entity<FireRecipient>()
                .HasMany(g => g.FireGroups)
                .WithMany(r => r.Recipients)
                .UsingEntity("FireGroupFireRecipient",
                    l => l.HasOne(typeof(FireGroup)).WithMany().HasForeignKey("GroupsId").HasPrincipalKey(nameof(FireGroup.Id)),
                    r => r.HasOne(typeof(FireRecipient)).WithMany().HasForeignKey("RecipientsId").HasPrincipalKey(nameof(FireRecipient.Id)));


            builder.Entity<FireRecipient>().Navigation(g => g.FireGroups).AutoInclude();

        }


        public DbSet<FireGroup> Groups { get; set; } = default!;
        public DbSet<FireRecipient> Recipients { get; set; } = default!;
        public DbSet<MessageHistory> MessageHistory { get; set; } = default!;
        public DbSet<MessageType> MessageTypes { get; set; } = default!;
        public DbSet<PhoneProvider> PhoneProviders { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<FireGroupFireRecipient> GroupRecipientLink { get; set; } = default!;




        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
                        bool acceptAllChangesOnSuccess, 
                        CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var now = DateTime.Now;

            foreach(var entry in entries )
            {
                if(entry.Entity is BaseEntity trackable)
                {
                    switch(entry.State)
                    {
                        case EntityState.Modified:
                            trackable.Updated = now;
                            entry.Property("Created").IsModified = false;
                        break;
                        case EntityState.Added:
                            trackable.Created = now;
                            trackable.Updated = now;
                        break;
                    }
                }
            }
        }
    }
}
