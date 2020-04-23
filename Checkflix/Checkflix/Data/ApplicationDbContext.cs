using Checkflix.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Production> Productions { get; set; }
        public DbSet<Vod> Vods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationUserCategory> ApplicationUserCategories { get; set; }
        public DbSet<ApplicationUserProduction> ApplicationUserProductions { get; set; }
        public DbSet<ApplicationUserVod> ApplicationUserVods { get; set; }
        public DbSet<ProductionCategory> ProductionCategories { get; set; }
        public DbSet<VodProduction> VodProductions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // seed 
            // n-n table configurations

            // AplicationUserCategories
            builder.Entity<ApplicationUserCategory>()
                .HasKey(bc => new { bc.ApplicationUserId, bc.CategoryId });
            builder.Entity<ApplicationUserCategory>()
               .HasOne(bc => bc.ApplicationUser)
               .WithMany(b => b.ApplicationUserCategories)
               .HasForeignKey(bc => bc.ApplicationUserId);
            builder.Entity<ApplicationUserCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(b => b.ApplicationUserCategories)
                .HasForeignKey(bc => bc.CategoryId);

            // ApplicationUserProductions
            builder.Entity<ApplicationUserProduction>()
               .HasKey(bc => new { bc.ApplicationUserId, bc.ProductionId });
            // default values
            builder.Entity<ApplicationUserProduction>()
                .Property(p => p.Favourites)
                .HasDefaultValue(false);
            builder.Entity<ApplicationUserProduction>()
               .Property(p => p.ToWatch)
               .HasDefaultValue(false);
            builder.Entity<ApplicationUserProduction>()
               .Property(p => p.Watched)
               .HasDefaultValue(false);

            builder.Entity<ApplicationUserProduction>()
               .HasOne(bc => bc.ApplicationUser)
               .WithMany(b => b.ApplicationUserProductions)
               .HasForeignKey(bc => bc.ApplicationUserId);
            builder.Entity<ApplicationUserProduction>()
                .HasOne(bc => bc.Production)
                .WithMany(b => b.ApplicationUserProductions)
                .HasForeignKey(bc => bc.ProductionId);

            // ApplicationUserVods
            builder.Entity<ApplicationUserVod>()
              .HasKey(bc => new { bc.ApplicationUserId, bc.VodId });
            builder.Entity<ApplicationUserVod>()
               .HasOne(bc => bc.ApplicationUser)
               .WithMany(b => b.ApplicationUserVods)
               .HasForeignKey(bc => bc.ApplicationUserId);
            builder.Entity<ApplicationUserVod>()
                .HasOne(bc => bc.Vod)
                .WithMany(b => b.ApplicationUserVods)
                .HasForeignKey(bc => bc.VodId);

            // ProductionCategories
            builder.Entity<ProductionCategory>()
             .HasKey(bc => new { bc.ProductionId, bc.CategoryId });
            builder.Entity<ProductionCategory>()
               .HasOne(bc => bc.Production)
               .WithMany(b => b.ProductionCategories)
               .HasForeignKey(bc => bc.ProductionId);
            builder.Entity<ProductionCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(b => b.ProductionCategories)
                .HasForeignKey(bc => bc.CategoryId);

            // VodProductions
            builder.Entity<VodProduction>()
                .HasKey(bc => new { bc.VodId, bc.ProductionId });
            builder.Entity<VodProduction>()
           .HasOne(bc => bc.Vod)
           .WithMany(b => b.VodProductions)
           .HasForeignKey(bc => bc.VodId);
            builder.Entity<VodProduction>()
                .HasOne(bc => bc.Production)
                .WithMany(b => b.VodProductions)
                .HasForeignKey(bc => bc.ProductionId);


            base.OnModelCreating(builder);
        }

    }
}
