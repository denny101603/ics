using System;
using ICS_team_4615.BL.Model;

namespace ICS_team_4615.BL.Messages
{
    public class TeamSelectedMessage : IMessage
    {
        public int TeamId { get; set; }
        public UserModel User { get; set; }

    }
}
