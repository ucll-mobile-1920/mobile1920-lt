using Android.App;
using Android.Hardware;
using Android.OS;
using Android.Views;
using Android.Widget;
using sTalker.Notifications;
using System;

namespace sTalker
{
    public class CameraFragment : Fragment
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
                camera.TakePicture(null, null, callback);
            }
            catch
            {
                throw;
            }
        }

        public override void OnDestroy()
        {
            camera.StopPreview();
            camera.Release();
            cameraReleased = true;
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