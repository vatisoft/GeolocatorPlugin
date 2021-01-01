using System;

namespace Plugin.Geolocator.Abstractions
{
    /// <summary>
    /// Availability args
    /// </summary>
    public class PositionAvailabilityEventArgs : EventArgs
    {
        public PositionAvailabilityEventArgs(bool isLocationAvailable)
        {
            IsLocationAvailable = isLocationAvailable;
        }

        public bool IsLocationAvailable { get; }
    }
}