using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using sTalker.Activities;
using sTalker.Helpers;
using sTalker.Notifications;
using sTalker.Shared.Models;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class EnterRoomCodeActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.enterRoomCode);

            EditText text = FindViewById<EditText>(Resource.Id.roomCode);


            FindViewById<Button>(Resource.Id.next_btn).Click += async (sender, e) =>
            {
                GameInfo.roomCode = text.Text;
                try
                {
                    GameInfo.personGroup = Task.Run(async () =>
                        await GameInfo.faceServiceClient.GetPersonGroupAsync(text.Text)).Result;

                    StartActivity(typeof(RegistrationActivity));
                }
                catch (AggregateException)
                {
                    new ToastCreator(this, "Room not found").Run();
                }
            };
        }
    }
}

