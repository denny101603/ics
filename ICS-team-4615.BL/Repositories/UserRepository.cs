using System;
using System.Collections.Generic;
using System.Linq;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ICS_team_4615.DAL;
using ICS_team_4615.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ICS_team_4615.BL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public UserRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }
        public void Remove(int id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var user = new User
                {
                    UserId = id
                };
                var userteams = dbContext.UserTeams.Select(ut => ut).Where(ut => ut.userId == user.UserId).ToList();
                foreach (var userteam in userteams)
                {
                    dbContext.UserTeams.Attach(userteam);
                    dbContext.UserTeams.Remove(userteam);
                }
                dbContext.Users.Attach(user);
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
        }

        public UserModel GetByMail(String mail)
        {
            //made by Berry, uploaded by Peter
            User foundEntity = null;
            List<Team> teams = new List<Team>();
            using (var context = dbContextFactory.CreateDbContext())
            {
                foundEntity = context.Users.FirstOrDefault(t => t.MailAddress == mail);
                if (foundEntity != null)
                {
                    var userTeams = context.UserTeams.Include(t => t.team).Select(ut => ut).Where(ut => ut.userId == foundEntity.UserId).ToList();
                    teams = userTeams.Select(ut => ut.team).ToList();
                }
            }
            return foundEntity == null ? null : mapper.MapUserToUserModel(foundEntity, teams);
        }

        public UserModel GetById(int id)
        {
            User foundEntity = null;
            List<Team> teams = new List<Team>();
            using (var context = dbContextFactory.CreateDbContext())
            {
                foundEntity = context.Users.FirstOrDefault(t => t.UserId == id);
                if (foundEntity != null)
                {
                    var userTeams = context.UserTeams.Include(t => t.team).Select(ut => ut).Where(ut => ut.userId == foundEntity.UserId).ToList();
                    teams = userTeams.Select(ut => ut.team).ToList();
                }
            }
            return foundEntity == null ? null : mapper.MapUserToUserModel(foundEntity, teams);
        }

        public void UpdateInfo(UserModel userModel)
        {
            //Princip: Vytahnu si z DB entitu (normálně přes repo), v ní změním údaje a potom touhle fcí propíšu zpátky. 
            //Cirkus kvůli tomu, abych tu u každé prop nekontroloval isNull
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Users.FirstOrDefault(t => t.UserId == userModel.Id);
                entity.Name = userModel.Name;
                entity.LastLogged = userModel.LastLogged;
                entity.MailAddress = userModel.MailAddress;
                entity.PasswordHash = userModel.PasswordHash;
                dbContext.SaveChanges();
            }
        }

        public UserModel Add(UserModel userModel)
        {
            //WARNING: uzivatele vzdy pridavat prazdného
            using (var context = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapUserModelToUser(userModel);
                context.Users.Add(entity);
                context.SaveChanges();
                return mapper.MapUserToUserModel(entity, null);
            }
        }
    }
}