namespace App.Runtime.Services.Cameras
{
    public interface ICameraProvider
    {
        UnityEngine.Camera UICamera { get; }
        UnityEngine.Camera WorldCamera { get; }
        void SetCameras(CamerasRegistration registration);
    }
}