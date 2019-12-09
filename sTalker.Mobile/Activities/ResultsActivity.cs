using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Firebase.Database.Query;
using sTalker.Adapters;
using sTalker.Helpers;
using sTalker.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class ResultsActivity : AppCompatActivity
    {
        private ListView pointsListView;
        private List<Player> registeredPlayers = new List<Player>();
        PointsAdapter adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.results);

            FindViewById<Button>(Resource.Id.mainMenu_btn).Click += (sender, e) =>
            {
                StartActivity(typeof(MainActivity));
            };

            FindViewById<TextView>(Resource.Id.title).Text = GameInfo.title;
            SetupRecyClerView();
        }

        private void SetupRecyClerView()
        {
            registeredPlayers = Task.Run(async () => await GetAllPlayers()).Result;
            registeredPlayers = registeredPlayers.OrderBy(p => p.points).Reverse().ToList();
            pointsListView = FindViewById<ListView>(Resource.Id.list);
            adapter = new PointsAdapter(this, registeredPlayers);
            pointsListView.Adapter = adapter;
            pointsListView.ItemClick += PointsListView_ItemClick;
        }


        public async Task<List<Player>> GetAllPlayers()
        {

            return (await DataHelper.GetFirebase()
              .Child($"Games/{GameInfo.roomCode}/Players")
              .OnceAsync<Player>()).Select(item => new Player
              {
                  Name = item.Object.Name,
                  UserId = item.Object.UserId,
                  points = item.Object.points
              }).ToList();
        }

        private void PointsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = registeredPlayers[e.Position].Name;
            Toast.MakeText(this, select, ToastLength.Long).Show();
        }
    }
}

