using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Tests;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Model;
using ICS_team_4615.BL.Repositories;
using ICS_team_4615.DAL;
using ICS_team_4615.DAL.Entities;
using Xunit;

namespace BL.Tests
{
    public class RepositoryAndMapperTests
    {
        private readonly Mapper mapper = new Mapper();
        private readonly IDbContextFactory dbContextFactory = new InMemoryDbContextFactory();//DesignTimeDbContextFactory();


        [Fact]
        public void AddCommentTest()
        {
            var userRepository = new UserRepository(dbContextFactory, mapper);
            var teamRepository = new TeamRepository(dbContextFactory, mapper);

            var userModel = new UserModel
            {
                Name = "Pepa Hnátek",
                LastLogged = DateTime.Today,
                MailAddress = "bla@bla.com"
            };

            var teamModel = new TeamModel
            {
                Description = "Tym resici ics",
                Name = "Borci"
            };

            userModel = userRepository.Add(userModel);
            teamModel = teamRepository.Add(teamModel);
            teamRepository.AddUserToTeam(teamModel.Id, userModel.Id);
            userModel = userRepository.GetById(userModel.Id);
            teamModel = teamRepository.GetById(teamModel.Id);

            var postRepository = new PostRepository(dbContextFactory, mapper);
            var commentRepository = new CommentRepository(dbContextFactory, mapper);
            var postModel = new PostModel
            {
                Title = "Hlava",
                Author = userModel,
                Text = "Ztratil jsem hlavu!",
                TimeCreated = DateTime.Now,
                Team = teamModel
            };
            postModel = postRepository.Add(postModel);

            var commentModel = new CommentModel
            {
                Text = "Tak ji zase najdi.",
                TimeCreated = DateTime.Now,
                Author = userModel,
                ParentPost = postModel
            };
            commentModel = commentRepository.Add(commentModel);

            //assert
            Assert.Equal("Tak ji zase najdi.", commentRepository.getById(commentModel.Id).Text);
            Assert.Equal("Tak ji zase najdi.", postRepository.getById(postModel.Id).Comments.First().Text);

            teamRepository.Remove(teamModel.Id); //should remove also the post and comment
            userRepository.Remove(userModel.Id);
            Assert.Null(postRepository.getById(postModel.Id));
            Assert.Null(commentRepository.getById(commentModel.Id));
        }

        [Fact]
        public void AddPostTest()
        {
            //arrange
            var userModel = new UserModel()
            {
                Name = "Pepa Hnátek",
                LastLogged = DateTime.Today,
                MailAddress = "bla@bla.com"
            };
            var userRepository = new UserRepository(dbContextFactory, mapper);
            userModel = userRepository.Add(userModel);

            var teamModel = new TeamModel
            {
                Description = "Tym resici ics",
                Name = "Borci"
            };
            var teamRepository = new TeamRepository(dbContextFactory, mapper);
            teamModel = teamRepository.Add(teamModel);

            teamRepository.AddUserToTeam(teamModel.Id, userModel.Id);
            var postModel = new PostModel
            {
                Title = "Hlava",
                Author = userModel,
                Text = "Ztratil jsem hlavu!",
                TimeCreated = DateTime.Now,
                Team = teamModel
            };
            //act
            var postRepository = new PostRepository(dbContextFactory, mapper);
            postModel = postRepository.Add(postModel);
            //assert

            Assert.Equal("Hlava", postRepository.getById(postModel.Id).Title);

            teamRepository.Remove(teamModel.Id); //should remove also the post 
            userRepository.Remove(userModel.Id);
            Assert.Null(postRepository.getById(postModel.Id));
        }

