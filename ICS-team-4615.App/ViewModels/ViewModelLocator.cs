using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Repositories;
using ICS_team_4615.BL.Services;
using ICS_team_4615.DAL;

namespace ICS_team_4615.App.ViewModels
{
    class ViewModelLocator 
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly IDbContextFactory dbContextFactory;
        private readonly IUserRepository userRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;


        public UserViewModel UserViewModel => new UserViewModel(mediator, userRepository);
        public TeamViewModel TeamViewModel => new TeamViewModel(mediator, teamRepository,dbContextFactory,mapper);
        public PostViewModel PostViewModel => new PostViewModel(mediator, postRepository,dbContextFactory,mapper,true);
        public CommentViewModel CommentViewModel => new CommentViewModel(commentRepository,mediator,true);
        public HomeViewModel HomeViewModel => new HomeViewModel(mediator);


        public ViewModelLocator()
        {
            mapper = new Mapper();
            mediator = new Mediator();
            dbContextFactory = new DesignTimeDbContextFactory();
            userRepository = new UserRepository(dbContextFactory, mapper);
            teamRepository = new TeamRepository(dbContextFactory, mapper);
            postRepository = new PostRepository(dbContextFactory,mapper);
            commentRepository = new CommentRepository(dbContextFactory,mapper);
        }
    }
}
