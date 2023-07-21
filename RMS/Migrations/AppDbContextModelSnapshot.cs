﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RMS.Domain;

#nullable disable

namespace RMS.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RMS.Domain.Entities.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IsDeleted = false,
                            Name = "No internet"
                        });
                });

            modelBuilder.Entity("RMS.Domain.Entities.Lifecycle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("Cancelled")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Closed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Current")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Planning")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Lifecycles");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Planning = new DateTime(2023, 7, 20, 20, 50, 2, 843, DateTimeKind.Utc).AddTicks(7862)
                        },
                        new
                        {
                            Id = 2L,
                            Planning = new DateTime(2023, 7, 20, 20, 50, 2, 843, DateTimeKind.Utc).AddTicks(7871)
                        });
                });

            modelBuilder.Entity("RMS.Domain.Entities.Request", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int?>("AbonentUID")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("CancelledId")
                        .HasColumnType("bigint");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ClosedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("CreatedId")
                        .HasColumnType("bigint");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<long>("LifecycleId")
                        .HasColumnType("bigint");

                    b.Property<long?>("OpenedId")
                        .HasColumnType("bigint");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CancelledId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ClosedId");

                    b.HasIndex("LifecycleId");

                    b.HasIndex("OpenedId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("RMS.Domain.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "admin"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "manager"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "mounter"
                        });
                });

            modelBuilder.Entity("RMS.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImgPath")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Comment = "Comment",
                            FirstName = "Max",
                            ImgPath = "../../img/jpg/preview.jpg",
                            IsActive = true,
                            IsDeleted = false,
                            Login = "ADMIN",
                            Password = "$HASH|V1$10000$44Oc0ZT6zyXzeesVBkAAEV4h9L8N4Vqs0XTG5byA5hPCzvRg",
                            Surname = "Akchurin"
                        },
                        new
                        {
                            Id = 2L,
                            Comment = "Comment",
                            FirstName = "Anton",
                            ImgPath = "../../img/Avatar/user.png",
                            IsActive = true,
                            IsDeleted = false,
                            Login = "MANAGER",
                            Password = "$HASH|V1$10000$hNlw2dGyBl8/FPk6Dc95XfbKU6vKUrue6FM0z+5w7UVOhWpB",
                            Surname = "Guryshkin"
                        },
                        new
                        {
                            Id = 3L,
                            Comment = "Comment",
                            FirstName = "Georgii",
                            ImgPath = "../../img/Avatar/user.png",
                            IsActive = true,
                            IsDeleted = false,
                            Login = "mounter",
                            Password = "$HASH|V1$10000$0h2V0WY2QapCCk9uAom3qZ8Y/IwSg2v4n+SFt7b4QcO8qyh8",
                            Surname = "Perepelitsa"
                        });
                });

            modelBuilder.Entity("RMS.Domain.Entities.UserRole", b =>
                {
                    b.Property<long>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserRoleId"));

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("UserRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            UserRoleId = 1L,
                            RoleId = 1L,
                            UserId = 1L
                        },
                        new
                        {
                            UserRoleId = 2L,
                            RoleId = 2L,
                            UserId = 2L
                        },
                        new
                        {
                            UserRoleId = 3L,
                            RoleId = 3L,
                            UserId = 3L
                        });
                });

            modelBuilder.Entity("RMS.Domain.Entities.Request", b =>
                {
                    b.HasOne("RMS.Domain.Entities.User", "Cancel")
                        .WithMany()
                        .HasForeignKey("CancelledId");

                    b.HasOne("RMS.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RMS.Domain.Entities.User", "Close")
                        .WithMany()
                        .HasForeignKey("ClosedId");

                    b.HasOne("RMS.Domain.Entities.Lifecycle", "Lifecycle")
                        .WithMany()
                        .HasForeignKey("LifecycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RMS.Domain.Entities.User", "Open")
                        .WithMany()
                        .HasForeignKey("OpenedId");

                    b.Navigation("Cancel");

                    b.Navigation("Category");

                    b.Navigation("Close");

                    b.Navigation("Lifecycle");

                    b.Navigation("Open");
                });

            modelBuilder.Entity("RMS.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("RMS.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RMS.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
