using System;
using System.Collections.Generic;

namespace ICS_team_4615.BL.Model
{
    public class PostBaseModel : ModelBase
    {
        public string Text {get; set; }
        public UserModel Author {get; set; }
        public DateTime TimeCreated { get; set; }
    }
}