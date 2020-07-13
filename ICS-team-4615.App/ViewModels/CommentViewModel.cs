using System;
using ICS_team_4615.BL.Mapper;
using ICS_team_4615.BL.Messages;
using ICS_team_4615.BL.Model;
using ICS_team_4615.BL.Repositories;
using ICS_team_4615.BL.Services;
using ICS_team_4615.DAL;

namespace ICS_team_4615.App.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMediator _mediator;

        public CommentModel Model { get; set; }

        public CommentViewModel(ICommentRepository commentRepository, IMediator mediator, bool newComment)
        {
            _commentRepository = commentRepository;
            _mediator = mediator;
            if (newComment)
            {
                _mediator.Register<AddCommentMessage>(LoadComment);
                _mediator.Send(new CommentLoadedMessage());
            }
        }

        private void LoadComment(AddCommentMessage obj)
        {
            if (Model == null)
            {
                Load(obj.id);
            }
        }

        /// <summary>
        /// Metoda pouze vytvoří instanci modelu, pro její uložení je ještě potřeba použít metodu SaveNewComment()
        /// </summary>
        /// <param name="author">Autor komentáře</param>
        /// <param name="postCommented">Na který post tento comment reaguje</param>
        /// <param name="text">Obsah komentáře</param>
        public void Create(UserModel author, PostModel postCommented, string text)
        {
            Model = new CommentModel
            {
                Author = author,
                Text = text,
                TimeCreated = DateTime.Now,
                ParentPost = postCommented
            };
        }

        public void SaveNewComment()
        {
            //comment není možné upravovat
            if (Model.Id == default(int))
            {
                Model = _commentRepository.Add(Model);
            }
        }

        private bool CanSave()
        {
            return Model != null
                   && !string.IsNullOrWhiteSpace(Model.Text)
                   && Model.ParentPost != null
                   && Model.Author != null
                   && Model.TimeCreated != default(DateTime);
        }
        public void Load(int id)
        {
            Model = _commentRepository.getById(id);
        }
    }
}