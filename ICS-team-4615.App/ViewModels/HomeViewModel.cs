using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ICS_team_4615.BL.Messages;
using ICS_team_4615.BL.Services;

namespace ICS_team_4615.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private IMediator _mediator;
        private Visibility _homeVisibility;
        private Visibility _loginVisibility;
        private Visibility _createTeamVisibility;
        private Visibility _addMemberVisibility;
        private Visibility _editTeamVisibility;
        private Visibility _findVisibility;
        private Visibility _createUserVisibility;
        public Visibility HomeVisibility
        {
            get => _homeVisibility;
            set
            {
                _homeVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility LoginVisibility
        {
            get => _loginVisibility;
            set
            {
                _loginVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility CreateTeamVisibility {
            get => _createTeamVisibility;
            set
            {
                _createTeamVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility CreateUserVisibility {
            get => _createUserVisibility;
            set
            {
                _createUserVisibility = value;
                OnPropertyChanged();
            }
        }

	    public Visibility AddMemberVisibility {
            get => _addMemberVisibility;
            set
            {
                _addMemberVisibility = value;
                OnPropertyChanged();
            }
        }

	    public Visibility EditTeamVisibility {
            get => _editTeamVisibility;
            set
            {
                _editTeamVisibility = value;
                OnPropertyChanged();
            }
        }

	    public Visibility FindVisibility {
            get => _findVisibility;
            set
            {
                _findVisibility = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel(IMediator mediator)
        {
            _mediator = mediator;
            HomeVisibility = Visibility.Collapsed;
            CreateTeamVisibility = Visibility.Hidden;
            LoginVisibility = Visibility.Visible;
            AddMemberVisibility = Visibility.Hidden;
            EditTeamVisibility = Visibility.Hidden;
            FindVisibility = Visibility.Hidden;
            CreateUserVisibility = Visibility.Hidden;
           
            _mediator.Register<LogoutMessage>(LogoutPress);
            _mediator.Register<LoginMessage>(LoginPress);
            _mediator.Register<CreateTeamMessage>(CreateTeamPress);
            _mediator.Register<AddMemberMessage>(AddMemberPress);
            _mediator.Register<EditTeamMessage>(EditTeamPress);
            _mediator.Register<FindMessage>(FindPress);
            _mediator.Register<CancelMessage>(CancelPress);
            _mediator.Register<CreateUserMessage>(CreateUserPress);
        }

        private void CreateTeamPress(CreateTeamMessage obj)
        {
            HomeVisibility = Visibility.Collapsed;
            CreateTeamVisibility = Visibility.Visible;
        }

        private void CreateUserPress(CreateUserMessage obj)
        {
            HomeVisibility = Visibility.Collapsed;
            CreateUserVisibility = Visibility.Visible;
        }

	    private void AddMemberPress(AddMemberMessage obj)
        {
            HomeVisibility = Visibility.Collapsed;
            AddMemberVisibility = Visibility.Visible;
        }

	    private void EditTeamPress(EditTeamMessage obj)
        {
            HomeVisibility = Visibility.Collapsed;
            EditTeamVisibility = Visibility.Visible;
        }

	    private void FindPress(FindMessage obj)
        {
            FindVisibility = Visibility.Visible;
        }

        private void LogoutPress(LogoutMessage message)
        {
            HomeVisibility = Visibility.Collapsed;
            LoginVisibility = Visibility.Visible;
        }

        private void LoginPress(LoginMessage message)
        {
            HomeVisibility = Visibility.Visible;
            LoginVisibility = Visibility.Hidden;
        }

        private void CancelPress(CancelMessage message)
        {
            HomeVisibility = Visibility.Visible;
            CreateTeamVisibility = Visibility.Hidden;
            AddMemberVisibility = Visibility.Hidden;
            EditTeamVisibility = Visibility.Hidden;
            FindVisibility = Visibility.Hidden;
            CreateUserVisibility = Visibility.Hidden;
        }
    }
}
