using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models.Repositories
{
    public class AgentRepository : ICenterRepository<Agent>
    {
        private readonly AppDbContext context;

        public AgentRepository(AppDbContext _context)
        {
            context = _context;
        }
        public void Create(Agent entity)
        {
            context.Agents.Add(entity);
            context.SaveChanges();
        }

        public Agent Delete(Guid Id)
        {
            var agent = Get(Id);
            if (agent != null)
            {
                context.Agents.Remove(agent);
                context.SaveChanges();
            }
            return agent;
        }

        public Agent Get(Guid Id)
        {
            var agent = context.Agents.SingleOrDefault(a => a.Id == Id);
            return agent;
        }

        public IEnumerable<Agent> GetList()
        {
            return context.Agents;
        }

        public Agent Update(Agent entityChanges)
        {
            var agent = context.Agents.Attach(entityChanges);
            agent.State = EntityState.Modified;
            context.SaveChanges();
            return entityChanges;
        }
    }
}
