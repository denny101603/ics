using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using ICS_team_4615.App.Command;
using ICS_team_4615.BL.Extensions;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Messages;
using ICS_team_4615.BL.Model;
using ICS_team_4615.BL.Repositories;
using ICS_team_4615.BL.Services;
using ICS_team_4615.DAL;
using Xceed.Wpf.Toolkit;

namespace ICS_team_4615.App.ViewModels
{
    public class PostViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly IDbContextFactory _dbContext;
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private string _text;
        private string _title;
        private string _author;
        private DateTime _timeCreated;
        private UserModel _loggedUser;
        private int _loadingCommentIdx;


        public string Title {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        public DateTime TimeCreated
        {
            get => _timeCreated;
            set
            {
                _timeCreated = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CommentViewModel> Comments { get; set; } = new ObservableCollection<CommentViewModel>(); 

        public PostModel Model { get; set; }

        public ICommand AddCommentCommand { get; set; }

        

        public PostViewModel(IMediator mediator,IPostRepository postRepository, IDbContextFactory dbContext, IMapper mapper,bool newPost)
        {
            _postRepository = postRepository;
            this._dbContext = dbContext;
            this._mapper = mapper;
            _mediator = mediator;
            if (newPost)
            {
                _mediator.Register<LoadPostMessage>(LoadPost);
                _mediator.Send(new PostLoadedMessage());
            }
            
            _mediator.Register<SendLoggedUserMessage>(SetLoggedUser);
            _mediator.Register<CommentLoadedMessage>(CommentLoaded);
            AddCommentCommand = new RelayCommand((text) => AddComment((Xceed.Wpf.Toolkit.RichTextBox)text));
        }

        private void CommentLoaded(CommentLoadedMessage obj)
        {
            if (_loadingCommentIdx < Comments.Count)
            {
                var postId = Comments[_loadingCommentIdx].Model.Id;
                _mediator.Send(new AddCommentMessage { id = postId });
                _loadingCommentIdx += 1;
            }
        }

        private void SetLoggedUser(SendLoggedUserMessage obj)
        {
            _loggedUser = obj.User;
        }

        private void AddComment(Xceed.Wpf.Toolkit.RichTextBox textBox)
        {
            var newComment = new CommentViewModel(new CommentRepository(_dbContext,_mapper),_mediator,false);
            _mediator.Send(new GetLoggedUserMessage());
            newComment.Create(_loggedUser,Model,textBox.Text);
            textBox.Clear();
            _loadingCommentIdx = Comments.Count;
            newComment.SaveNewComment();
            Model.LastCommentedOrCreated = DateTime.Now;
            TimeCreated = Model.LastCommentedOrCreated;
            Comments.Add(newComment);
            _mediator.Send(new RefreshMessage());
            LoadComments();
        }

        private void LoadPost(LoadPostMessage obj)
        {
            if (Model == null)
            {
                Load(obj.id);
            }
        }


        public void LoadComments()
        {
            Comments.Clear();
            _loadingCommentIdx = 0;
            foreach (var comment in Model.Comments)
            {
                var model = new CommentViewModel(new CommentRepository(_dbContext, _mapper),_mediator,false) { Model = comment };
                Comments.Add(model);
            }
        }


        /// <summary>
        /// Metoda pouze vytvoří instanci modelu, pro její uložení je ještě potřeba použít metodu SaveNewPost()
        /// </summary>
        /// <param name="author">Autor příspěvku</param>
        /// <param name="team">Tým, do kterého příspěvěk náleží</param>
        /// <param name="title">Nadpis příspěvku</param>
        /// <param name="text">Text příspěvku</param>
        public void Create(UserModel author, TeamModel team, string title, string text)
        {
            Model = new PostModel
            {
                Author = author,
                Team = team,
                Text = text,
                Title = title,
                TimeCreated = DateTime.Now
            };
        }

        public void SaveNewPost()
        {
            //příspěvky není možné upravovat
            if (Model.Id == default(int))
            {
                Model = _postRepository.Add(Model);
            }
        }

        private bool CanSave()
        {
            return Model != null
                   && !string.IsNullOrWhiteSpace(Model.Title)
                   && !string.IsNullOrWhiteSpace(Model.Text)
                   && Model.Team != null
                   && Model.Author != null
                   && Model.TimeCreated != default(DateTime);
        }
        
        public void Load(int id)
        {
            Model = _postRepository.getById(id);
            Title = Model.Title;
            Text = Model.Text;
            Author = Model.Author.Name;
            TimeCreated = Model.LastCommentedOrCreated;
            Comments.Clear();
            LoadComments();
        }
    }
}