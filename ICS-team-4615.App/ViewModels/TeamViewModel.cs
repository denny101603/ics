using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using ICS_team_4615.App.Command;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Messages;
using ICS_team_4615.BL.Model;
using ICS_team_4615.BL.Repositories;
using ICS_team_4615.BL.Services;
using ICS_team_4615.DAL;
using MaterialDesignThemes.Wpf;

namespace ICS_team_4615.App.ViewModels
{
    public class TeamViewModel : ViewModelBase
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IMediator _mediator;
        private string _name;
        private string _description;
        private string _createName;
        private string _createDescription;
        private int _createId;
        private readonly IDbContextFactory _dbContext;
        private readonly IMapper _mapper;

        private UserModel _sender;
        private string _newPostText;
        private string _newPostTitle;
        private int _loadingPostIdx;
        private PackIcon _refreshIcon = new PackIcon { Kind = PackIconKind.Refresh };
        private string _errorMessage;
        private string _findingText;
        private bool _postFromFind;
        public ObservableCollection<UserModel> Members { get; set; } = new ObservableCollection<UserModel>();
        public ObservableCollection<PostViewModel> PostsViewModels { get; set; } = new ObservableCollection<PostViewModel>();
        public ObservableCollection<PostModel> FoundModels { get; set; } = new ObservableCollection<PostModel>(); 
        public ObservableCollection<PostViewModel> FoundViewModels { get; set; } = new ObservableCollection<PostViewModel>(); 
        public TeamModel TeamModel { get; set; }
        public ICommand RemoveUserFromTeamCommand { get; set; }
        public ICommand CreateCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand FindByTextCommand { get; set; }
        public ICommand CreatePostCommand { get; set; }
        public ICommand EditTeamCommand { get; set; }
        public ICommand EditNameCommand { get; set; }
        public ICommand EditDescriptionCommand { get; set; }
        public ICommand AddUserToTeamCommand { get; set; }
        public ICommand AddUserPressCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public string NewPostText
        {
            get => _newPostText;
            set
            {
                _newPostText = value;
                OnPropertyChanged();
            }
        }

        public string NewPostTitle
        {
            get => _newPostTitle;
            set
            {
                _newPostTitle = value;
                OnPropertyChanged();
            }
        }
        public string CreateName
        {
            get => _createName;
            set
            {
                _createName = value;
                OnPropertyChanged();
            }
        }