        [Fact]
        public void GetTeamPostsTest()
        {
            //arrange
            var userModel = new UserModel()
            {
                Name = "Pepa Hnátek",
                LastLogged = DateTime.Today,
                MailAddress = "bla@bla.com"
            };
            var userRepository = new UserRepository(dbContextFactory, mapper);
            userModel = userRepository.Add(userModel);

            var teamModel = new TeamModel
            {
                Description = "Tym resici ics",
                Name = "Borci"
            };
            var teamRepository = new TeamRepository(dbContextFactory, mapper);
            teamModel = teamRepository.Add(teamModel);

            teamRepository.AddUserToTeam(teamModel.Id, userModel.Id);

            var postModel = new PostModel
            {
                Title = "Hlava",
                Author = userModel,
                Text = "Ztratil jsem hlavu!",
                TimeCreated = DateTime.Now,
                Team = teamModel
            };
            var postModel2 = new PostModel
            {
                Title = "Vyvoj",
                Author = userModel,
                Text = "Už nám i zaèínají procházet testy!",
                TimeCreated = DateTime.Now,
                Team = teamModel
            };
            var postRepository = new PostRepository(dbContextFactory, mapper);
            postModel = postRepository.Add(postModel);
            postModel2 = postRepository.Add(postModel2);
            //act
            teamModel = teamRepository.GetById(teamModel.Id);
            var titles = new List<string>();
            foreach (var post in teamModel.Posts)
            {
                titles.Add(post.Title);
            }
            //assert
            Assert.Contains("Hlava", titles);
            Assert.Contains("Vyvoj", titles);
            Assert.DoesNotContain("balmsofinsadd", titles);
            Assert.Equal(2, teamModel.Posts.Count);

            teamRepository.Remove(teamModel.Id); //should remove also the posts 
            userRepository.Remove(userModel.Id);
            Assert.Null(postRepository.getById(postModel.Id));
        }

        [Fact]
        public void UserRepositoryTest()
        {
            var user1 = new User
            {
                Name = "Zbynek",
                LastLogged = DateTime.Today,
                MailAddress = "zbynda@example.com",
                PasswordHash = "aho",
                Teams = new List<UserTeam>()
            };

            var user2 = new User
            {
                Name = "Denny",
                LastLogged = DateTime.Today,
                MailAddress = "asdfasdfasdf"
            };

            var team1 = new Team
            {
                Name = "KOKOT",
                Description = "Chci dropnout",
                Posts = new List<Post>(),
                Users = new List<UserTeam>()
            };

            var team2 = new Team
            {
                Name = "LoremIpsum",
                Description = "nonsense team for user2",
                Posts = new List<Post>(),
                Users = new List<UserTeam>()
            };

            User n1 = null;
            User n2 = null;
            using (var context = dbContextFactory.CreateDbContext())
            {
                context.Users.Add(user1);
                context.Users.Add(user2);
                context.SaveChanges();
                n1 = context.Users.FirstOrDefault(u => u.UserId == user1.UserId);
                n2 = context.Users.FirstOrDefault(u => u.UserId == user2.UserId);
            }

            //user1 = context.Users.FirstOrDefault(u => u.UserId == user1.UserId);
            //user2 = context.Users.FirstOrDefault(u => u.UserId == user2.UserId);

            //test, jestli se entita správnì propojila s DB. Pøi Add by se mìla vytvoøit vazba a do userù nahráít i ID. 
            Assert.Equal(user1, n1);
            Assert.Equal(user2, n2);


            var teamRepo = new TeamRepository(dbContextFactory, mapper);
            var teamModel1 = teamRepo.Add(mapper.MapTeamToTeamModel(team1, null, null));
            var teamModel2 = teamRepo.Add(mapper.MapTeamToTeamModel(team2, null, null));



            teamRepo.AddUserToTeam(teamModel1.Id, user1.UserId);
            teamModel1 = teamRepo.GetById(teamModel1.Id);
            Assert.Equal(1, teamModel1.Members.Count); //Pokud OK, useøi se úspìšnì propisují do týmu

            Assert.Equal(teamModel1.Members.First().Id, user1.UserId);//stejne ID == stejný user z db

            teamRepo.RemoveUserFromTeam(teamModel1.Id, user1.UserId);
            teamModel1 = teamRepo.GetById(teamModel1.Id);
            Assert.Equal(0, teamModel1.Members.Count); //user uspesne vyrazen z tymu
        }

