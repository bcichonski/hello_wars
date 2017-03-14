using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IBotClient<TArenaInfo, TMove> : ICompetitor
    {
        int? MoveTimeout { get; set; }
        Task<TMove> NextMoveAsync(TArenaInfo arenaInfo);
    }
}
