// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    /// <summary>
    /// The output window service.
    /// </summary>
    internal interface IOutputWindowService
    {
        /// <summary>
        /// Write to output window the line.
        /// </summary>
        /// <param name="messageFormat">The message.</param>
        /// <param name="arguments">The arguments.</param>
        void OutputLine(string messageFormat, params object[] arguments);

        /// <summary>
        /// Write to output window the string.
        /// </summary>
        /// <param name="messageFormat">The message.</param>
        /// <param name="arguments">The arguments.</param>
        void OutputString(string messageFormat, params object[] arguments);
    }
}