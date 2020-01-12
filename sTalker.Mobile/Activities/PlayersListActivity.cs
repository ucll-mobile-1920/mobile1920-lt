using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Widget;
using Firebase.Database.Query;
using sTalker.Adapters;
using sTalker.Helpers;
using sTalker.Notifications;
using sTalker.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class PlayersListActivity : AppCompatActivity
    {
        private ListView playersListView;
        private List<Player> registeredPlayers = new List<Player>();
        PlayersAdapter adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.playersList);

            DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Players").AsObservable<Player>().Subscribe(x => UpdatePlayers(x.Object));

            Button startBtn = FindViewById<Button>(Resource.Id.letsTalk_btn);

            startBtn.Click += async (sender, e) => {
                if (registeredPlayers.Count < 2)
                {
                    new ToastCreator(this, "There must be at least 2 players").Run();
                    return;
                }
                startBtn.Enabled = false;
                await SetGameStart();
                StartActivity(typeof(LivePointsActivity));
                ShowNotification();
                Finish();
            };

            FindViewById<TextView>(Resource.Id.playerListTitle).Text = GameInfo.title;
            FindViewById<TextView>(Resource.Id.roomCode).Text = GameInfo.roomCode;

            var players = Task.Run(async () => await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Players").OnceAsync<Player>()).Result;
            foreach(var p in players)
            {
                if(registeredPlayers.Find(x => x.UserId == p.Object.UserId) == null) registeredPlayers.Add(p.Object);
            }

            playersListView = FindViewById<ListView>(Resource.Id.playersList);
            adapter = new PlayersAdapter(this, registeredPlayers);
            playersListView.Adapter = adapter;
        }
        

        private void UpdatePlayers(Player player)
        {
            if (player != null && registeredPlayers.Find(x=>x.UserId == player.UserId)==null)
            {
                registeredPlayers.Add(player);
                RunOnUiThread(() => adapter.NotifyDataSetChanged());
            }
        }

        private async Task SetGameStart()
        {
           await GameHelper.AssignPlayersToFind(GameInfo.roomCode);

           await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status/0").PutAsync(GameStatus.STARTED);

            var startTime = DateTime.Now;
            GameInfo.gameEnd = startTime.AddMinutes(GameInfo.duration);
            await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/StartTime").PutAsync(startTime);

            await GameInfo.faceServiceClient.TrainPersonGroupAsync(GameInfo.personGroup.PersonGroupId);

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

        private void ShowNotification()
        {
            ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(1000);
            ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(1001);

            var resultIntent = new Intent(this, typeof(LivePointsActivity));
            var stackBuilder = Android.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(this);
            stackBuilder.AddNextIntent(resultIntent);

            PendingIntent resultPendingIntent =
           stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(this, "location_notification")
              .SetAutoCancel(false)
              .SetContentIntent(resultPendingIntent) 
              .SetContentTitle("Game started")
              .SetSmallIcon(Resource.Drawable.ic_launcher) 
              .SetContentText($"Interested how the game is going? Click here!");

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(1001, builder.Build());
        }
    }
}

