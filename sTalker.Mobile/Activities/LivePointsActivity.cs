
using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Database.Query;
using sTalker.Helpers;
using sTalker.Shared.Models;
using System.Threading.Tasks;

namespace sTalker.Activities
{
    [Activity(Label = "LivePointsActivity")]
    public class LivePointsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.livePoints);

            Button endBtn = FindViewById<Button>(Resource.Id.end_btn);
            endBtn.Click += (sender,e)=>endBtn.Enabled = false;
            endBtn.Click += async (sender, e) => {
                await SetGameEnd();
                ((NotificationManager)ApplicationContext.GetSystemService(NotificationService)).Cancel(1001);
                StartActivity(typeof(AdminResultsActivity));
                Finish();
            };

            FindViewById<TextView>(Resource.Id.livePointsTitle).Text = GameInfo.title;
            FindViewById<TextView>(Resource.Id.livePointsRoomCode).Text = GameInfo.roomCode;
        }

        private async Task SetGameEnd()
        {
            await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status/0").PutAsync(GameStatus.FINISHED);
        }
    }
}
