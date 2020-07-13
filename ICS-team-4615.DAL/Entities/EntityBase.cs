using System;
using ICS_team_4615.DAL.Entities.Interfaces;

namespace ICS_team_4615.DAL.Entities
{
    public abstract class EntityBase : IEntity
    {
        public int Id { get; set; }
    }
}