using System;

namespace Common.Input
{
    public interface IInputService
    {
        event Action BackPressed;
    }
}