using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Java.IO;
using System;

namespace sTalker
{
    public class CameraPreview : SurfaceView, ISurfaceHolderCallback
    {
        private Android.Hardware.Camera camera;
        private static bool stopped;

        public CameraPreview(Context context, Android.Hardware.Camera camera) : base(context)
        {
            this.camera = camera;
            this.camera.SetDisplayOrientation(90);

            //Surface holder callback is set so theat SurfaceChanged, Created, destroy... 
            //Could be called from here.
            Holder.AddCallback(this);
            // deprecated but required on Android versions less than 3.0
            Holder.SetType(SurfaceType.PushBuffers);
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            if (Holder.Surface == null)
            {
                return;
            }
            
            try
            {
                camera.StopPreview();
            }
            catch
            {
                // ignore: tried to stop a non-existent preview
            }

            try
            {
                // start preview with new settings
                camera.SetPreviewDisplay(Holder);
                camera.StartPreview();
            }
            catch (Exception e)
            {
                Log.Debug("", "Error starting camera preview: " + e.Message);
            }
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                camera.SetPreviewDisplay(holder);
                camera.StartPreview();
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
        }
    }
}