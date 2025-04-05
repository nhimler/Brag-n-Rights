using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IStepCompetitionRepository : IRepository<StepCompetitionEntity>
{

    Task<StepCompetitionEntity> CreateCompetitionAsync(string creatorId);
}