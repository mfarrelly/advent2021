using System;

namespace Day1
{
    internal enum RadarState
    {
        Unknown, // = "N/A - no previous measurement";
        Increasing, // = "increasing",
        Decreasing // = "decreasing";
    }
    
    internal static class RadarStateExtensions
    {
        public static string ToReadableString(this RadarState state) =>
            state switch
            {
                RadarState.Increasing => "increasing",
                RadarState.Decreasing => "decreasing",
                RadarState.Unknown => "N/A - no previous measurement",
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
    }
}