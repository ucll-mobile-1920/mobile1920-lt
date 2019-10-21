﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace sTalker.Activities
{
    [Activity(Label = "ProveItActivity")]
    public class ProveItActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.selfieCameraBase);

            FragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, 
                    new CameraFragment(this,Resource.Layout.proveItCamera, Resource.Id.proveItButton, Resource.Id.proveIt_camera_preview, false))
               .Commit();


        }
    }

}
