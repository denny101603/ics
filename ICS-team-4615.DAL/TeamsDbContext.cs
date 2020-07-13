using System;
using ICS_team_4615.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS_team_4615.DAL
{
    public class TeamsDbContext : DbContext
    {
        public TeamsDbContext()
        {
        }

        public TeamsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Team>().HasKey(u => u.TeamId);

            modelBuilder.Entity<UserTeam>().HasKey(sc => new { sc.userId, sc.teamId });

            modelBuilder.Entity<Post>()
                .HasOne(t => t.Team)
                .WithMany(t => t.Posts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentPost)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTeam>()
                .HasOne<User>(sc => sc.user)
                .WithMany(s => s.Teams)
                .HasForeignKey(sc => sc.userId);


            modelBuilder.Entity<UserTeam>()
                .HasOne<Team>(sc => sc.team)
                .WithMany(s => s.Users)
                .HasForeignKey(sc => sc.teamId);
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }

    }
}
