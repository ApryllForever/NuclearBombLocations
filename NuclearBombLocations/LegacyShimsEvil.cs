using System;

namespace NuclearBombLocations;

//
// Summary:
//     Wraps newer .NET features that improve performance, but aren't available on .NET
//     Framework platforms.
internal static class LegacyShimsEvil
{
    //
    // Summary:
    //     Get an empty array without allocating a new array each time.
    //
    // Type parameters:
    //   T:
    //     The array value type.
    public static T[] EmptyArray<T>()
    {
        return Array.Empty<T>();
    }
}
