using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using sTalker.Activities;
using sTalker.Helpers;
using sTalker.Notifications;
using sTalker.Shared.Models;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class RegistrationActivity : AppCompatActivity
    {
        Player player;
        string name;
        string[] hints;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.registration);
            hints = new string[5];


            FindViewById<Button>(Resource.Id.next_btn).Click += (sender, e) => {

                name = FindViewById<EditText>(Resource.Id.name).Text;
                hints[0]=FindViewById<EditText>(Resource.Id.hint1).Text;
                hints[1]=FindViewById<EditText>(Resource.Id.hint2).Text;
                hints[2]=FindViewById<EditText>(Resource.Id.hint3).Text;
                hints[3]=FindViewById<EditText>(Resource.Id.hint4).Text;
                hints[4]=FindViewById<EditText>(Resource.Id.hint5).Text;

                //TODO: remove when needed to get actual input
                //Test();

                if (string.IsNullOrEmpty(name) || hints.Where(x=>string.IsNullOrEmpty(x)).Count() != 0)
                {
                    new ToastCreator(this, "Please fill in all fields!").Run();
                    return;
                }
                                
                StartActivity(typeof(SelfieCameraActivity));
                player = new Player(name, hints.ToList());
                GameInfo.player = player;

                DatabaseReference reference = DataHelper.GetDatabase().GetReference("games"); 
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("name").SetValue(name);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint1").SetValue(hints[0]);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint2").SetValue(hints[1]);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint3").SetValue(hints[2]);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint4").SetValue(hints[3]);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint5").SetValue(hints[4]);
            };
        }

        public void Test()
        {
            name = "test";
            hints[0] = "hint1";
            hints[1] = "gint2";
            hints[2] = "gdg";
            hints[3] = "fsdfsf";
            hints[4] = "fdfdf";
        }
    }
}

