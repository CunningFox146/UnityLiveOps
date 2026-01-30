using UnityEngine;

namespace App.Runtime.Services.Cameras
{
    public class CameraProvider : ICameraProvider
    {
        public Camera UICamera { get; }
        public Camera WorldCamera { get; }

        public CameraProvider(CamerasRegistration registration)
        {
            UICamera = registration.UICamera;
            WorldCamera = registration.WorldCamera;
        }
    }
}