        public string CreateDescription
        {
            get => _createDescription;
            set
            {
                _createDescription = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public PackIcon RefreshIcon
        {
            get => _refreshIcon;
            set
            {
                _refreshIcon = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public string FindingText
        {
            get => _findingText;
            set
            {
                _findingText = value;
                OnPropertyChanged();
            }
        }

        public TeamViewModel(IMediator mediator, ITeamRepository teamRepository, IDbContextFactory dbContext, IMapper mapper)
        {
            TeamModel = new TeamModel();
            this._mediator = mediator;
            _teamRepo = teamRepository;
            this._dbContext = dbContext;
            this._mapper = mapper;

            _mediator.Register<TeamSelectedMessage>(TeamSelected);
            _mediator.Register<CreateTeamMessage>(AddCreatorToTeam);
            _mediator.Register<PostLoadedMessage>(PostLoaded);
            _mediator.Register<AddCreatedUserToTeamMessage>(AddCreatedUserToTeam);
            _mediator.Register<LogoutMessage>(LogOut);
            _mediator.Register<RefreshMessage>(Refresh);
            _mediator.Register<FindMessage>(FindByT); 

            CancelCommand = new RelayCommand(Cancel);
            CreateCommand = new RelayCommand(Create);
            CreatePostCommand = new RelayCommand(CreatePost);
            EditTeamCommand = new RelayCommand(EditTeam);
            AddUserPressCommand = new RelayCommand(AddUserPress);
            AddUserToTeamCommand = new RelayCommand((user) => AddUserToTeamByMail((TextBox)user));
            RemoveUserFromTeamCommand = new RelayCommand((user) => RemoveUserFromTeam((UserModel)user));
            EditNameCommand = new RelayCommand((name) => UpdateTeamName((TextBox)name));
            EditDescriptionCommand = new RelayCommand((description) => UpdateTeamDescription((TextBox)description));
            RefreshCommand = new RelayCommand(RefreshSend);
            FindByTextCommand = new RelayCommand((text) => FindBy((TextBox)text));
        }

        private void Refresh(RefreshMessage obj)
        {
            if (TeamModel == null)
            {
                return;
            }
            Load(TeamModel.Id);
            Name = TeamModel.Name;
            Description = TeamModel.Description;
            LoadMembers();
            PostsViewModels.Clear();
            _loadingPostIdx = 0;
            foreach (var post in TeamModel.Posts)
            {
                var model = new PostViewModel(_mediator, new PostRepository(_dbContext, _mapper), _dbContext, _mapper, false) { Model = post };
                model.LoadComments();
                PostsViewModels.Add(model);
            }
        }

        private void FindByT(FindMessage obj)
        {
            FoundViewModels.Clear();
            _loadingPostIdx = 0;
            _postFromFind = true;
            foreach (var post in obj.res)
            {
                var model = new PostViewModel(_mediator, new PostRepository(_dbContext, _mapper), _dbContext, _mapper, false) { Model = post };
                model.Load(post.Id);
                model.LoadComments();
                FoundViewModels.Add(model);
            }

            FindingText = "Results for finding \"" + obj.Text +"\"";
        }

        public void FindBy(TextBox text)
        {
            string textToSearch = text.Text;
            List<PostModel> results = FindByText(textToSearch);
            _mediator.Send(new FindMessage { res = results, Text = textToSearch});
            text.Clear();
        }

        private void RefreshSend()
        {
            _mediator.Send(new RefreshMessage());
        }

        private void LogOut(LogoutMessage obj)
        {
            Members.Clear();
            PostsViewModels.Clear();
            Description = "";
            Name = "";
        }

        private void AddCreatedUserToTeam(AddCreatedUserToTeamMessage obj)
        {
            if (obj.UserModel != null)
            {
                AddUserToTeam(obj.UserModel.Id);
                obj.UserModel = null;
                TeamModel.Members.Add(obj.UserModel);
            }
        }

        private void PostLoaded(PostLoadedMessage obj)
        {
            if (_postFromFind)
            {
                if (_loadingPostIdx < FoundViewModels.Count)
                {
                    var postId = FoundViewModels[_loadingPostIdx].Model.Id;
                    _mediator.Send(new LoadPostMessage { id = postId });
                    _loadingPostIdx += 1;
                }
                if (_loadingPostIdx == FoundViewModels.Count)
                {
                    _postFromFind = false;
                }
            }
            else
            {
                if (_loadingPostIdx < PostsViewModels.Count)
                {
                    var postId = PostsViewModels[_loadingPostIdx].Model.Id;
                    _mediator.Send(new LoadPostMessage { id = postId });
                    _loadingPostIdx += 1;
                }
            }
        }

        private void AddUserPress()
        {
            _mediator.Send(new AddMemberMessage());
        }

        private void EditTeam()
        {
            _mediator.Send(new EditTeamMessage());
        }

        public void LoadMembers()
        {
            Members.Clear();
            foreach (var member in TeamModel.Members)
            {
                Members.Add(member);
            }
        }

        private void CreatePost()
        {
            if (TeamModel == null || TeamModel.Id == 0)
            {
                NewPostText = "";
                NewPostTitle = "";
                return;
            }

            if (NewPostText == "" || NewPostTitle == "")
            {
                return;
            }
            var newPost = new PostViewModel(_mediator, new PostRepository(_dbContext, _mapper), _dbContext, _mapper, false);
            newPost.Create(_sender, TeamModel, NewPostTitle, NewPostText);
            _loadingPostIdx = 0;
            NewPostText = "";
            NewPostTitle = "";
            newPost.SaveNewPost();
            TeamModel.Posts.Insert(0, newPost.Model);
            PostsViewModels.Insert(0, newPost);
        }

        private void AddCreatorToTeam(CreateTeamMessage obj)
        {
            _createId = obj.Id;
        }

        private void Cancel()
        {
            CreateName = "";
            CreateDescription = "";
            ErrorMessage = "";
            _mediator.Send(new CancelMessage());
        }

        private void TeamSelected(TeamSelectedMessage obj)
        {
            Load(obj.TeamId);
            _sender = obj.User;
            Name = TeamModel.Name;
            Description = TeamModel.Description;
            LoadMembers();
            PostsViewModels.Clear();
            _loadingPostIdx = 0;
            foreach (var post in TeamModel.Posts)
            {
                var model = new PostViewModel(_mediator, new PostRepository(_dbContext, _mapper), _dbContext, _mapper, false) { Model = post };
                model.LoadComments();
                PostsViewModels.Add(model);
            }
        }

        public void Load(int id)
        {
            TeamModel = _teamRepo.GetById(id);
        }

        public void Save()
        {
            if (TeamModel.Id == 0)
            {
                TeamModel = _teamRepo.Add(TeamModel);
            }
            else
            {
                _teamRepo.UpdateInfo(TeamModel);
            }
        }

        public void Delete()
        {
            _teamRepo.Remove(TeamModel.Id);
        }

        public void Create()
        {
            TeamModel = new TeamModel
            {
                Name = CreateName,
                Description = CreateDescription,
            };
            Save();
            AddUserToTeam(_createId);
            CreateName = "";
            CreateDescription = "";
            _mediator.Send(new CancelMessage());
            _mediator.Send(new TeamsUpdatedMessage { team = TeamModel });
        }

        public bool DeleteTeam()
        {
            if (TeamModel.Id != 0)
            {
                _teamRepo.Remove(TeamModel.Id);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<PostModel> FindByText(string text)
        {
            PostRepository postRepo = new PostRepository(new DesignTimeDbContextFactory(), new Mapper());
            return postRepo.FindByText(text, this.TeamModel, true);
        }
        public void AddUserToTeamByMail(TextBox mail)
        {
            var userRepository = new UserRepository(_dbContext, _mapper);
            var user = userRepository.GetByMail(mail.Text);
            if (user == null)
            {
                ErrorMessage = "User with mail \""+ mail.Text +"\" does not exist!";
                return;
            }

            foreach (var member in TeamModel.Members)
            {
                if (member.Id == user.Id)
                {
                    ErrorMessage = "User with mail \"" + mail.Text + "\" is already in team!";
                    return;
                }
            }
            AddUserToTeam(user.Id);
            mail.Text = "";
            _mediator.Send(new RefreshMessage());
            _mediator.Send(new CancelMessage());
        }

        public void AddUserToTeam(int userId)
        {
            _teamRepo.AddUserToTeam(this.TeamModel.Id, userId);
        }

        public void RemoveUserFromTeam(UserModel user)
        {
            if (user == null)
            {
                return;
            }
            _teamRepo.RemoveUserFromTeam(this.TeamModel.Id, user.Id);
            Members.Remove(user);
            if (user.Id == _sender.Id)
            {
                Members.Clear();
                PostsViewModels.Clear();
                Description = "";
                Name = "";
            }
            _mediator.Send(new RefreshMessage());
            if (TeamModel.Members.Count == 0)
            {
                DeleteTeam();
            }
        }

        public void UpdateTeamName(TextBox name)
        {
            TeamModel.Name = name.Text;
            Save();
            _mediator.Send(new TeamsUpdatedMessage { team = TeamModel });
            _mediator.Send(new CancelMessage());
        }
        public void UpdateTeamDescription(TextBox description)
        {
            TeamModel.Description = description.Text;
            Save();
            _mediator.Send(new TeamsUpdatedMessage { team = TeamModel });
            _mediator.Send(new CancelMessage());
        }

    }
}