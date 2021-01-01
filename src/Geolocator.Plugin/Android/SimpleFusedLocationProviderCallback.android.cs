using System;
using Android.Gms.Location;
using Plugin.Geolocator.Abstractions;

namespace Plugin.Geolocator
{
    internal class SimpleFusedLocationProviderCallback : LocationCallback
    {
        public event EventHandler<PositionAvailabilityEventArgs> PositionAvailabilityChanged;
        public event EventHandler<PositionEventArgs> PositionChanged;

        // Called when there is a change in the availability of location data.
        public override void OnLocationAvailability(LocationAvailability locationAvailability)
        {
            PositionAvailabilityChanged?.Invoke(this, new PositionAvailabilityEventArgs(locationAvailability.IsLocationAvailable));
        }

        // Called when device location information is available.
        public override void OnLocationResult(LocationResult result)
        {
            PositionChanged?.Invoke(this, new PositionEventArgs(result.LastLocation.ToPosition()));

            // TODO need to handle result.Locations ?
        }
    }
}