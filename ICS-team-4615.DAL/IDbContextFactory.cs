using System;
using System.Collections.Generic;
using System.Text;

namespace ICS_team_4615.DAL
{
    public interface IDbContextFactory
    {
        TeamsDbContext CreateDbContext();
    }
}