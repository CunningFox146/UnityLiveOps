using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.Common.Views
{
    public interface IEventPopup : IDisposable
    {
        void SetTitle(string title);
        void SetCtaText(string ctaText);
        UniTask WaitForCtaClick(CancellationToken token);
    }
}