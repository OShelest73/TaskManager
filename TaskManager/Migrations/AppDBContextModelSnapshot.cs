﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManager.Data;

#nullable disable

namespace TaskManager.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NotificationModelUserModel", b =>
                {
                    b.Property<int>("NotificationsId")
                        .HasColumnType("int");

                    b.Property<string>("ReceiversEmailAddress")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("NotificationsId", "ReceiversEmailAddress");

                    b.HasIndex("ReceiversEmailAddress");

                    b.ToTable("NotificationModelUserModel");
                });

            modelBuilder.Entity("TaskManager.Models.CategoryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TaskManager.Models.NotificationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("TaskManager.Models.StatusModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("TaskManager.Models.TaskModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorEmailAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FinishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorEmailAddress");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StatusId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TaskManager.Models.UserModel", b =>
                {
                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int");

                    b.HasKey("EmailAddress");

                    b.HasIndex("CategoryId");

                    b.HasIndex("TaskId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskManager.Models.WorkspaceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("WorkspaceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("Workspaces_Users", b =>
                {
                    b.Property<string>("EmailAddresses")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("EmailAddresses", "WorkspaceId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Workspaces_Users");
                });

            modelBuilder.Entity("NotificationModelUserModel", b =>
                {
                    b.HasOne("TaskManager.Models.NotificationModel", null)
                        .WithMany()
                        .HasForeignKey("NotificationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.UserModel", null)
                        .WithMany()
                        .HasForeignKey("ReceiversEmailAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskManager.Models.TaskModel", b =>
                {
                    b.HasOne("TaskManager.Models.UserModel", "Author")
                        .WithMany("Tasks")
                        .HasForeignKey("AuthorEmailAddress");

                    b.HasOne("TaskManager.Models.CategoryModel", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.StatusModel", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.WorkspaceModel", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Category");

                    b.Navigation("Status");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("TaskManager.Models.UserModel", b =>
                {
                    b.HasOne("TaskManager.Models.CategoryModel", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.TaskModel", "Task")
                        .WithMany("AppointedUsers")
                        .HasForeignKey("TaskId");

                    b.Navigation("Category");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Workspaces_Users", b =>
                {
                    b.HasOne("TaskManager.Models.UserModel", null)
                        .WithMany()
                        .HasForeignKey("EmailAddresses")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.WorkspaceModel", null)
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskManager.Models.TaskModel", b =>
                {
                    b.Navigation("AppointedUsers");
                });

            modelBuilder.Entity("TaskManager.Models.UserModel", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
