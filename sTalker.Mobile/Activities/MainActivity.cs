using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Java.Util;
using Newtonsoft.Json;
using sTalker.Helpers;
using sTalker.Listeners;
using sTalker.Shared.Models;

namespace sTalker
{
    

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static Player player;
        GameListener gameListener;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.main);

            FindViewById<Button>(Resource.Id.newGame_btn).Click += (sender, e) => {
                StartActivity(typeof(CreateGameActivity));
            };

            FindViewById<Button>(Resource.Id.joinRoom_btn).Click += (sender, e) => {
                StartActivity(typeof(RegistrationActivity));
            };
        }

        public void RetriveData()
        {
            gameListener = new GameListener();
            gameListener.Create();
            gameListener.DataRetrieved += GameListener_GamesRetrieved;
        }

        private void GameListener_GamesRetrieved(object sender, GameListener.GameDataEventArgs e)
        {
            //ga
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

