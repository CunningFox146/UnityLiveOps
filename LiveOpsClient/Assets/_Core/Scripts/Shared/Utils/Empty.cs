using System;

namespace App.Shared.Utils
{
    /// <summary>
    /// Represents no value. Use instead of void for generic type parameters.
    /// </summary>
    public readonly struct Empty : IEquatable<Empty>
    {
        public static readonly Empty Default = default;
        
        public bool Equals(Empty other) => true;
        public override bool Equals(object obj) => obj is Empty;
        public override int GetHashCode() => 0;
        public override string ToString() => nameof(Empty);
        
        public static bool operator ==(Empty left, Empty right) => true;
        public static bool operator !=(Empty left, Empty right) => false;
    }
}