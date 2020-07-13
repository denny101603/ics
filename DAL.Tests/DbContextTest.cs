using System;
using System.Collections.Generic;
using System.Linq;
using ICS_team_4615.BL.Model;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Repositories;
using ICS_team_4615.DAL;
using ICS_team_4615.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DAL.Tests
{
    public class DbContextTest
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly Mapper mapper = new Mapper();

        public DbContextTest()
        {
            dbContextFactory = new InMemoryDbContextFactory();//DesignTimeDbContextFactory();
        }

        [Fact]
        public void AddUserTest()
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                db.Users.Add(new User
                {
                    Name = "Berry",
                    MailAddress = "j@s.cz",
                    LastLogged = DateTime.Today,
                    PasswordHash = "123456",
                    Teams = null
                });

                db.SaveChanges();
            }

            using (var db = dbContextFactory.CreateDbContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Name == "Berry");
                var referenceUser = new User
                {
                    Name = "Berry",
                    MailAddress = "j@s.cz",
                    LastLogged = DateTime.Today,
                    PasswordHash = "123456",
                    Teams = null
                };

                Assert.Equal(referenceUser.Name, user.Name);
                db.Remove(user);
                db.SaveChanges();
            }
            Assert.Null(dbContextFactory.CreateDbContext().Users.FirstOrDefault(u => u.Name == "Berry"));
        }

        [Fact]
        public void AddTeamTest()
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                db.Teams.Add(new Team
                {
                    Name = "Salati",
                    Description = "ICS",
                    Users = null
                });
                db.SaveChanges();
            }

            using (var db = dbContextFactory.CreateDbContext())
            {
                var team = db.Teams.FirstOrDefault(x => x.Name == "Salati");
                var referenceTeam = new Team
                {
                    Name = "Salati",
                    Description = "ICS",
                    Users = null
                };
                Assert.Equal(referenceTeam.Name, team.Name);
                db.Remove(team);
                db.SaveChanges();
            }

            Assert.Null(dbContextFactory.CreateDbContext().Teams.FirstOrDefault(t => t.Name == "Salati"));
        }

        [Fact]
        public void ModelToEntityTest()
        {
            var mapper = new Mapper();
            var model = new UserModel
            {
                Name = "Denny",
                MailAddress = "d@s.cz",
                LastLogged = DateTime.Today,
                PasswordHash = "0123456",
                Teams = null
            };

            var user = mapper.MapUserModelToUser(model);
            using (var db = dbContextFactory.CreateDbContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }

            using (var db = dbContextFactory.CreateDbContext())
            {
                var loadedUser = db.Users.FirstOrDefault(x => x.Name == "Denny");
                var referenceUser = new User
                {
                    Name = "Denny",
                    MailAddress = "d@s.cz",
                    LastLogged = DateTime.Today,
                    PasswordHash = "0123456",
                    Teams = null
                };
                Assert.Equal(referenceUser.Name, loadedUser.Name);

            }
        }




    }
}