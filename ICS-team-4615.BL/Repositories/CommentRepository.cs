using System.Collections.Generic;
using System.Linq;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Model;
using ICS_team_4615.DAL;
using ICS_team_4615.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ICS_team_4615.BL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public CommentRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public List<CommentModel> FindByText(string text, TeamModel team)
        {
            var foundComments = dbContextFactory
                .CreateDbContext()
                .Comments
                .Where(c => c.ParentPost.Team.TeamId == team.Id && c.Text.Contains(text))
                .Include(p => p.Author)
                .Include(c => c.ParentPost).ThenInclude(p => p.Team)
                .Include(c => c.ParentPost).ThenInclude(p => p.Author);

            return foundComments.Select(comment => mapper.MapCommentToCommentModel(comment, true)).ToList();
        }

        public void Remove(int id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = new Comment
                {
                    Id = id
                };

                dbContext.Comments.Attach(entity);
                dbContext.Comments.Remove(entity);
                dbContext.SaveChanges();
            }
        }

        public CommentModel getById(int id)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .Comments
                .Include(c=>c.Author)
                .Include(c=>c.ParentPost)
                .FirstOrDefault(t => t.Id == id);

            return foundEntity == null ? null : mapper.MapCommentToCommentModel(foundEntity, true);
        }

        public void Update(CommentModel commentModel)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapCommentModelToComment(commentModel);
                dbContext.Comments.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public CommentModel Add(CommentModel commentModel)
        {
            var entity = mapper.MapCommentModelToComment(commentModel);
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                entity.Author = null;
                entity.ParentPost = null;
                dbContext.Comments.Add(entity);
                dbContext.SaveChanges();
            }

            AddAuthorAndParentPost(commentModel, entity);
            return getById(entity.Id);
        }

        private void AddAuthorAndParentPost(CommentModel commentModel, Comment entity)
        {
            var retModel = mapper.MapCommentToCommentModel(entity, true);
            retModel.ParentPost = commentModel.ParentPost;
            retModel.Author = commentModel.Author;
            Update(retModel);
        }
    }
}