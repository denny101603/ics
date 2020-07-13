using ICS_team_4615.DAL.Entities;

namespace ICS_team_4615.DAL
{
    public class UserTeam
    {
        public int userId { get; set; }
        public User user { get; set; }

        public int teamId { get; set; }
        public Team team { get; set; }

    }
}