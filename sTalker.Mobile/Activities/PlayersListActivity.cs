using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;
using sTalker.Activities;
using sTalker.Adapters;
using sTalker.Helpers;
using sTalker.Listeners;
using sTalker.Shared.Models;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class PlayersListActivity : AppCompatActivity
    {
        private ListView playersListView;
        private List<Player> registeredPlayers;
        PlayersAdapter adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.playersList);

            DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Players").AsObservable<Player>().Subscribe(x => UpdatePlayers(x.Object));

            FindViewById<Button>(Resource.Id.letsTalk_btn).Click += async (sender, e) => {
                await SetGameStart();
                StartActivity(typeof(LivePointsActivity));
            };

            FindViewById<TextView>(Resource.Id.playerListTitle).Text = GameInfo.title;
            FindViewById<TextView>(Resource.Id.roomCode).Text = GameInfo.roomCode;
        }
        

        private void UpdatePlayers(Player player)
        {
            //TODO: update listview with new player (passed as parameter to this method)
        }

        private async Task SetGameStart()
        {
           await GameHelper.AssignPlayersToFind(GameInfo.roomCode);

           await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status/0").PutAsync(GameStatus.STARTED);
        }

        private void PlayersListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = registeredPlayers[e.Position].Name;
            Toast.MakeText(this, select, ToastLength.Long).Show();
        }


        private void SetupRecyClerView()
        {
            playersListView = FindViewById<ListView>(Resource.Id.playersList);
            adapter = new PlayersAdapter(this, registeredPlayers);
            playersListView.Adapter = adapter;
            playersListView.ItemClick += PlayersListView_ItemClick;
        }
    }
}

