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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("RequestId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1L,
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
                            Planning = new DateTime(2023, 7, 3, 19, 24, 2, 987, DateTimeKind.Utc).AddTicks(483)
                        },
                        new
                        {
                            Id = 2L,
                            Planning = new DateTime(2023, 7, 3, 19, 24, 2, 987, DateTimeKind.Utc).AddTicks(530)
                        });
                });

            modelBuilder.Entity("RMS.Domain.Entities.Request", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("CancelId")
                        .HasColumnType("bigint");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CloseId")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("CreatedId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ExecutorId")
                        .HasColumnType("bigint");

                    b.Property<long>("LifecycleId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("OpenId")
                        .HasColumnType("bigint");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CancelId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CloseId");

                    b.HasIndex("CreatedId");

                    b.HasIndex("ExecutorId");

                    b.HasIndex("LifecycleId");

                    b.HasIndex("OpenId");

                    b.ToTable("Requests");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Address = "some address",
                            CategoryId = 1L,
                            Comment = "comment",
                            CreatedId = 1L,
                            Description = "description",
                            ExecutorId = 1L,
                            LifecycleId = 1L,
                            Name = "request 1",
                            Priority = 1,
                            Status = 1
                        },
                        new
                        {
                            Id = 2L,
                            Address = "some address",
                            CategoryId = 1L,
                            Comment = "comment",
                            CreatedId = 1L,
                            Description = "description",
                            ExecutorId = 1L,
                            LifecycleId = 2L,
                            Name = "request 2",
                            Priority = 2,
                            Status = 1
                        });
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
                            Login = "ADMIN",
                            Password = "password",
                            Surname = "Akchurin"
                        },
                        new
                        {
                            Id = 2L,
                            Comment = "Comment",
                            FirstName = "Anton",
                            ImgPath = "../../img/png/user.png",
                            IsActive = true,
                            Login = "MANAGER",
                            Password = "password",
                            Surname = "Guryshkin"
                        },
                        new
                        {
                            Id = 3L,
                            Comment = "Comment",
                            FirstName = "Georgii",
                            ImgPath = "../../img/png/user.png",
                            IsActive = true,
                            Login = "mounter",
                            Password = "password",
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

            modelBuilder.Entity("RMS.Domain.Entities.Category", b =>
                {
                    b.HasOne("RMS.Domain.Entities.Request", null)
                        .WithMany("Categories")
                        .HasForeignKey("RequestId");
                });

            modelBuilder.Entity("RMS.Domain.Entities.Request", b =>
                {
                    b.HasOne("RMS.Domain.Entities.User", "Cancelled")
                        .WithMany()
                        .HasForeignKey("CancelId");

                    b.HasOne("RMS.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RMS.Domain.Entities.User", "Closed")
                        .WithMany()
                        .HasForeignKey("CloseId");

                    b.HasOne("RMS.Domain.Entities.User", "Created")
                        .WithMany()
                        .HasForeignKey("CreatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RMS.Domain.Entities.User", "Executor")
                        .WithMany()
                        .HasForeignKey("ExecutorId");

                    b.HasOne("RMS.Domain.Entities.Lifecycle", "Lifecycle")
                        .WithMany()
                        .HasForeignKey("LifecycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RMS.Domain.Entities.User", "Opened")
                        .WithMany()
                        .HasForeignKey("OpenId");

                    b.Navigation("Cancelled");

                    b.Navigation("Category");

                    b.Navigation("Closed");

                    b.Navigation("Created");

                    b.Navigation("Executor");

                    b.Navigation("Lifecycle");

                    b.Navigation("Opened");
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

            modelBuilder.Entity("RMS.Domain.Entities.Request", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
