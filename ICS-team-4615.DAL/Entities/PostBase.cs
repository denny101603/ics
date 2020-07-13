using System;

namespace ICS_team_4615.DAL.Entities
{
    public abstract class PostBase : EntityBase
    {
        public string Text { get; set; }
        public User Author { get;set; }
        public DateTime TimeCreated { get; set; }
    }
}