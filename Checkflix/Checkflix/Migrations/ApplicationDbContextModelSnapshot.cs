﻿// <auto-generated />
using System;
using Checkflix.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Checkflix.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Checkflix.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserCategory", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ApplicationUserId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ApplicationUserCategories");
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserNotification", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("NotificationId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSeen")
                        .HasColumnType("INTEGER");

                    b.HasKey("ApplicationUserId", "NotificationId");

                    b.HasIndex("NotificationId");

                    b.ToTable("ApplicationUserNotifications");
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserProduction", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductionId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Favourites")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool?>("ToWatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool?>("Watched")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.HasKey("ApplicationUserId", "ProductionId");

                    b.HasIndex("ProductionId");

                    b.ToTable("ApplicationUserProductions");
                });

            modelBuilder.Entity("Checkflix.Models.ApplicationUserVod", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("VodId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ApplicationUserId", "VodId");

                    b.HasIndex("VodId");

                    b.ToTable("ApplicationUserVods");
                });

            modelBuilder.Entity("Checkflix.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryName")
                        .HasColumnType("TEXT");

                    b.Property<int>("GenreApiId")
                        .HasColumnType("INTEGER");

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

            modelBuilder.Entity("Checkflix.Models.Following", b =>
                {
                    b.Property<string>("FolloweeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FollowerId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("FolloweeIsMuted")
                        .HasColumnType("INTEGER");

                    b.HasKey("FolloweeId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Followings");
                });

            modelBuilder.Entity("Checkflix.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.HasKey("NotificationId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Checkflix.Models.Production", b =>
                {
                    b.Property<int>("ProductionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImbdId")
                        .HasColumnType("TEXT");

                    b.Property<float>("ImbdRating")
                        .HasColumnType("REAL");

                    b.Property<string>("Poster")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subtitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("Synopsis")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductionId");

                    b.ToTable("Productions");
                });

            modelBuilder.Entity("Checkflix.Models.ProductionCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CategoryId", "ProductionId");

                    b.HasIndex("ProductionId");

                    b.ToTable("ProductionCategories");
                });

            modelBuilder.Entity("Checkflix.Models.Vod", b =>
                {
                    b.Property<int>("VodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlatformName")
                        .HasColumnType("TEXT");

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
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VodId", "ProductionId");

                    b.HasIndex("ProductionId");

                    b.ToTable("VodProductions");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50000);

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SubjectId")
                        .HasColumnType("TEXT")
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
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50000);

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("TEXT");

                    b.Property<string>("SubjectId")
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("Key");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.ToTable("PersistedGrants");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

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

            modelBuilder.Entity("Checkflix.Models.ApplicationUserNotification", b =>
                {
                    b.HasOne("Checkflix.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("ApplicationUserNotifications")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Checkflix.Models.Notification", "Notification")
                        .WithMany("ApplicationUserNotifications")
                        .HasForeignKey("NotificationId")
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

            modelBuilder.Entity("Checkflix.Models.Following", b =>
                {
                    b.HasOne("Checkflix.Models.ApplicationUser", "Followee")
                        .WithMany("Followers")
                        .HasForeignKey("FolloweeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Checkflix.Models.ApplicationUser", "Follower")
                        .WithMany("Followees")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict)
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
