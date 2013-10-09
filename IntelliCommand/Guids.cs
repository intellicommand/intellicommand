// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand
{
    using System;

    /// <summary>
    /// The guids.
    /// </summary>
    internal static class GuidList
    {
        /// <summary>
        /// Gets the package guid string.
        /// </summary>
        public const string GuidIntelliCommandPkgString = "398b3606-0cbd-450f-88b6-f40cd85e697e";

        /// <summary>
        /// Gets the package guid.
        /// </summary>
        public static readonly Guid GuidIntelliCommandPkg = new Guid(GuidIntelliCommandPkgString);
    }
}