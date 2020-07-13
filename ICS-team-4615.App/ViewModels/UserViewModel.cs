using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ICS_team_4615.App.Command;
using ICS_team_4615.BL.Model;
using ICS_team_4615.BL.Repositories;
using ICS_team_4615.BL.Messages;
using ICS_team_4615.BL.Services;

namespace ICS_team_4615.App.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepo;
        public UserModel UserModel { get; set; }

        public ICommand TeamSelectedCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand CreateTeamCommand { get; set; }
        public ICommand RegisterWithAddCommand { get; set; }
        private string _loginMail;
        private string _loginErrorMessage;
        private string _registerMail;
        private string _registerName;
        private string _registerMessage;
        private SolidColorBrush _registerMessageColor;
        private string _name;
        private DateTime _lastLogged;

        public string LoginMail
        {
            get => _loginMail;
            set
            {
                _loginMail = value;
                OnPropertyChanged();
            }
        }
        public string LoginErrorMessage
        {
            get => _loginErrorMessage;
            set
            {
                _loginErrorMessage = value;
                OnPropertyChanged();
            }
        }
        public string RegisterMail
        {
            get => _registerMail;
            set
            {
                _registerMail = value;
                OnPropertyChanged();
            }
        }
        public string RegisterName
        {
            get => _registerName;
            set
            {
                _registerName = value;
                OnPropertyChanged();
            }
        }
        public string RegisterMessage
        {
            get => _registerMessage;
            set
            {
                _registerMessage = value;
                OnPropertyChanged();
            }
        }
        public SolidColorBrush RegisterMessageColor
        {
            get => _registerMessageColor;
            set
            {
                _registerMessageColor = value;
                OnPropertyChanged();
            }
        }
        public string Name {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public DateTime LastLogged {
            get => _lastLogged;
            set
            {
                _lastLogged = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TeamModel> Teams {get; set;} = new ObservableCollection<TeamModel>();

        public UserViewModel(IMediator mediator,IUserRepository userRepository)
        {
            this._mediator = mediator;
            this._userRepo = userRepository;
            TeamSelectedCommand = new RelayCommand<TeamModel>(TeamSelected);
            UserModel = new UserModel();

            _mediator.Register<TeamsUpdatedMessage>(UpdateTeams);
            _mediator.Register<LoginMessage>(LoginPress);
            _mediator.Register<GetLoggedUserMessage>(GetLoggedUser);
            _mediator.Register<RefreshMessage>(Refresh);

            LoginCommand = new RelayCommand((passwordBox) => Login((PasswordBox)passwordBox));
            RegisterCommand = new RelayCommand<object>((passwordBox) => Register((PasswordBox)passwordBox));
            LogoutCommand = new RelayCommand(Logout);
            CreateTeamCommand = new RelayCommand(CreateTeam);
            RegisterWithAddCommand = new RelayCommand((passwordBox) => RegisterWithAdd((PasswordBox)passwordBox));
        }

        private void Refresh(RefreshMessage obj)
        {
            Load(UserModel.Id);
            LoadTeams();
        }

        private void GetLoggedUser(GetLoggedUserMessage obj)
        {
            _mediator.Send(new SendLoggedUserMessage{User = UserModel});
        }

        private void RegisterWithAdd(PasswordBox passwordBox)
        {
            Register(passwordBox);
            _mediator.Send(new AddCreatedUserToTeamMessage{UserModel = UserModel});
            _mediator.Send(new CancelMessage());
        }

        private void UpdateTeams(TeamsUpdatedMessage obj)
        {
            Load(UserModel.Id);
            LoadTeams();
        }

        private void LoadTeams()
        {
            Teams.Clear();
            foreach (var team in UserModel.Teams)
            {
                Teams.Add(team);
            }
        }
        private void CreateTeam()
        {
            _mediator.Send(new CreateTeamMessage{ Id = UserModel.Id });
        }

        private void Logout()
        {
            _mediator.Send(new LogoutMessage());
        }

        private void LoginPress(LoginMessage obj)
        {
            Load(obj.Id);
            Name = UserModel.Name;
            LastLogged = obj.LastLogged;
            LoadTeams();
        }

        private void TeamSelected(TeamModel team)
        {
            _mediator.Send(new TeamSelectedMessage
            {
                TeamId = team.Id,
                User = UserModel
            });
        }


        public void UpdateUsername(string name)
        {
            UserModel.Name = name;
            Save();
        }

        public void Load(int id)
        {
            UserModel = _userRepo.GetById(id);
        }

        public void Save()
        {
            if (UserModel.Id == 0)
            {
                UserModel = _userRepo.Add(UserModel);
            }
            else
            {
                _userRepo.UpdateInfo(UserModel);
            }
        }

        public void Delete()
        {
            _userRepo.Remove(UserModel.Id);
        }

        public void Register(PasswordBox passwordBox)
        {
            var password = passwordBox.Password;
            if (RegisterMail == "" || RegisterName == "" || password == "")
            {
                RegisterMail = "";
                passwordBox.Clear();
                RegisterName = "";
                RegisterMessage = "Registration Error, set all values!";
                RegisterMessageColor = new SolidColorBrush(Colors.Red);
                return;
            }
            UserModel = _userRepo.GetByMail(_registerMail);
            if (UserModel != null)
            {
                RegisterMessage = "Registration Error";
                RegisterMessageColor = new SolidColorBrush(Colors.Red);
                RegisterMail = "";
                passwordBox.Clear();
                RegisterName = "";
                return;
            }

            UserModel = new UserModel
            {
                Name = _registerName,
                MailAddress = _registerMail,
                PasswordHash = HidePswd(password)
            };
            Save();
            RegisterMessage = "Registration successful";
            RegisterMessageColor = new SolidColorBrush(Colors.LawnGreen);
            RegisterMail = "";
            passwordBox.Clear();
            RegisterName = "";
            return;
        }

        private void Login(PasswordBox passwordBox)
        {
            var password = passwordBox.Password;
            UserModel = _userRepo.GetByMail(_loginMail);
            if (UserModel == null || CheckPswd(UserModel.PasswordHash, password))
            {
                LoginErrorMessage = "Login Error";
                return;
            }

            LoginMail = "";
            passwordBox.Clear();
            DateTime lastLogged;
            if (UserModel.LastLogged == default(DateTime))
            {
                lastLogged = DateTime.Now;
            }
            else
            {
               lastLogged = UserModel.LastLogged;
            }
            
            UserModel.LastLogged = DateTime.Now;
            Save();
            _mediator.Send(new LoginMessage{ Id = UserModel.Id, LastLogged = lastLogged});
            return;
        }



        /*
         * Funkce zahešuje heslo do podoby, ve které se dá uložit do Db
         */
        private string HidePswd(string password)
        {
            //Generate salt
            byte[] salt = new byte[16];
            var saltGenerator = new RNGCryptoServiceProvider();
            saltGenerator.GetBytes(salt);

            //generate hash
            var hashGenerator = new Rfc2898DeriveBytes(password, salt, 4096);
            //How much operations needed to generate hash. More = better but slower. Should be between 1k and 10k

            var hash = hashGenerator.GetBytes(16); //Length of the hash. 16 is OK

            byte[] stored = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, stored, 0, salt.Length);
            Array.Copy(hash, 0, stored, salt.Length, hash.Length);

            string storedsalthash = Convert.ToBase64String(stored);
            return storedsalthash;
        }


        /*
         * Funkce overi, zda se zadany string shoduje s heslem v Db
         */
        private bool CheckPswd(string saltHash, string testPassword)
        {
            byte[] byteSaltHash = Convert.FromBase64String(saltHash);
            byte[] saltFromDb = new byte[16];
            Array.Copy(byteSaltHash, 0, saltFromDb, 0, 16);
            byte[] hashFromDb = new byte[16];
            Array.Copy(byteSaltHash, 16, hashFromDb, 0, 16);
            var hashGeneratorNew = new Rfc2898DeriveBytes(testPassword, saltFromDb, 4096);
            var newHash = hashGeneratorNew.GetBytes(20);

            var err = false;
            for (int i = 0; i < 16; i++)
            {
                if (newHash[i] != hashFromDb[i])
                {
                    err = true;
                }
            }

            return err;
        }
    }
}