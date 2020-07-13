using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ICS_team_4615.BL.Model
{
    public class UserModel : ModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string MailAddress { get; set; }
        public DateTime LastLogged { get; set; }
        public string PasswordHash { get; set; }
        public ObservableCollection<TeamModel> Teams { get; set; }
    }
}