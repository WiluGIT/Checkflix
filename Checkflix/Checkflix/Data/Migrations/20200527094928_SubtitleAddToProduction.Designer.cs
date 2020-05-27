﻿// <auto-generated />
using System;
using Checkflix.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Checkflix.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200527094928_SubtitleAddToProduction")]
    partial class SubtitleAddToProduction
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Checkflix.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserCategory", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("ApplicationUserId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ApplicationUserCategories");
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserProduction", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ProductionId")
                        .HasColumnType("int");

                    b.Property<bool>("Favourites")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("ToWatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("Watched")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("ApplicationUserId", "ProductionId");

                    b.HasIndex("ProductionId");

                    b.ToTable("ApplicationUserProductions");
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserVod", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VodId")
                        .HasColumnType("int");

                    b.HasKey("ApplicationUserId", "VodId");

                    b.HasIndex("VodId");

                    b.ToTable("ApplicationUserVods");
                });

            modelBuilder.Entity("Checkflix.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GenreApiId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Akcja",
                            GenreApiId = 28
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Thriller",
                            GenreApiId = 53
                        },
                        new
                        {
                            CategoryId = 3,
                            CategoryName = "film TV",
                            GenreApiId = 10770
                        },
                        new
                        {
                            CategoryId = 4,
                            CategoryName = "Sci-Fi",
                            GenreApiId = 878
                        },
                        new
                        {
                            CategoryId = 5,
                            CategoryName = "Romans",
                            GenreApiId = 10749
                        },
                        new
                        {
                            CategoryId = 6,
                            CategoryName = "Tajemnica",
                            GenreApiId = 9648
                        },
                        new
                        {
                            CategoryId = 7,
                            CategoryName = "Muzyczny",
                            GenreApiId = 10402
                        },
                        new
                        {
                            CategoryId = 8,
                            CategoryName = "Horror",
                            GenreApiId = 27
                        },
                        new
                        {
                            CategoryId = 9,
                            CategoryName = "Wojenny",
                            GenreApiId = 10752
                        },
                        new
                        {
                            CategoryId = 10,
                            CategoryName = "Historyczny",
                            GenreApiId = 36
                        },
                        new
                        {
                            CategoryId = 11,
                            CategoryName = "Familijny",
                            GenreApiId = 10751
                        },
                        new
                        {
                            CategoryId = 12,
                            CategoryName = "Dramat",
                            GenreApiId = 18
                        },
                        new
                        {
                            CategoryId = 13,
                            CategoryName = "Dokumentalny",
                            GenreApiId = 99
                        },
                        new
                        {
                            CategoryId = 14,
                            CategoryName = "Kryminał",
                            GenreApiId = 80
                        },
                        new
                        {
                            CategoryId = 15,
                            CategoryName = "Komedia",
                            GenreApiId = 35
                        },
                        new
                        {
                            CategoryId = 16,
                            CategoryName = "Animacja",
                            GenreApiId = 16
                        },
                        new
                        {
                            CategoryId = 17,
                            CategoryName = "Przygodowy",
                            GenreApiId = 12
                        },
                        new
                        {
                            CategoryId = 18,
                            CategoryName = "Fantasy",
                            GenreApiId = 14
                        },
                        new
                        {
                            CategoryId = 19,
                            CategoryName = "Western",
                            GenreApiId = 37
                        },
                        new
                        {
                            CategoryId = 20,
                            CategoryName = "Akcja i Przygoda",
                            GenreApiId = 10759
                        },
                        new
                        {
                            CategoryId = 21,
                            CategoryName = "Dzieci",
                            GenreApiId = 10762
                        },
                        new
                        {
                            CategoryId = 22,
                            CategoryName = "News",
                            GenreApiId = 10763
                        },
                        new
                        {
                            CategoryId = 23,
                            CategoryName = "Reality",
                            GenreApiId = 10764
                        },
                        new
                        {
                            CategoryId = 24,
                            CategoryName = "Sci-Fi & Fantasy",
                            GenreApiId = 10765
                        },
                        new
                        {
                            CategoryId = 25,
                            CategoryName = "Opera",
                            GenreApiId = 10766
                        },
                        new
                        {
                            CategoryId = 26,
                            CategoryName = "Rozmowy",
                            GenreApiId = 10767
                        },
                        new
                        {
                            CategoryId = 27,
                            CategoryName = "Wojna & Polityka",
                            GenreApiId = 10768
                        });
                });

            modelBuilder.Entity("Checkflix.Models.Production", b =>
                {
                    b.Property<int>("ProductionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImbdId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("ImbdRating")
                        .HasColumnType("real");

                    b.Property<string>("Poster")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subtitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Synopsis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ProductionId");

                    b.ToTable("Productions");
                });

            modelBuilder.Entity("Checkflix.Models.ProductionCategory", b =>
                {
                    b.Property<int>("ProductionId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("ProductionId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ProductionCategories");
                });

            modelBuilder.Entity("Checkflix.Models.Vod", b =>
                {
                    b.Property<int>("VodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PlatformName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VodId");

                    b.ToTable("Vods");

                    b.HasData(
                        new
                        {
                            VodId = 1,
                            PlatformName = "Netflix"
                        },
                        new
                        {
                            VodId = 2,
                            PlatformName = "HBO GO"
                        });
                });

            modelBuilder.Entity("Checkflix.Models.VodProduction", b =>
                {
                    b.Property<int>("VodId")
                        .HasColumnType("int");

                    b.Property<int>("ProductionId")
                        .HasColumnType("int");

                    b.HasKey("VodId", "ProductionId");

                    b.HasIndex("ProductionId");

                    b.ToTable("VodProductions");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(50000);

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(50000);

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Key");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.ToTable("PersistedGrants");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserCategory", b =>
                {
                    b.HasOne("Checkflix.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("ApplicationUserCategories")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkflix.Models.Category", "Category")
                        .WithMany("ApplicationUserCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserProduction", b =>
                {
                    b.HasOne("Checkflix.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("ApplicationUserProductions")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkflix.Models.Production", "Production")
                        .WithMany("ApplicationUserProductions")
                        .HasForeignKey("ProductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserVod", b =>
                {
                    b.HasOne("Checkflix.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("ApplicationUserVods")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkflix.Models.Vod", "Vod")
                        .WithMany("ApplicationUserVods")
                        .HasForeignKey("VodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Checkflix.Models.ProductionCategory", b =>
                {
                    b.HasOne("Checkflix.Models.Category", "Category")
                        .WithMany("ProductionCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkflix.Models.Production", "Production")
                        .WithMany("ProductionCategories")
                        .HasForeignKey("ProductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Checkflix.Models.VodProduction", b =>
                {
                    b.HasOne("Checkflix.Models.Production", "Production")
                        .WithMany("VodProductions")
                        .HasForeignKey("ProductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkflix.Models.Vod", "Vod")
                        .WithMany("VodProductions")
                        .HasForeignKey("VodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Checkflix.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Checkflix.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkflix.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Checkflix.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
