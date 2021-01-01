using System;

namespace Plugin.Geolocator.Abstractions.FusedLocation
{
    /// <summary>
    /// A data object that contains quality of service parameters for requests to the <c>FusedLocationProviderApi</c>.
    /// </summary>
    /// <remarks>
    /// <see cref="LocationRequest"/> objects are used to request a quality of service for location updates from the <c>FusedLocationProviderApi</c>.
    /// <br />
    /// For example, if your application wants high accuracy location it should create a location request with <see cref="Priority"/> set to <value>HighAccuracy</value> and <see cref="Interval"/> to 5 seconds. This would be appropriate for mapping applications that are showing your location in real-time.
    /// <br />
    /// At the other extreme, if you want negligible power impact, but to still receive location updates when available, then create a location request with <see cref="Priority"/> set to <value>NoPower</value>. With this request your application will not trigger (and therefore will not receive any power blame) any location updates, but will receive locations triggered by other applications. This would be appropriate for applications that have no firm requirement for location, but can take advantage when available.
    /// <br />
    /// In between these two extremes is a very common use-case, where applications definitely want to receive updates at a specified interval, and can receive them faster when available, but still want a low power impact. These applications should consider <value>BalancedPowerAccuracy</value> combined with a faster <see cref="FastestInterval"/> (such as 1 minute) and a slower <see cref="Interval"/> (such as 60 minutes). They will only be assigned power blame for the interval set by <see cref="Interval"/>, but can still receive locations triggered by other applications at a rate up to <see cref="FastestInterval"/>. This style of request is appropriate for many location aware applications, including background usage. Do be careful to also throttle <see cref="FastestInterval"/> if you perform heavy-weight work after receiving an update - such as using the network.
    /// <br />
    /// Activities should strongly consider removing all location request when entering the background (for example at <c>onPause()</c>), or at least swap the request to a larger interval and lower quality.
    /// <br />
    /// Applications cannot specify the exact location sources, such as GPS, that are used by the <c>LocationClient</c>. In fact, the system may have multiple location sources (providers) running and may fuse the results from several sources into a single <c>Location</c> object.
    /// <br />
    /// Location requests from applications with <c>ACCESS_COARSE_LOCATION</c> and not <c>ACCESS_FINE_LOCATION</c> will be automatically throttled to a slower interval, and the location object will be obfuscated to only show a coarse level of accuracy.
    /// <br />
    /// All location requests are considered hints, and you may receive locations that are more/less accurate, and faster/slower than requested.
    /// </remarks>
    public class LocationRequest
    {
        /// <summary>
        /// Gets or sets the priority of the request.
        /// </summary>
        /// <remarks>
        /// Use with a priority such as <see cref="LocationRequestPriority"/>. No other values are accepted.
        /// <br />
        /// The priority of the request is a strong hint to the LocationClient for which location sources to use. For example, <value>HighAccuracy</value> is more likely to use GPS, and <value>BalancedPowerAccuracy</value> is more likely to use WIFI &amp; Cell tower positioning, but it also depends on many other factors (such as which sources are available) and is implementation dependent.
        /// <br />
        /// setPriority(int) and setInterval(long) are the most important parameters on a location request.
        /// </remarks>
        public LocationRequestPriority? Priority { get; set; }

        /// <summary>
        /// Gets or sets the desired interval for active location updates.
        /// </summary>
        /// <remarks>
        /// The location client will actively try to obtain location updates for your application at this interval, so it has a direct influence on the amount of power used by your application. Choose your interval wisely.
        /// <br />
        /// This interval is inexact. You may not receive updates at all (if no location sources are available), or you may receive them slower than requested. You may also receive them faster than requested (if other applications are requesting location at a faster interval). The fastest rate that that you will receive updates can be controlled with <see cref="FastestInterval"/>. By default this fastest rate is 6x the interval frequency.
        /// <br />
        /// Applications with only the coarse location permission may have their interval silently throttled.
        /// <br />
        /// An interval of 0 is allowed, but not recommended, since location updates may be extremely fast on future implementations.
        /// <br />
        /// <see cref="Priority"/> and <see cref="Interval"/> are the most important parameters on a location request.
        /// </remarks>
        public TimeSpan? Interval { get; set; }

        /// <summary>
        /// Gets or sets the fastest interval for location updates.
        /// </summary>
        /// <remarks>
        /// This controls the fastest rate at which your application will receive location updates, which might be faster than <see cref="Interval"/> in some situations (for example, if other applications are triggering location updates).
        /// <br />
        /// This allows your application to passively acquire locations at a rate faster than it actively acquires locations, saving power.
        /// <br />
        /// Unlike <see cref="Interval"/>, this parameter is exact. Your application will never receive updates faster than this value.
        /// <br />
        /// If you don't set this value, a fastest interval will be selected for you. It will be a value faster than your active interval <see cref="Interval"/>.
        /// <br />
        /// An interval of 0 is allowed, but not recommended, since location updates may be extremely fast on future implementations.
        /// <br />
        /// If <see cref="FastestInterval"/> is set slower than <see cref="Interval"/>, then your effective fastest interval is <see cref="Interval"/>.
        /// </remarks>
        public TimeSpan? FastestInterval { get; set; }

        /// <summary>
        /// Set the minimum displacement between location updates in meters
        /// </summary>
        /// <remarks>
        /// By default this is 0.
        /// </remarks>
        public float? SmallestDisplacement { get; set; }
    }
}