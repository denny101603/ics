using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ICS_team_4615.DAL
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TeamsDbContext>, IDbContextFactory
    {
        public TeamsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TeamsDbContext>();
            optionsBuilder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog = TasksDB; MultipleActiveResultSets = True; Integrated Security = True");
            return new TeamsDbContext(optionsBuilder.Options);
        }

        public TeamsDbContext CreateDbContext()
        {
            return CreateDbContext(null);
        }
    }
}
