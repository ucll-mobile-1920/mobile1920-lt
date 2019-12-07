
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Database.Query;
using sTalker.Helpers;
using sTalker.Notifications;

namespace sTalker.Activities
{
    [Activity(Label = "ProveItActivity")]
    public class ProveItActivity : Activity
    {
        private CameraFragment cameraFragment;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.selfieCameraBase);

            cameraFragment = new CameraFragment(this, Resource.Layout.proveItCamera, Resource.Id.proveItButton, Resource.Id.proveIt_camera_preview, false);
            cameraFragment.PlayerFaceFound += UpdatePoints;

            FragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, cameraFragment)
               .Commit();
        }

        private async void UpdatePoints(object sender, bool correctPerson)
        {
            var now = DateTime.Now;
            if (correctPerson)
            {
                GameInfo.player.points += (int)GameInfo.gameEnd.Subtract(now).TotalSeconds;
                if (await GameHelper.AssignNewPlayer(GameInfo.roomCode, GameInfo.player.UserId))
                {
                    StartActivity(typeof(GameActivity));
                    Finish();
                }
                else
                {
                    new ToastCreator(this, "You have found all players!");
                    StartActivity(typeof(WaitGameEndActivity));
                    ShowNotification();
                    Finish();
                }
            }
            else
            {
                GameInfo.player.points -= (int)GameInfo.gameEnd.Subtract(now).TotalSeconds / 5;
            }
            await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Players/{GameInfo.player.UserId}/points").PutAsync(GameInfo.player.points);

        }

        private void ShowNotification()
        {
            ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(2001);

            var resultIntent = new Intent(this, typeof(WaitGameEndActivity));
            var stackBuilder = Android.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(this);
            stackBuilder.AddNextIntent(resultIntent);

            PendingIntent resultPendingIntent =
           stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(this, "location_notification")
              .SetAutoCancel(false)
              .SetContentIntent(resultPendingIntent)
              .SetContentTitle("Wait for others")
              .SetSmallIcon(Resource.Drawable.ic_launcher) 
              .SetContentText($"Wait for others to finish");

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(2003, builder.Build());
        }
    }

}
