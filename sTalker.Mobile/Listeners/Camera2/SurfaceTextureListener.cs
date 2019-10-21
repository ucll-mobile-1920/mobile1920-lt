using Android.Views;
using sTalker.Fragments;

namespace sTalker.Listeners
{
    public class SurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        private readonly Camera2Fragment owner;

        public SurfaceTextureListener(Camera2Fragment owner)
        {
            this.owner = owner ?? throw new System.ArgumentNullException("owner");
        }

        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            owner.OpenCamera(width, height);
        }

        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
        {
            return true;
        }

        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
        {

        }
    }
}