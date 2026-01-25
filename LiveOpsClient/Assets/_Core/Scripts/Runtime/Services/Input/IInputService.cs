using System;

namespace App.Runtime.Services.Input
{
    public interface IInputService
    {
        event Action BackPressed;
    }
}