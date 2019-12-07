
using Android.Content;
using Android.Hardware;
using Android.Media;
using Android.Widget;
using Java.IO;
using Microsoft.ProjectOxford.Face;
using sTalker.Activities;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace sTalker
{
    public class CameraPictureCallBack : Java.Lang.Object, Camera.IPictureCallback
    {
        private Context context;
        private CameraFragment cameraFragment;
        private Camera camera;
        public EventHandler FaceAdded;
        private MediaPlayer mediaPlayer;
        public EventHandler<bool> PlayerFaceFound;
        public Button snapButton;

        public CameraPictureCallBack(Context cont, CameraFragment cameraFragment, Button btn)
        {
            context = cont;
            this.cameraFragment = cameraFragment;
            snapButton = btn;
        }

        public void OnPictureTaken(byte[] data, Camera camera)
        {
            this.camera = camera;
            try
            {
                var stream = new MemoryStream(data); 
                var faces = Task.Run(async () => await GameInfo.faceServiceClient.DetectAsync(stream)).Result;

                if (faces.Length < 1)
                {
                    //TODO: if occurs, fix the issue that camera sometimes flips taken photos
                    mediaPlayer = MediaPlayer.Create(context, Resource.Raw.fail);
                    mediaPlayer.Start();
                    cameraFragment.ShowToast("No face detected!");
                    Vibration.Vibrate();
                    snapButton.Enabled = true;
                }
                else if (faces.Length > 1)
                {
                    mediaPlayer = MediaPlayer.Create(context, Resource.Raw.fail);
                    mediaPlayer.Start();
                    cameraFragment.ShowToast("More than one face detected!");
                    snapButton.Enabled = true;
                }
                else
                {
                    if (context.GetType() == typeof(SelfieCameraActivity))
                    {
                        AddNewFace(data);
                        return;
                    } else
                    {
                        snapButton.Enabled = false;
                        bool success = Task.Run(async()=> await Recognize(data,new Guid[]{ faces[0].FaceId })).Result;
                        if (success)
                        {
                            camera.StopPreview();
                            camera.Release();
                            cameraFragment.OwnerActivity.StartActivity(typeof(GameActivity));
                            return;
                        }
                        snapButton.Enabled = true;
                    }
                }

                //We start the camera preview back since after taking a picture it freezes
                camera.StartPreview();
            }
            catch (Exception e)
            {
                cameraFragment.ShowToast($"Unhandled error: {e.Message}");
                snapButton.Enabled = true;
            }
        }

        private async void AddNewFace(byte[] data)
        {
            try
            {
                var result = await GameInfo.faceServiceClient.CreatePersonAsync(GameInfo.personGroup.PersonGroupId,
                    GameInfo.player.UserId, GameInfo.player.Name);
                               
                var added = await GameInfo.faceServiceClient.AddPersonFaceAsync(GameInfo.personGroup.PersonGroupId,
                    result.PersonId, new MemoryStream(data));

                GameInfo.player.RecognitionServiceId = result.PersonId;
            }
            catch (FaceAPIException e)
            {
                cameraFragment.ShowToast("Face detection service failed.");
            }
            camera.StopPreview();
            camera.Release();
            cameraFragment.OwnerActivity.StartActivity(typeof(UserWaitingActivity));
            FaceAdded?.Invoke(this,EventArgs.Empty);
        }

        private async Task<bool> Recognize(byte[] data, Guid[] faceIds)
        {
            try
            {
                var result = Task.Run(async()=>await GameInfo.faceServiceClient.IdentifyAsync(GameInfo.personGroup.PersonGroupId, faceIds,1)).Result;

                if (result?[0].Candidates.Length != 0 && 
                    result?[0].Candidates?[0].PersonId == GameInfo.player.playerToFind.RecognitionServiceId)
                {
                    PlayerFaceFound?.Invoke(this, true);
                    mediaPlayer = MediaPlayer.Create(context, Resource.Raw.success);
                    mediaPlayer.Start();
                    cameraFragment.ShowToast("NICE WORK!");
                    return true;
                }
                else
                {
                    PlayerFaceFound?.Invoke(this, false);
                    mediaPlayer = MediaPlayer.Create(context, Resource.Raw.fail);
                    mediaPlayer.Start();
                    cameraFragment.ShowToast("YOU FAILED!");
                    Vibration.Vibrate();
                    return false;
                }
            }
            catch (FaceAPIException e)
            {
                cameraFragment.ShowToast("Face recognition service failed. Try again.");
                return false;
            }
            catch(Exception e)
            {
                cameraFragment.ShowToast(e.Message);
                return false;
            }

        }






        /// <summary>
        /// UNUSED
        /// Was used because there were problems with Face API calls when providing byte stream to the service directly
        /// In this method, image is temporary stored in device memory and put to the firebase storage
        /// Then the Face API is called with url from firebase, not stream
        /// </summary>
        /// <param name="bytes"></param>
        private async void AddNewFaceWithStorage(byte[] bytes)
        {
            using (var file = new Java.IO.File(cameraFragment.OwnerActivity.GetExternalFilesDir(null), "temp.jpg"))
            {
                using (var output = new FileOutputStream(file))
                {
                    try
                    {
                        output.Write(bytes);
                    }
                    catch (Java.IO.IOException e)
                    {
                        e.PrintStackTrace();
                    }
                }


                using (var stream = System.IO.File.Open(file.AbsolutePath, FileMode.Open))
                {
                    var imageName = $"faces/{GameInfo.personGroup.PersonGroupId}-{GameInfo.player.UserId}.jpg";

                    //var storage = DataHelper.GetStorage();
                    //await storage.Child(imageName).PutAsync(stream);
                    //var url = await storage.Child(imageName).GetDownloadUrlAsync();

                    try
                    {
                        var result = await GameInfo.faceServiceClient.CreatePersonAsync(GameInfo.personGroup.PersonGroupId, GameInfo.player.UserId, GameInfo.player.Name);
                        //var added = await GameInfo.faceServiceClient.AddPersonFaceAsync(GameInfo.personGroup.PersonGroupId, result.PersonId, url);

                    }
                    catch (FaceAPIException e)
                    {

                    }
                }
            }
        }
    }
}