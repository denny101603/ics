using ICS_team_4615.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Tests
{
    public class InMemoryDbContextFactory : IDesignTimeDbContextFactory<TeamsDbContext>, IDbContextFactory
    {
        public TeamsDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TeamsDbContext>();
            optionsBuilder.UseInMemoryDatabase("TodoDbName");
            return new TeamsDbContext(optionsBuilder.Options);
        }

        public TeamsDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }
    }
}