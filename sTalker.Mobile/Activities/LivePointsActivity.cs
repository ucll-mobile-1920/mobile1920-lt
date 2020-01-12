
using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Database.Query;
using sTalker.Adapters;
using sTalker.Helpers;
using sTalker.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Reactive.Linq;
using System.Timers;

namespace sTalker.Activities
{
    [Activity(Label = "LivePointsActivity")]
    public class LivePointsActivity : Activity
    {
        private ListView pointsListView;
        private List<Player> registeredPlayers = new List<Player>();
        PointsAdapter adapter;
        private Button menuBtn;
        private Button endBtn;

        public Timer timer { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.livePoints);

            endBtn = FindViewById<Button>(Resource.Id.end_btn);
            menuBtn = FindViewById<Button>(Resource.Id.mainMenu_btn);
            menuBtn.Visibility = Android.Views.ViewStates.Gone;
            menuBtn.Click+=(sender,e) => StartActivity(typeof(MainActivity));

            endBtn.Click += (sender,e)=>endBtn.Enabled = false;
            endBtn.Click += async (sender, e) => {
                await SetGameEnd();
                ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(1001);
                endBtn.Enabled = false;
                endBtn.Visibility = Android.Views.ViewStates.Gone;
                menuBtn.Visibility = Android.Views.ViewStates.Visible;
                StartActivity(typeof(ResultsActivity));
                Finish();
            };
            FindViewById<TextView>(Resource.Id.livePointsTitle).Text = GameInfo.title;
            FindViewById<TextView>(Resource.Id.livePointsRoomCode).Text = GameInfo.roomCode;
            SetupRecyClerView();
            DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Players").AsObservable<Player>().Subscribe(x => UpdatePlayers(x.Object));

            timer = new Timer();
            timer.Interval = 3000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now >= GameInfo.gameEnd)
            {
                timer.Stop();
                await SetGameEnd();
                ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(1001);
                RunOnUiThread(() =>
                {
                    endBtn.Enabled = false;
                    endBtn.Visibility = Android.Views.ViewStates.Gone;
                    menuBtn.Visibility = Android.Views.ViewStates.Visible;
                    StartActivity(typeof(ResultsActivity));
                    Finish();
                });
            }
        }

        private void UpdatePlayers(Player player)
        {
            if (player != null)
            {
                var playerToUpdate = registeredPlayers.Find(x => x.UserId == player.UserId);
                playerToUpdate.points = player.points;
                var list = registeredPlayers.OrderBy(p => p.points).ToList();
                list.Reverse();
                registeredPlayers.Clear();
                registeredPlayers.AddRange(list);

                RunOnUiThread(() => adapter.NotifyDataSetChanged());
            }
        }

        private async Task SetGameEnd()
        {
            await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status/0").PutAsync(GameStatus.FINISHED);
        }

        private void SetupRecyClerView()
        {
            registeredPlayers = Task.Run(async () => await GetAllPlayers()).Result;
            pointsListView = FindViewById<ListView>(Resource.Id.livePointsList);
            registeredPlayers = registeredPlayers.OrderBy(p => p.points).ToList();
            registeredPlayers.Reverse();
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
