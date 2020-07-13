using System;
using System.Collections.Generic;
using ICS_team_4615.DAL.Entities;

namespace ICS_team_4615.BL.Model
{
    public class PostModel : PostBaseModel
    {
        public string Title { get; set; }
        public TeamModel Team { get; set; }
        public DateTime LastCommentedOrCreated { get; set; } //pokud prispevek neni okomentovan, v teto promenne se nachazi cas jeho vytvoreni
        public IList<CommentModel> Comments { get; set; }
    }
}