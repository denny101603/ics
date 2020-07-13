using System.Collections.Generic;

namespace ICS_team_4615.DAL.Entities
{
    public class Post : PostBase
    {
        public string Title { get; set; }
        public Team Team { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
