
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Database.Query;
using sTalker.Helpers;

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


