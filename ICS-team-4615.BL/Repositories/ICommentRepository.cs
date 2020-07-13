using System.Collections.Generic;
using ICS_team_4615.BL.Model;

namespace ICS_team_4615.BL.Repositories
{
    public interface ICommentRepository : IRepositoryBase
    {
        CommentModel getById(int id);
        void Update(CommentModel commentModel);
        CommentModel Add(CommentModel commentModel);
    }
}