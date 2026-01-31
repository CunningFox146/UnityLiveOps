using App.Runtime.Services.Cameras;

namespace App.Runtime.Services.ViewsFactory
{
    public interface ICameraAware
    {
        void SetCamera(ICameraProvider provider);
    }
}