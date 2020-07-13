using System.Collections.Generic;
using ICS_team_4615.BL.Model;

namespace ICS_team_4615.BL.Repositories
{
    public interface ITeamRepository: IRepositoryBase
    {
        TeamModel GetById(int id);
        void UpdateInfo(TeamModel teamModel);
        TeamModel Add(TeamModel teamModel);

        void RemoveUserFromTeam(int teamModelId, int userId);
        void AddUserToTeam(int teamModelId, int userId);
    }
}