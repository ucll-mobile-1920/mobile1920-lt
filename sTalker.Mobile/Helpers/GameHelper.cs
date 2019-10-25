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
            var players = (await DataHelper.GetFirebase().Child($"Games/{roomCode}/Players").
                OnceAsync<Player>()).Where(x=>!x.Object.isAdmin)
                .Select(x => new Player { Name = x.Object.Name, RecognitionServiceId = x.Object.RecognitionServiceId, UserId = x.Object.UserId }).ToList();

            if (players.Count() < 1)
                return;

            for (int i=1; i<players.Count(); i++)
            {
                players[i].playerToFind = players[i - 1];
                await DataHelper.GetFirebase().Child($"Games/{roomCode}/Players/{players[i].UserId}/playerToFind").
                    PutAsync(players[i].playerToFind);
            }
            await DataHelper.GetFirebase().Child($"Games/{roomCode}/Players/{players[0].UserId}/playerToFind").
                    PutAsync(players.Last());
        }
    }
}