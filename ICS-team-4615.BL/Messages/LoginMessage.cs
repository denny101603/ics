using System;
using System.Collections.Generic;
using System.Text;

namespace ICS_team_4615.BL.Messages
{
    public class LoginMessage : IMessage
    {
        public int Id { get; set; }
        public DateTime LastLogged { get; set; }
    }
}
