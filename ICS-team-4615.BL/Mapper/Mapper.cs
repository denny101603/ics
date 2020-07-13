using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using ICS_team_4615.BL.Model;
using ICS_team_4615.DAL;
using ICS_team_4615.DAL.Entities;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;


namespace ICS_team_4615.BL.Mapper
{
    public class Mapper : IMapper
    {
        public CommentModel MapCommentToCommentModel(Comment entity, bool mapParent)
        {
            PostModel postModel;
            if (mapParent)
                postModel = MapPostToPostModel(entity.ParentPost);
            else
                postModel = null;
            return new CommentModel
            {
                Id = entity.Id,
                Author = MapUserToUserModel(entity.Author, null),
                ParentPost = postModel,
                Text = entity.Text,
                TimeCreated = entity.TimeCreated
            };
        }

        /// <summary>
        /// Namapuje post entitu na postmodel a seradi komentare podle casu jejich pridani
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PostModel MapPostToPostModel(Post entity)
        {
            if (entity == null)
                return null;
            var returnPostModel = new PostModel
            {
                Id = entity.Id,
                Author = MapUserToUserModel(entity.Author, null),
                TimeCreated = entity.TimeCreated,
                LastCommentedOrCreated = entity.TimeCreated,
                Comments = new List<CommentModel>(),
                Team = MapTeamToTeamModel(entity.Team, null, null),
                Text = entity.Text,
                Title = entity.Title
            };

            if (entity.Comments.Count == 0) return returnPostModel;
            foreach (var comment in entity.Comments)
            {
                returnPostModel.Comments.Add(MapCommentToCommentModel(comment, false));
            }
            _SortCommentsInPost(returnPostModel);

            return returnPostModel;
        }



        public TeamModel MapTeamToTeamModel(Team team, List<User> users, List<Post> posts)
        {
            if (team == null) return null;
            var teamModelDetail = new TeamModel
            {
                Id = team.TeamId,
                Description = team.Description,
                Name = team.Name,
                Posts = new List<PostModel>(),
                Members = new ObservableCollection<UserModel>()
            };
            if (users == null)
                return teamModelDetail;
            foreach (var user in users)
                teamModelDetail.Members.Add(MapUserToUserModel(user, null));
            if (posts == null)
                return teamModelDetail;
            foreach (var post in posts)
                teamModelDetail.Posts.Add(MapPostToPostModel(post));

            _SortPostsInTeam(teamModelDetail);

            return teamModelDetail;
        }

        public UserModel MapUserToUserModel(User entity, List<Team> teams)
        {
            if (entity == null) return null;
            var userModel = new UserModel
            {
                Id = entity.UserId,
                Name = entity.Name,
                MailAddress = entity.MailAddress,
                LastLogged = entity.LastLogged,
                PasswordHash = entity.PasswordHash,
                Teams = new ObservableCollection<TeamModel>()
            };
            if (teams == null)
            {
                return userModel;
            }
            foreach (var team in teams)
            {
                userModel.Teams.Add(MapTeamToTeamModel(team, null, null));
            }

            return userModel;
        }

        public Comment MapCommentModelToComment(CommentModel model)
        {
            var entity = new Comment
            {
                Id = model.Id,
                Author = MapUserModelToUser(model.Author),
                ParentPost = MapPostModelToPost(model.ParentPost),
                Text = model.Text,
                TimeCreated = model.TimeCreated
            };
            if (model.ParentPost.Author.Id == model.Author.Id)
                entity.Author = entity.ParentPost.Author;
            return entity;
        }

        public Post MapPostModelToPost(PostModel model)
        {
            if (model == null)
                return null;
            var returnPost = new Post
            {
                Id = model.Id,
                Author = MapUserModelToUser(model.Author),
                Comments = new List<Comment>(),
                Team = MapTeamModelToTeam(model.Team),
                Text = model.Text,
                TimeCreated = model.TimeCreated,
                Title = model.Title
            };

            return returnPost;
        }

        public Team MapTeamModelToTeam(TeamModel model)
        {
            if (model == null) return null;
            return new Team
            {
                Description = model.Description,
                Name = model.Name,
                TeamId = model.Id
            };
        }

        public User MapUserModelToUser(UserModel model)
        {
            if (model == null) return null;
            var retUser = new User
            {
                UserId = model.Id,
                Name = model.Name,
                MailAddress = model.MailAddress,
                LastLogged = model.LastLogged,
                PasswordHash = model.PasswordHash,
            };

            return retUser;
        }

        private static void _SortCommentsInPost(PostModel returnPostModel)
        {
            returnPostModel.Comments = returnPostModel.Comments.OrderBy(c => c.TimeCreated).ToList();
            returnPostModel.LastCommentedOrCreated = returnPostModel.Comments.Last().TimeCreated;
        }

        private static void _SortPostsInTeam(TeamModel teamModelDetail)
        {
            teamModelDetail.Posts = teamModelDetail.Posts.OrderBy(p => p.LastCommentedOrCreated).Reverse().ToList();
        }
    }
}