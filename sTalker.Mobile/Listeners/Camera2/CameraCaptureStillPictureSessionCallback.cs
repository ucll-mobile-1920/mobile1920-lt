using Android.Hardware.Camera2;
using Android.Util;
using sTalker.Fragments;

namespace sTalker.Listeners
{
    public class CameraCaptureStillPictureSessionCallback : CameraCaptureSession.CaptureCallback
    {
        private static readonly string TAG = "CameraCaptureStillPictureSessionCallback";

        private readonly Camera2Fragment owner;

        public CameraCaptureStillPictureSessionCallback(Camera2Fragment owner)
        {
            this.owner = owner ?? throw new System.ArgumentNullException("owner");
        }

        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            Log.Debug(TAG, owner.mFile.ToString());
            owner.UnlockFocus();
            if (owner.Detected)
            {
                owner.OnPause();
            }
        }
    }
}
