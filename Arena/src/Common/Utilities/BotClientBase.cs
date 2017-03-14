﻿using System.Threading.Tasks;
using Common.Helpers;
using Common.Interfaces;
using Common.Models;

namespace Common.Utilities
{
    public class BotClientBase<TArenaInfo, TMove> : Competitor, IBotClient<TArenaInfo, TMove>
    {
        public int? MoveTimeout { get; set; }

        public BotClientBase()
        {}

        public BotClientBase(ICompetitor competitor) : base(competitor)
        {}

        public virtual async Task<TMove> NextMoveAsync(TArenaInfo arenaInfo)
        {
            return await WebClientHelper.PostDataAsync<TArenaInfo, TMove>(Url + Resources.PerformNextMoveUrlSuffix, arenaInfo, MoveTimeout);
        }
    }
}
