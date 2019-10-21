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

namespace sTalker.Activities
{
	[Activity (Label = "SelfieCameraActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class SelfieCameraActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            SetContentView(Resource.Layout.selfieCameraBase);

            FragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, 
                    new CameraFragment(this,Resource.Layout.selfieCamera, Resource.Id.selfie_Button, Resource.Id.selfie_camera_preview))
               .Commit();


            /*Use with camera2fragment*/

            //SetContentView (Resource.Layout.cameraWindowBase);

            //         if (bundle == null) {
            //             FragmentManager.BeginTransaction ().Replace (Resource.Id.container, Camera2Fragment.NewInstance (this,this,false)).Commit ();
            //}
        }
	}
}


