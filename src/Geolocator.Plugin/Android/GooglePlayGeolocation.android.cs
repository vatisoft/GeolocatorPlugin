using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Plugin.Geolocator.Abstractions;
using LocationRequest = Plugin.Geolocator.Abstractions.FusedLocation.LocationRequest;

namespace Plugin.Geolocator
{
	[Preserve(AllMembers = true)]
	public class GooglePlayGeolocationImplementation : GeolocationBase, IGeolocator
	{
        public event EventHandler<PositionErrorEventArgs> PositionError;
        public event EventHandler<PositionEventArgs> PositionChanged;

        private readonly Lazy<FusedLocationProviderClient> client = new Lazy<FusedLocationProviderClient>(LocationServices.GetFusedLocationProviderClient(Application.Context));

        private readonly AsyncLock stateLock = new AsyncLock();
        private SimpleFusedLocationProviderCallback activeCallback;

        private FusedLocationProviderClient Client => client.Value;

        public double DesiredAccuracy { get; set; }
        public bool IsListening => activeCallback != null;
        public bool SupportsHeading => true;
        public bool IsGeolocationAvailable => Task.Run(async () => (await Client.GetLocationAvailabilityAsync())?.IsLocationAvailable ?? false).Result;
        public bool IsGeolocationEnabled => GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Application.Context) == ConnectionResult.Success;

        public async Task<Position> GetLastKnownLocationAsync()
        {
            await EnsurePermissionsAsync();

            var location = await Client.GetLastLocationAsync();
            return location.ToPosition();
        }

        public Task<Position> GetPositionAsync(TimeSpan? timeout = null, CancellationToken? token = null, bool includeHeading = false)
        {
            // TODO add if FusedLocationProviderClient.getCurrentLocation() become available in Xamarin package

            throw new NotImplementedException();
        }

        public Task<IEnumerable<Address>> GetAddressesForPositionAsync(Position position, string mapKey = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Position>> GetPositionsForAddressAsync(string address, string mapKey = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StartListeningAsync(TimeSpan minimumTime, double minimumDistance, bool includeHeading = false, ListenerSettings listenerSettings = null, LocationRequest locationRequest = null)
        {
            await EnsurePermissionsAsync();

            // TODO additional precondition checks

            var request = locationRequest.ToLocationRequest();
            var callback = new SimpleFusedLocationProviderCallback();
            var looper = Looper.MyLooper() ?? Looper.MainLooper;

            callback.PositionAvailabilityChanged += OnPositionAvailabilityChanged;
            callback.PositionChanged += OnPositionChanged;

            await Client.RequestLocationUpdatesAsync(request, callback, looper);

            await SetCallbackAsync(callback);

            return true;
        }

        public async Task<bool> StopListeningAsync()
        {
            if (IsListening)
            {
                await stateLock.LockAsync();
                try
                {
                    if (IsListening)
                    {
                        await StopLocationUpdatesInternalAsync(activeCallback);
                    }

                    activeCallback = null;
                }
                finally
                {
                    stateLock.Release();
                }
            }

            return true;
        }

        private async Task SetCallbackAsync(SimpleFusedLocationProviderCallback callback)
        {
            await stateLock.LockAsync();
            try
            {
                if (IsListening)
                {
                    await StopLocationUpdatesInternalAsync(activeCallback);
                }

                activeCallback = callback;
            }
            finally
            {
                stateLock.Release();
            }
        }

        private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            if (!IsListening)
            {
                return;
            }

            PositionChanged?.Invoke(this, e);
        }

        private void OnPositionAvailabilityChanged(object sender, PositionAvailabilityEventArgs e)
        {
            // TODO
        }

        private async Task StopLocationUpdatesInternalAsync(SimpleFusedLocationProviderCallback callbackToStop)
        {
            // TODO check if this is still true with new version

            // RemoveLocationUpdatesAsync does not complete (unknown issue), not awaiting
            //await client.RemoveLocationUpdatesAsync(callback);

            await Task.Run(() => Client.RemoveLocationUpdates(callbackToStop));
        }
    }
}
