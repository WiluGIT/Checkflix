﻿using Checkflix.Models;
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
            // seed vod
            builder.Entity<Vod>().HasData(
                new Vod {VodId = 1, PlatformName = "Netflix"},
                new Vod {VodId = 2, PlatformName = "HBO GO" }
                );
            // seed categories
            builder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Akcja", GenreApiId = 28},
                new Category { CategoryId = 2, CategoryName = "Thriller", GenreApiId = 53 },
                new Category { CategoryId = 3, CategoryName = "film TV", GenreApiId = 10770 },
                new Category { CategoryId = 4, CategoryName = "Sci-Fi", GenreApiId = 878 },
                new Category { CategoryId = 5, CategoryName = "Romans", GenreApiId = 10749 },
                new Category { CategoryId = 6, CategoryName = "Tajemnica", GenreApiId = 9648 },
                new Category { CategoryId = 7, CategoryName = "Muzyczny", GenreApiId = 10402 },
                new Category { CategoryId = 8, CategoryName = "Horror", GenreApiId = 27 },
                new Category { CategoryId = 9, CategoryName = "Wojenny", GenreApiId = 10752 },
                new Category { CategoryId = 10, CategoryName = "Historyczny", GenreApiId = 36 },
                new Category { CategoryId = 11, CategoryName = "Familijny", GenreApiId = 10751 },
                new Category { CategoryId = 12, CategoryName = "Dramat", GenreApiId = 18 },
                new Category { CategoryId = 13, CategoryName = "Dokumentalny", GenreApiId = 99 },
                new Category { CategoryId = 14, CategoryName = "Kryminał", GenreApiId = 80 },
                new Category { CategoryId = 15, CategoryName = "Komedia", GenreApiId = 35 },
                new Category { CategoryId = 16, CategoryName = "Animacja", GenreApiId = 16 },
                new Category { CategoryId = 17, CategoryName = "Przygodowy", GenreApiId = 12 },
                new Category { CategoryId = 18, CategoryName = "Fantasy", GenreApiId = 14 },
                new Category { CategoryId = 19, CategoryName = "Western", GenreApiId = 37 }
                );

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
