using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Widget;
using sTalker.Helpers;
using sTalker.Notifications;
using sTalker.Shared.Models;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace sTalker.Activities
{
    [Activity(Label = "UserWaitingActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class UserWaitingActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.userWaiting);

            //if (!CheckIfCanJoin())
            //{
            //    new ToastCreator(this, "Game has already started! You're too late").Run();
            //    return;
            //}

            DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status").AsObservable<GameStatus>().Subscribe(x => StartGame(x.Object));
        }

        public void StartGame(GameStatus gameStatus)
        {
            if(gameStatus == GameStatus.STARTED)
            {
                StartActivity(typeof(GameActivity));
                ShowNotification();
                Finish();
            }
        }

        //private bool CheckIfCanJoin()
        //{
        //    var gameStatus = Task.Run(async () => await DataHelper.GetFirebase()
        //        .Child($"Games/{GameInfo.roomCode}/Status").OnceAsync<GameStatus>()).Result;
        //    return false;
        //}

        private void ShowNotification()
        {
            ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(2000);
            ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(2001);

            var resultIntent = new Intent(this, typeof(GameActivity));
            var stackBuilder = Android.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(this);
            stackBuilder.AddNextIntent(resultIntent);

            PendingIntent resultPendingIntent =
           stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(this, "location_notification")
              .SetAutoCancel(false)
              .SetContentIntent(resultPendingIntent)
              .SetContentTitle("Go sTalk!")
              .SetSmallIcon(Resource.Drawable.ic_launcher) 
              .SetContentText($"Found your person? Click here!");

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(2001, builder.Build());
        }


    }
}