using System;

namespace App.Runtime.Features.Common
{
    public enum FeatureType
    {
        Unknown,
        ClickerLiveOp,
        KeyCollectLiveOp,
        PlayGamesLiveOp,
    }

    public static class FeatureTypeExtensions
    {
        private static readonly string[] TypeToString = Enum.GetNames(typeof(FeatureType));

        public static FeatureType ToFeatureType(this string type)
        {
            for (var i = 0; i < TypeToString.Length; i++)
            {
                if (string.Equals(TypeToString[i], type, StringComparison.Ordinal))
                    return (FeatureType)i;
            }
            return FeatureType.Unknown;
        }
    }
}