        [Fact]
        public void CompleteTest()
        {
            var user1Model = new UserModel
            {
                Name = "TestUser1",
                LastLogged = DateTime.Today,
                MailAddress = "test@user1",
                PasswordHash = "ph1",
            };
            var user2Model = new UserModel
            {
                Name = "TestUser2",
                LastLogged = DateTime.Today,
                MailAddress = "test@user2",
                PasswordHash = "ph2",
            };
            var user3Model = new UserModel
            {
                Name = "TestUser3",
                LastLogged = DateTime.Today,
                MailAddress = "test@user3",
                PasswordHash = "ph3",
            };

            var team1Model = new TeamModel
            {
                Name = "team1",
                Description = "Describtion of team1",
            };
            var team2Model = new TeamModel
            {
                Name = "team2",
                Description = "Describtion of team2",
            };
            var teamRepo = new TeamRepository(dbContextFactory, mapper);
            var userRepo = new UserRepository(dbContextFactory, mapper);

            team1Model = teamRepo.Add(team1Model);
            team2Model = teamRepo.Add(team2Model);

            user1Model = userRepo.Add(user1Model);
            user2Model = userRepo.Add(user2Model);
            user3Model = userRepo.Add(user3Model);

            Assert.NotEqual(0, user3Model.Id); //checking only one, but if the last one has propper ID, the others should have it too
            Assert.NotEqual(0, team2Model.Id);
            var getteam1 = teamRepo.GetById(team1Model.Id);
            Assert.Equal(getteam1.Id, team1Model.Id);

            teamRepo.AddUserToTeam(team1Model.Id, user3Model.Id);

            teamRepo.AddUserToTeam(team1Model.Id, user1Model.Id);
            teamRepo.AddUserToTeam(team1Model.Id, user2Model.Id);
            team1Model = teamRepo.GetById(team1Model.Id);
            user1Model = userRepo.GetById(user1Model.Id);
            user2Model = userRepo.GetById(user2Model.Id);
            user3Model = userRepo.GetById(user3Model.Id);
            //if all users are in current team
            Assert.Equal(user1Model.Teams.Count, 1);
            Assert.Equal(user2Model.Teams.Count, user3Model.Teams.Count);

            var postRepo = new PostRepository(dbContextFactory, mapper);
            var commRepo = new CommentRepository(dbContextFactory, mapper);

            var post1Model = new PostModel
            {
                Title = "Title1",
                Text = "Text1",
                TimeCreated = DateTime.Today,
                Team = team1Model,
                Author = user1Model
            };
            post1Model = (postRepo.Add(post1Model));

            var comm1Model = new CommentModel
            {
                Text = "comm1",
                Author = user3Model,
                TimeCreated = DateTime.Today,
                ParentPost = post1Model
            };
            comm1Model = (commRepo.Add((comm1Model)));

            var comm2Model = new CommentModel
            {
                Text = "comm2",
                Author = user2Model,
                TimeCreated = DateTime.Today,
                ParentPost = post1Model
            };
            comm2Model = (commRepo.Add((comm2Model)));



            //parentpost test
            Assert.Equal(post1Model.Id, comm1Model.ParentPost.Id);
            Assert.Equal(post1Model.Id, comm2Model.ParentPost.Id);

            //author and team test
            Assert.Equal(post1Model.Team.Id, team1Model.Id);
            Assert.Equal(post1Model.Author.Id, user1Model.Id);
            Assert.Equal(comm1Model.ParentPost.Id, post1Model.Id);
            Assert.Equal(comm2Model.Author.Id, user2Model.Id);

            user1Model.Name = "zmeniljsemsijmeno";
            userRepo.UpdateInfo(user1Model);
            user1Model = userRepo.GetById(user1Model.Id);
            Assert.Equal("zmeniljsemsijmeno", user1Model.Name);

            team1Model.Description = "Uz vim, co sem napisu, a bude to dloooooouheeeeeee...";
            teamRepo.UpdateInfo(team1Model);
            team1Model = teamRepo.GetById(team1Model.Id);
            Assert.Equal("Uz vim, co sem napisu, a bude to dloooooouheeeeeee...", team1Model.Description);

            teamRepo.RemoveUserFromTeam(team1Model.Id, user1Model.Id);
            user1Model = userRepo.GetById(user1Model.Id);
            Assert.Equal(0, user1Model.Teams.Count);

            teamRepo.Remove(team1Model.Id);
            team1Model = teamRepo.GetById(team1Model.Id);
            post1Model = postRepo.getById(post1Model.Id);
            comm1Model = commRepo.getById(comm1Model.Id);
            comm2Model = commRepo.getById(comm2Model.Id);
            user2Model = userRepo.GetById(user2Model.Id);


            Assert.Null(team1Model);
            Assert.Null(post1Model);
            Assert.Null(comm1Model);
            Assert.Null(comm2Model);
            Assert.Equal(0, user2Model.Teams.Count);
        }

