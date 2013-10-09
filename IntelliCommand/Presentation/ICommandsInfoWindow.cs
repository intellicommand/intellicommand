// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Presentation
{
    /// <summary>
    /// Currrent commands popup window.
    /// </summary>
    internal interface ICommandsInfoWindow
    {
        /// <summary>
        /// Check if current window is already opened.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Set commands for window.
        /// </summary>
        /// <param name="commandsContainer">Container of commands.</param>
        void SetCombinations(CurrentCommandsContainer commandsContainer);
    }
}