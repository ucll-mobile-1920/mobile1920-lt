using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using sTalker.Helpers;
using sTalker.Notifications;
using sTalker.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class EnterRoomCodeActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.enterRoomCode);

            EditText text = FindViewById<EditText>(Resource.Id.roomCode);

            Button nextBtn = FindViewById<Button>(Resource.Id.next_btn);
            nextBtn.Click += (sender, e) =>
            {
                nextBtn.Enabled = false;
                GameInfo.roomCode = text.Text;
                try
                {
                    GameInfo.personGroup = Task.Run(async () =>
                        await GameInfo.faceServiceClient.GetPersonGroupAsync(text.Text)).Result;

                    var status = Task.Run(async () => 
                        await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status/0").OnceSingleAsync<GameStatus>()).Result;
                    if(status != GameStatus.NEW)
                    {
                        new ToastCreator(this, "Game has already started. You cannot join.").Run();
                        nextBtn.Enabled = true;
                        return;
                    }

                    GameInfo.title = Task.Run(async () =>
                        await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Title").OnceSingleAsync<string>()).Result;
                    StartActivity(typeof(RegistrationActivity));
                    Finish();
                }
                catch (AggregateException)
                {
                    new ToastCreator(this, "Room not found").Run();
                    nextBtn.Enabled = true;
                }
            };
        }
    }
}