        [Fact]
        public void FindPostsByTextTest()
        {
            //arrange
            var userModel = new UserModel()
            {
                Name = "Pepa Hnátek",
                LastLogged = DateTime.Today,
                MailAddress = "bla@bla.com"
            };
            var userRepository = new UserRepository(dbContextFactory, mapper);
            userModel = userRepository.Add(userModel);

            var teamModel = new TeamModel
            {
                Description = "Tym resici ics",
                Name = "Borci"
            };
            var teamRepository = new TeamRepository(dbContextFactory, mapper);
            teamModel = teamRepository.Add(teamModel);
            teamRepository.AddUserToTeam(teamModel.Id, userModel.Id);

            var post1 = new PostModel
            {
                Title = "Tesla",
                Author = userModel,
                Text = "Tesla je prý koneènì zisková!",
                TimeCreated = DateTime.Now,
                Team = teamModel
            };
            var post2 = new PostModel
            {
                Title = "Hlava",
                Author = userModel,
                Text = "Ztratil jsem hlavu!",
                TimeCreated = DateTime.Now,
                Team = teamModel
            };
            var postRepository = new PostRepository(dbContextFactory, mapper);
            post1 = postRepository.Add(post1);
            post2 = postRepository.Add(post2);
            var comment1 = new CommentModel
            {
                Author = userModel,
                ParentPost = post1,
                Text = "No tak to je super, tøeba vydìlá dost penìz, aby nám v Brnì postavil hyperloop",
                TimeCreated = DateTime.Now
            };
            var comment2 = new CommentModel
            {
                Author = userModel,
                ParentPost = post1,
                Text = "Jsem tu sám, tak si komentuju vlastní posty :D",
                TimeCreated = DateTime.Now
            };
            var commentRepository = new CommentRepository(dbContextFactory, mapper);
            commentRepository.Add(comment1);
            commentRepository.Add(comment2);
            //act
            var postsShouldBeFound1 = postRepository.FindByText("jsem", teamModel, false);
            var postsShouldBeFound2 = postRepository.FindByText("to je super", teamModel, true);
            var postsShouldBeFound3 = postRepository.FindByText("Hlava", teamModel, true);
            var postsShouldNotBeFound1 = postRepository.FindByText("alohomora", teamModel, true);
            var postsShouldNotBeFound2 = postRepository.FindByText("to je super", teamModel, false);
            var postsShouldNotBeFound3 = postRepository.FindByText("Teslaaaratata", teamModel, true);
            //assert
            Assert.NotEmpty(postsShouldBeFound1);
            Assert.NotEmpty(postsShouldBeFound2);
            Assert.NotEmpty(postsShouldBeFound3);
            Assert.Empty(postsShouldNotBeFound1);
            Assert.Empty(postsShouldNotBeFound2);
            Assert.Empty(postsShouldNotBeFound3);

            teamRepository.Remove(teamModel.Id); //should remove also posts and comments
            userRepository.Remove(userModel.Id);
            Assert.Null(postRepository.getById(post2.Id));
        }

        [Fact]
        public void FindCommentTest()
        {
            //arrange
            var userModel = new UserModel()
            {
                Name = "Pepa Hnátek",
                LastLogged = DateTime.Today,
                MailAddress = "bla@bla.com"
            };
            var userRepository = new UserRepository(dbContextFactory, mapper);
            userModel = userRepository.Add(userModel);

            var teamModel = new TeamModel
            {
                Description = "Tym resici ics",
                Name = "Borci"
            };
            var teamRepository = new TeamRepository(dbContextFactory, mapper);
            teamModel = teamRepository.Add(teamModel);
            teamRepository.AddUserToTeam(teamModel.Id, userModel.Id);

            var postModel = new PostModel
            {
                Title = "Hlava",
                Author = userModel,
                Text = "Ztratil jsem hlavu!",
                TimeCreated = DateTime.Now,
                Team = teamModel
            };
            var postRepository = new PostRepository(dbContextFactory, mapper);
            postModel = postRepository.Add(postModel);

            var comment1 = new CommentModel
            {
                Author = userModel,
                ParentPost = postModel,
                Text = "Hledej, dìlej!!",
                TimeCreated = DateTime.Now
            };
            var commentRepository = new CommentRepository(dbContextFactory, mapper);
            commentRepository.Add(comment1);
            //act
            var commentsShouldBeFound = commentRepository.FindByText("dìlej", teamModel);
            var commentsShouldNotBeFound = commentRepository.FindByText("alohomora", teamModel);
            //assert
            Assert.NotEmpty(commentsShouldBeFound);
            Assert.Empty(commentsShouldNotBeFound);

            teamRepository.Remove(teamModel.Id); //should remove also the post and comment
            userRepository.Remove(userModel.Id);
            Assert.Null(postRepository.getById(postModel.Id));
        }
    }
}
