
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Microsoft.ProjectOxford.Face;
using sTalker.Activities;
using sTalker.Helpers;

namespace sTalker
{
    public class CameraPictureCallBack : Java.Lang.Object, Camera.IPictureCallback
    {
        private Context context;
        private CameraFragment cameraFragment;
        private Camera camera;
        public EventHandler FaceAdded;

        public CameraPictureCallBack(Context cont, CameraFragment cameraFragment)
        {
            context = cont;
            this.cameraFragment = cameraFragment;
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
                    cameraFragment.ShowToast("No face detected!");
                }
                else if (faces.Length > 1)
                {
                    cameraFragment.ShowToast("More than one face detected!");
                }
                else
                {
                    if (context.GetType() == typeof(SelfieCameraActivity))
                    {
                        AddNewFace(data);
                        return;
                    } else
                    {
                        bool success = Task.Run(async()=> await Recognize(data,new Guid[]{ faces[0].FaceId })).Result;
                        if (success)
                        {
                            camera.StopPreview();
                            camera.Release();
                            cameraFragment.OwnerActivity.StartActivity(typeof(GameActivity));
                            return;
                        }
                    }
                }

                //We start the camera preview back since after taking a picture it freezes
                camera.StartPreview();
            }
            catch (Exception e)
            {
                cameraFragment.ShowToast($"Unhandled error: {e.Message}");
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
                await GameInfo.faceServiceClient.TrainPersonGroupAsync(GameInfo.personGroup.PersonGroupId);
                var result = await GameInfo.faceServiceClient.IdentifyAsync(GameInfo.personGroup.PersonGroupId, faceIds,1);

                if (result?[0].Candidates?[0].PersonId == GameInfo.player.playerToFind.RecognitionServiceId)
                {
                    //TODO: Handle points and new person to find
                    cameraFragment.ShowToast($"SUCCESS");
                    return true;
                }
                else
                {
                    //TODO: Handle points substraction
                    cameraFragment.ShowToast("YOU FAILED!");
                    return false;
                }
            }
            catch (FaceAPIException e)
            {
                cameraFragment.ShowToast("Face recognition service failed. Try again.");
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