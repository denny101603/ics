using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ICS_team_4615.DAL.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserTeam> Users { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
