using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ICS_team_4615.BL.Model
{
    //NOTE When registration, maybe new Model will be useful; something between detail and basic.
    public class TeamModel : ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<UserModel> Members { get; set; }
        public IList<PostModel> Posts { get; set; }
    }
}