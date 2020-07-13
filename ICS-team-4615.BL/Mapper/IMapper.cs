using System.Collections;
using System.Collections.Generic;
using ICS_team_4615.BL.Model;
using ICS_team_4615.DAL.Entities;

namespace ICS_team_4615.BL.Mapper
{
    public interface IMapper
    {
        CommentModel MapCommentToCommentModel(Comment entity, bool mapParent);

        PostModel MapPostToPostModel(Post entity);

        TeamModel MapTeamToTeamModel(Team entity, List<User> users, List<Post> posts);

        UserModel MapUserToUserModel(User entity, List<Team> teams);


        Comment MapCommentModelToComment(CommentModel model);

        Post MapPostModelToPost(PostModel model);

        Team MapTeamModelToTeam(TeamModel model);

        User MapUserModelToUser(UserModel model);


    }
}
