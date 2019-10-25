using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database.Query;
using sTalker.Shared.Models;

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