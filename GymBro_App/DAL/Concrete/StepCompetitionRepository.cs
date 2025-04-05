using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymBro_App.DAL.Concrete
{
    public class StepCompetitionRepository : IStepCompetitionRepository
    {
        private readonly GymBroDbContext _context;

        public StepCompetitionRepository(GymBroDbContext context)
        {
            _context = context;
        }

            public async Task AddAsync(StepCompetitionEntity competition)
            {
                await _context.StepCompetitions.AddAsync(competition);
                await _context.SaveChangesAsync();
            }
        public StepCompetitionEntity AddOrUpdate(StepCompetitionEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(StepCompetitionEntity entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public StepCompetitionEntity FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StepCompetitionEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<StepCompetitionEntity> GetAll(Expression<Func<StepCompetitionEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StepCompetitionEntity> GetAll(params Expression<Func<StepCompetitionEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }
    }
}