using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Firebase.Database;
using sTalker.Activities;
using sTalker.Helpers;
using sTalker.Shared.Models;

namespace sTalker
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class RegistrationActivity : AppCompatActivity
    {
        Player player;
        string name;
        List<string> hints;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.registration);
            hints = new List<string>();
            name = FindViewById<EditText>(Resource.Id.name).Text;
            hints.Add(FindViewById<EditText>(Resource.Id.hint1).Text);
            hints.Add(FindViewById<EditText>(Resource.Id.hint2).Text);
            hints.Add(FindViewById<EditText>(Resource.Id.hint3).Text);
            hints.Add(FindViewById<EditText>(Resource.Id.hint4).Text);
            hints.Add(FindViewById<EditText>(Resource.Id.hint5).Text);


            FindViewById<Button>(Resource.Id.next_btn).Click += (sender, e) => {
                StartActivity(typeof(RegisterPhotoActivity));
                player = new Player(name, hints);
                DatabaseReference reference = DataHelper.GetDatabase().GetReference("games");
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("name").SetValue(FindViewById<EditText>(Resource.Id.name).Text);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint1").SetValue(FindViewById<EditText>(Resource.Id.hint1).Text);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint2").SetValue(FindViewById<EditText>(Resource.Id.hint2).Text);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint3").SetValue(FindViewById<EditText>(Resource.Id.hint3).Text);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint4").SetValue(FindViewById<EditText>(Resource.Id.hint4).Text);
                reference.Child(GameInfo.roomCode).Child("players").Child(player.UserId.ToString()).Child("hint5").SetValue(FindViewById<EditText>(Resource.Id.hint5).Text);
            };
        }
    }
}

