using System.Collections.Concurrent;
using System.Threading;
using Game.Domain.Core.Simulation;

namespace Game.NativeAot.Export.Internal;

internal static class SessionRegistry
{
    private static readonly ConcurrentDictionary<ulong, FishingSession> Sessions = new();
    private static long nextHandle;

    public static ulong Add(FishingSession session)
    {
        while (true)
        {
            var handle = unchecked((ulong)Interlocked.Increment(ref nextHandle));
            if (handle == 0)
            {
                continue;
            }

            if (Sessions.TryAdd(handle, session))
            {
                return handle;
            }
        }
    }

    public static bool TryGet(ulong handle, out FishingSession? session)
    {
        return Sessions.TryGetValue(handle, out session);
    }

    public static bool Remove(ulong handle)
    {
        return Sessions.TryRemove(handle, out _);
    }
}
