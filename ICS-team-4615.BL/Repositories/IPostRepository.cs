using System.Collections.Generic;
using ICS_team_4615.BL.Model;

namespace ICS_team_4615.BL.Repositories
{
    public interface IPostRepository :IRepositoryBase
    {
        PostModel getById(int id);
        void Update(PostModel postModel);
        PostModel Add(PostModel postModel);
    }
}