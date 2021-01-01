namespace Plugin.Geolocator.Abstractions.FusedLocation
{
    /// <summary>
    /// The priority of the <see cref="LocationRequest"/>.
    /// </summary>
    public enum LocationRequestPriority
    {
        /// <summary>
        /// Request the most accurate locations available
        /// </summary>
        HighAccuracy = 100,

        /// <summary>
        /// Request "block" level accuracy
        /// </summary>
        BalancedPowerAccuracy = 102,

        /// <summary>
        /// Request "city" level accuracy
        /// </summary>
        LowPower = 104,

        /// <summary>
        /// Request the best accuracy possible with zero additional power consumption
        /// </summary>
        NoPower = 105
    }
}