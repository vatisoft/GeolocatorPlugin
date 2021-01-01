using System.Threading;
using System.Threading.Tasks;

namespace Plugin.Geolocator.Abstractions
{
    public class AsyncLock
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public async Task LockAsync()
        {
            await semaphore.WaitAsync();
        }

        public void Release()
        {
            semaphore.Release();
        }
    }
}