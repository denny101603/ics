using System;
using System.Collections.Generic;
using System.Text;
using ICS_team_4615.BL.Model;

namespace ICS_team_4615.BL.Messages
{
    public class FindMessage : IMessage
    {
        public List<PostModel> res { get; set; }
        public string Text { get; set; }
    }
}
