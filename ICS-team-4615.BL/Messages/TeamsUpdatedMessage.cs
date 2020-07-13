using System;
using System.Collections.Generic;
using System.Text;
using ICS_team_4615.BL.Model;

namespace ICS_team_4615.BL.Messages
{
    public class TeamsUpdatedMessage : IMessage
    {
        public TeamModel team;
    }
}
