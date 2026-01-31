using System;
using UnityEngine;

namespace App.Runtime.Services.Cameras
{
    [Serializable]
    public class CamerasRegistration
    {
        [field: SerializeField] public Camera UICamera { get; private set; }
        [field: SerializeField] public Camera WorldCamera { get; private set; } 
    }
}