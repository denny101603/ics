namespace ICS_team_4615.BL.Model
{
    public class CommentModel : PostBaseModel
    {
        public PostModel ParentPost { get; set; }
    }
}