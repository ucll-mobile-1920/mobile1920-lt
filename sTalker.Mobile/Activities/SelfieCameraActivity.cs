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

namespace sTalker.Activities
{
	[Activity (Label = "SelfieCameraActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class SelfieCameraActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.cameraWindowBase);

            //FindViewById<Button>(Resource.Id.takeSelfie_btn).Click += (sender, e) =>
            //{
            //    StartActivity(typeof(GameActivity));
            //};

            if (bundle == null) {
				FragmentManager.BeginTransaction ().Replace (Resource.Id.container, Camera2Fragment.NewInstance (this,this,true)).Commit ();
			}
		}
	}
}


