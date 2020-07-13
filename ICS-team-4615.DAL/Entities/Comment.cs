namespace ICS_team_4615.DAL.Entities
{
    public class Comment : PostBase
    {
        public Post ParentPost { get; set; }
    }
}