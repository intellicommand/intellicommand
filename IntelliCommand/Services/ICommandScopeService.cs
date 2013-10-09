// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// The CommandScopeService interface.
    /// </summary>
    public interface ICommandScopeService
    {
        /// <summary>
        /// The get current scope.
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        IEnumerable<string> GetCurrentScopes();
    }
}