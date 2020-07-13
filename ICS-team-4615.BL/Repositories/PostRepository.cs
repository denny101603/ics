using System.Collections.Generic;
using System.Linq;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Model;
using ICS_team_4615.DAL;
using ICS_team_4615.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS_team_4615.BL.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public PostRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public List<PostModel> FindByText(string text, TeamModel team, bool searchAlsoInComments)
        {
            var retList = new List<PostModel>();
            var foundPosts = dbContextFactory
                .CreateDbContext()
                .Posts
                .Where(p => p.Team.TeamId == team.Id && (p.Title.Contains(text) || p.Text.Contains(text)));

            if (searchAlsoInComments)
            {
                var commentRepository = new CommentRepository(dbContextFactory, mapper);

                foreach (var foundComment in commentRepository.FindByText(text, team))
                    retList.Add(foundComment.ParentPost);
            }
            foreach (var post in foundPosts)
            {
                retList.Add(getById(post.Id));
            }

            return retList;
        }

        public void Remove(int id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
                dbContext.Posts.Remove(entity);
                dbContext.SaveChanges();
            }
        }

        public PostModel getById(int id)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .Posts
                .Include(p => p.Comments)
                .Include(p=>p.Author)
                .Include(p=>p.Team)
                .FirstOrDefault(p => p.Id == id);

            return mapper.MapPostToPostModel(foundEntity);
        }

        public void Update(PostModel postModel)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapPostModelToPost(postModel);
                entity.Comments = null;
                dbContext.Posts.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public PostModel Add(PostModel postModel)
        {
            var entity = mapper.MapPostModelToPost(postModel);
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                entity.Team = null;
                entity.Author = null;
                dbContext.Posts.Add(entity);
                dbContext.SaveChanges();
            }
            AddTeamAndUserToPost(postModel, entity);
            return getById(entity.Id);
        }

        private void AddTeamAndUserToPost(PostModel postModel, Post entity)
        {
            var returnedModel = mapper.MapPostToPostModel(entity);
            returnedModel.Team = postModel.Team;
            returnedModel.Author = postModel.Author;
            Update(returnedModel);
        }
    }
}