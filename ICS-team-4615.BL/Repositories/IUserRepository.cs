using System;
using ICS_team_4615.BL.Model;

namespace ICS_team_4615.BL.Repositories
{
    public interface IUserRepository: IRepositoryBase
    {
        UserModel GetById(int id);
        void UpdateInfo(UserModel userModel);
        UserModel Add(UserModel userModel);
        UserModel GetByMail(String mail);
    }
}