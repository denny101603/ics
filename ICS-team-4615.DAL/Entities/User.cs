using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICS_team_4615.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string MailAddress { get; set; }
        public DateTime LastLogged { get; set; }
        public string PasswordHash { get; set; }
        public virtual ICollection<UserTeam> Teams { get; set; }
    }
}