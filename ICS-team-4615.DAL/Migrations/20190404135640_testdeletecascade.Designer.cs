﻿// <auto-generated />
using System;
using ICS_team_4615.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ICS_team_4615.DAL.Migrations
{
    [DbContext(typeof(TeamsDbContext))]
    [Migration("20190404135640_testdeletecascade")]
    partial class testdeletecascade
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ICS_team_4615.DAL.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorUserId");

                    b.Property<int?>("ParentPostId");

                    b.Property<string>("Text");

                    b.Property<DateTime>("TimeCreated");

                    b.HasKey("Id");

                    b.HasIndex("AuthorUserId");

                    b.HasIndex("ParentPostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ICS_team_4615.DAL.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorUserId");

                    b.Property<int?>("TeamId");

                    b.Property<string>("Text");

                    b.Property<DateTime>("TimeCreated");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorUserId");

                    b.HasIndex("TeamId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ICS_team_4615.DAL.Entities.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("TeamId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ICS_team_4615.DAL.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastLogged");

                    b.Property<string>("MailAddress");

                    b.Property<string>("Name");

                    b.Property<string>("PasswordHash");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ICS_team_4615.DAL.UserTeam", b =>
                {
                    b.Property<int>("userId");

                    b.Property<int>("teamId");

                    b.HasKey("userId", "teamId");

                    b.HasIndex("teamId");

                    b.ToTable("UserTeams");
                });

            modelBuilder.Entity("ICS_team_4615.DAL.Entities.Comment", b =>
                {
                    b.HasOne("ICS_team_4615.DAL.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorUserId");

                    b.HasOne("ICS_team_4615.DAL.Entities.Post", "ParentPost")
                        .WithMany("Comments")
                        .HasForeignKey("ParentPostId");
                });

            modelBuilder.Entity("ICS_team_4615.DAL.Entities.Post", b =>
                {
                    b.HasOne("ICS_team_4615.DAL.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorUserId");

                    b.HasOne("ICS_team_4615.DAL.Entities.Team", "Team")
                        .WithMany("Posts")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ICS_team_4615.DAL.UserTeam", b =>
                {
                    b.HasOne("ICS_team_4615.DAL.Entities.Team", "team")
                        .WithMany("Users")
                        .HasForeignKey("teamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ICS_team_4615.DAL.Entities.User", "user")
                        .WithMany("Teams")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
