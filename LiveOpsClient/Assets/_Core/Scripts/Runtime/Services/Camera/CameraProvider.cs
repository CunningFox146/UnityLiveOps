namespace App.Runtime.Services.Camera
{
    public class CameraProvider : ICameraProvider
    {
        public UnityEngine.Camera Camera { get; }

        public CameraProvider(UnityEngine.Camera camera)
        {
            Camera = camera;
        }
    }
}