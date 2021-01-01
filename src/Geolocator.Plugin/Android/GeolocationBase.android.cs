using System;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;

namespace Plugin.Geolocator
{
    public class GeolocationBase
    {
        private async Task<bool> CheckPermissionsAsync()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
            if (status != global::Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                Console.WriteLine("Currently does not have Location permissions, requesting permissions");

                var request = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();

                if (request != global::Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    Console.WriteLine("Location permission denied, can not get positions async.");
                    return false;
                }
            }

            return true;
        }

        protected async Task EnsurePermissionsAsync()
        {
            var hasPermission = await CheckPermissionsAsync();
            if (!hasPermission)
            {
                throw new GeolocationException(GeolocationError.Unauthorized);
            }
        }
    }
}
