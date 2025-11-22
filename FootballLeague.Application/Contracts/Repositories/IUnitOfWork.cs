using System.Threading.Tasks;

namespace FootballLeague.Application.Contracts.Repositories
{
    public interface IUnitOfWork
    {
        ITeamRepository Teams { get; }

        IMatchRepository Matches { get; }

        Task SaveChangesAsync();
    }
}
