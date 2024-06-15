using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Primal.Entities;

namespace Primal
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) :
            base(options)
        {
        }

        private string Serialize<T>(T x)
        {
            return JsonSerializer.Serialize(x, (JsonSerializerOptions)null);
        }

        private Dictionary<T, int> Deserialize<T>(string x)
        {
            return JsonSerializer.Deserialize<Dictionary<T, int>>(x, (JsonSerializerOptions)null);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured) options.UseSqlite("Data Source=Primal.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Boss>()
                .Property(e => e.Health)
                .HasConversion(x => Serialize(x), x => Deserialize<string>(x));

            modelBuilder
                .Entity<Boss>()
                .Property(e => e.Might)
                .HasConversion(x => Serialize(x), x => Deserialize<Might>(x));

            modelBuilder
                .Entity<Minion>()
                .Property(e => e.Might)
                .HasConversion(x => Serialize(x), x => Deserialize<Might>(x));

            modelBuilder
                .Entity<EncounterPlayer>()
                .Property(e => e.Tokens)
                .HasConversion(x => Serialize(x), x => Deserialize<Token>(x));

            modelBuilder
                .Entity<Player>()
                .Property(e => e.Might)
                .HasConversion(x => Serialize(x), x => Deserialize<Might>(x));

            modelBuilder
                .Entity<Item>()
                .Property(e => e.Might)
                .HasConversion(x => Serialize(x), x => Deserialize<Might>(x));
        }

        public DbSet<Attack> Attacks { get; set; }
        public DbSet<AttackMinion> AttackMinions { get; set; }
        public DbSet<Boss> Bosses { get; set; }
        public DbSet<BossAction> BossActions { get; set; }
        public DbSet<BossAttack> BossAttacks { get; set; }
        public DbSet<BossAttackPlayer> BossAttackPlayers { get; set; }
        public DbSet<Encounter> Encounters { get; set; }
        public DbSet<EncounterMightDeck> EncounterMightDecks { get; set; }
        public DbSet<EncounterPlayer> EncounterPlayers { get; set; }
        public DbSet<FreeCompany> FreeCompanies { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MightCard> MightCards { get; set; }
        public DbSet<Minion> Minons { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerAbility> PlayerAbilities { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
