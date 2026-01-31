using UnityEngine;

namespace App.Runtime.Services.Cameras
{
    public class CameraProvider : ICameraProvider
    {
        public Camera UICamera { get; private set; }
        public Camera WorldCamera { get; private set; }

        public void SetCameras(CamerasRegistration registration)
        {
            UICamera = registration.UICamera;
            WorldCamera = registration.WorldCamera;
        }
    }
}