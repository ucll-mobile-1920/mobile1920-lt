using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android;
using sTalker.Fragments;
using System.Threading.Tasks;
using Android.Support.V4.App;
using Firebase.Database;
using sTalker.Helpers;
using Firebase.Database.Query;

namespace sTalker.Activities
{
	[Activity (Label = "SelfieCameraActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class SelfieCameraActivity : Activity
	{
        public CameraFragment Camera { get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            SetContentView(Resource.Layout.selfieCameraBase);

            Camera = new CameraFragment(this, Resource.Layout.selfieCamera, Resource.Id.selfie_Button, Resource.Id.selfie_camera_preview);

            Camera.FaceAdded += (x,y)=>SaveNewPlayer();

            FragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, Camera)
               .Commit();
        }

        private async void SaveNewPlayer()
        {
            await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Players/{GameInfo.player.UserId}").PutAsync(GameInfo.player);
        }
	}
}


