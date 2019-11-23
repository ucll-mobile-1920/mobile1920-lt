using Android;
using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using sTalker.Notifications;
using System;

namespace sTalker
{
    public class CameraFragment : Android.App.Fragment
    {
        private Camera camera;
        private CameraPreview cameraPreview;
        private FrameLayout frameLayout;
        private bool cameraReleased;
        private int layout;
        private int button;
        private int preview;
        private bool facedFront;
        public EventHandler FaceAdded;
        public EventHandler<bool> PlayerFaceFound;

        public Activity OwnerActivity { get; }


        public CameraFragment(Activity activity, int layout, int button, int preview, bool facedFront = true)
        {
            OwnerActivity = activity;
            this.layout = layout;
            this.button = button;
            this.preview = preview;
            this.facedFront = facedFront;
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignor = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(layout, container, false);

            var snapButton = view.FindViewById<Button>(button);
            snapButton.BringToFront();
            snapButton.Click += SnapButtonClick; ;

            camera = SetUpCamera();
            frameLayout = view.FindViewById<FrameLayout>(preview);
            SetCameraPreview();

            return view;
        }

        private void SnapButtonClick(object sender, EventArgs e)
        {
            try
            {
                camera.StartPreview();
                var callback = new CameraPictureCallBack(Activity, this);
                callback.FaceAdded += (x,y)=>FaceAdded?.Invoke(x, y);
                callback.PlayerFaceFound += (x, y) => PlayerFaceFound.Invoke(x, y);
                camera.TakePicture(null, null, callback);
            }
            catch
            {
                throw;
            }
        }

        public override void OnDestroy()
        {
            try
            {
                camera.StopPreview();
                camera.Release();
                cameraReleased = true;
            }
            catch
            { //TODO
            }
            base.OnDestroy();
        }

        public override void OnResume()
        {
            if (cameraReleased)
            {
                camera.Reconnect();
                camera.StartPreview();
                cameraReleased = false;
            }
            base.OnResume();
        }

        /// <summary>
        /// Set the Camera Preview, and pass it the current device's Camera
        /// </summary>
        private void SetCameraPreview()
        {
            frameLayout.AddView(new CameraPreview(Activity, camera));
        }

        /// <summary>
        /// Get an instace of the current hardware camera of the device
        /// </summary>
        private Camera SetUpCamera()
        {
            int MY_PERMISSIONS_REQUEST_Camera = 101;
            if (ContextCompat.CheckSelfPermission(OwnerActivity,
                            Manifest.Permission.Camera)
                    != Permission.Granted)
            {

                // Should we show an explanation?
                if (ActivityCompat.ShouldShowRequestPermissionRationale(OwnerActivity,
                        Manifest.Permission.Camera))
                {

                    // Show an expanation to the user *asynchronously* -- don't block
                    // this thread waiting for the user's response! After the user
                    // sees the explanation, try again to request the permission.

                }
                else
                {
                    ActivityCompat.RequestPermissions(OwnerActivity,
                            new String[] { Manifest.Permission.Camera },
                            MY_PERMISSIONS_REQUEST_Camera);
                }
            }
            Camera camera = null;
            try
            {
                if (Camera.NumberOfCameras > 1 && facedFront)
                {
                    camera = Camera.Open(1);
                }
                else
                {
                    camera = Camera.Open(0);
                }
            }
            catch
            {
                ShowToast("Device camera not available now.");
            }

            return camera;
        }

        public void ShowToast(string text)
        {
            if (Activity != null)
            {
                Activity.RunOnUiThread(new ToastCreator(Activity.ApplicationContext, text));
            }
        }
    }
}