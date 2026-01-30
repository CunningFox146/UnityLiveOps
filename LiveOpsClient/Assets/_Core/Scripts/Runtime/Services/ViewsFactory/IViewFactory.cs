using UnityEngine;

namespace App.Runtime.Services.ViewsFactory
{
    public interface IViewFactory
    {
        TView CreateView<TView>(TView prefab)
            where TView : Object;
    }
}