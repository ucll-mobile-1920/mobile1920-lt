using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using sTalker.Activities;

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


            FindViewById<Button>(Resource.Id.next_btn).Click += (sender, e) => {
                GameInfo.roomCode = text.Text;      
                StartActivity(typeof(RegistrationActivity));
            };
        }
    }
}

