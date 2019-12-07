using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Widget;
using Firebase.Database.Query;
using sTalker.Helpers;
using sTalker.Notifications;
using sTalker.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class CreateGameActivity : AppCompatActivity
    {
        EditText title;
        int duration;
        SeekBar seekBar;
        TextView textView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            duration = 10;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.createGame);

            title = FindViewById<EditText>(Resource.Id.nameOfTheGame_txt);

            Button newBtn = FindViewById<Button>(Resource.Id.next_btn);
            newBtn.Click += async (sender, e) => {

                if (string.IsNullOrWhiteSpace(title.Text))
                {
                    new ToastCreator(this, "Title cannot be empty!").Run();
                    return;
                }

                newBtn.Enabled = false;

                if (await CreateGame(title.Text))
                {
                    ShowNotification();
                    StartActivity(typeof(PlayersListActivity));
                    Finish();
                }
            };

            seekBar = FindViewById<SeekBar>(Resource.Id.time_seekBar);
            textView = FindViewById<TextView>(Resource.Id.time);
            seekBar.Max = 170;
            seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    textView.Text = string.Format("{0} min", e.Progress+10);
                    duration = e.Progress + 10;
                }
            };

        }

        private async Task<bool> CreateGame(string gameTitle)
        {
            var game = new Game(gameTitle, duration);
            GameInfo.duration = duration;
            try
            {
                await GameInfo.faceServiceClient.CreatePersonGroupAsync(game.RoomCode.ToString(), game.Title);

                await DataHelper.GetFirebase().Child($"Games/{game.RoomCode}").PutAsync(game);

                return true;
            }
            catch { new ToastCreator(this, "Unable to establish connection").Run(); return false; }

        }

        private void ShowNotification()
        {
            ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(1000);
            var resultIntent = new Intent(this, typeof(PlayersListActivity));
            var stackBuilder = Android.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(this);
            stackBuilder.AddNextIntent(resultIntent);

            PendingIntent resultPendingIntent =
           stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(this, "location_notification")
              .SetAutoCancel(false) 
              .SetContentIntent(resultPendingIntent)
              .SetContentTitle("Waiting for players") 
              .SetSmallIcon(Resource.Drawable.ic_launcher)
              .SetContentText($"Want to start the game? Click here!");

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(1000, builder.Build());
        }


    }
}

