using Firebase.Database.Query;
using sTalker.Shared.Models;
using System.Linq;
using System.Threading.Tasks;

namespace sTalker.Helpers
{
    public static class GameHelper
    {

        public async static Task AssignPlayersToFind(string roomCode)
        {
            var players = (await DataHelper.GetFirebase().Child($"Games/{roomCode}/Players")
                .OnceAsync<Player>())
                .Select(x => new Player {
                    Name = x.Object.Name,
                    RecognitionServiceId = x.Object.RecognitionServiceId,
                    UserId = x.Object.UserId,
                    hints = x.Object.hints
                }).ToList();

            if (players.Count() < 1)
                return;

            for (int i=1; i<players.Count(); i++)
            {
                await DataHelper.GetFirebase().Child($"Games/{roomCode}/Players/{players[i].UserId}/playerToFind").
                    PutAsync(players[i-1]);
            }
            await DataHelper.GetFirebase().Child($"Games/{roomCode}/Players/{players[0].UserId}/playerToFind").
                    PutAsync(players.Last());
        }

        public async static Task<bool> AssignNewPlayer(string roomCode, string playerId)
        {
            Player newAsignedPlayer;

            var players = (await DataHelper.GetFirebase().Child($"Games/{roomCode}/Players")
                .OnceAsync<Player>())
                .Select(x => new Player {
                    Name = x.Object.Name,
                    RecognitionServiceId = x.Object.RecognitionServiceId,
                    UserId = x.Object.UserId,
                    hints = x.Object.hints,
                    playerToFind = x.Object.playerToFind
                }).ToList();

            var current = players.Where(p => p.UserId == playerId).First();
            var previousAsignedPlayer = current.playerToFind;

            for(int i=0; i < players.Count(); i++)
            {
                if (players[i].UserId == previousAsignedPlayer.UserId)
                {
                    if (i + 1 == players.Count())
                    {
                        newAsignedPlayer = players[0];
                    }
                    else
                    {
                        newAsignedPlayer = players[i + 1];
                    }
                    if (newAsignedPlayer.UserId == current.UserId)
                    {
                        return false;
                    }
                    else
                    {
                        await DataHelper.GetFirebase().Child($"Games/{roomCode}/Players/{playerId}/playerToFind").
                            PutAsync(newAsignedPlayer);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}