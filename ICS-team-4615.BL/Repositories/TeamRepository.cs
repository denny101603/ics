using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Model;
using ICS_team_4615.DAL;
using ICS_team_4615.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ICS_team_4615.BL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public TeamRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public void Remove(int id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var team = dbContext.Teams
                    .Include(t => t.Posts)
                    .ThenInclude(p => p.Comments)
                    .FirstOrDefault(t => t.TeamId == id);

                var userteams = dbContext.UserTeams.Select(ut => ut).Where(ut => ut.teamId == team.TeamId).ToList();
                foreach (var userteam in userteams)
                {
                    dbContext.UserTeams.Attach(userteam);
                    dbContext.UserTeams.Remove(userteam);
                }
                dbContext.Teams.Remove(team);
                dbContext.SaveChanges();
            }
        }

        public TeamModel GetById(int id)
        {
            Team foundEntity = null;
            List<User> users = new List<User>();
            List<Post> posts = new List<Post>();
            using (var ctx = dbContextFactory.CreateDbContext())
            {
                foundEntity = ctx.Teams.FirstOrDefault(t => t.TeamId == id);
                if (foundEntity != null)
                {
                    var userTeams = ctx.UserTeams.Include(t => t.user).Select(ut => ut).Where(ut => ut.teamId == foundEntity.TeamId).ToList();
                    users = userTeams.Select(ut => ut.user).ToList();
                    posts = ctx.Posts.Select(p => p)
                        .Where(p => p.Team.TeamId == foundEntity.TeamId)
                        .Include(p => p.Comments)
                        .ToList();
                }
            }

            return foundEntity == null ? null : mapper.MapTeamToTeamModel(foundEntity, users, posts);
        }

        public void UpdateInfo(TeamModel teamModel)
        {
            //Princip: Vytahnu si z DB entitu (normálně přes repo), v ní změním údaje a potom touhle fcí propíšu zpátky. 
            //Cirkus kvůli tomu, abych tu u každé prop nekontroloval isNull
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Teams.FirstOrDefault(t => t.TeamId == teamModel.Id);
                entity.Description = teamModel.Description;
                entity.Name = teamModel.Name;
                dbContext.SaveChanges();
            }
        }

        public TeamModel Add(TeamModel teamModel)
        {
            //WARNING: tým přidávat vždy prázdný!
            using (var context = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapTeamModelToTeam(teamModel);
                context.Teams.Add(entity);
                context.SaveChanges();
                return mapper.MapTeamToTeamModel(entity, null, null);
            }
        }

        public void AddUserToTeam(int addTeamId, int AddedUserId)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                var relationship = new UserTeam
                {
                    teamId = addTeamId,
                    userId = AddedUserId
                };
                context.UserTeams.Add(relationship);
                context.SaveChanges();
            }
        }

        public void RemoveUserFromTeam(int rTeamId, int removedUserId)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                var relationship =
                    context.UserTeams.FirstOrDefault(ut => ut.userId == removedUserId && ut.teamId == rTeamId);
                if (relationship == null)
                {
                    return;
                }
                context.UserTeams.Remove(relationship);
                context.SaveChanges();
            }
        }
    }
}