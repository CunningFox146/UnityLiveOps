using System;
using System.Threading;
using App.Runtime.Services.ViewStack;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.Common.Views
{
    public interface IEventPopup : ICloseableView, IDisposable
    {
        void SetTitle(string title);
        void SetCtaText(string ctaText);
        UniTask WaitForCtaClick(CancellationToken token);
    }
}