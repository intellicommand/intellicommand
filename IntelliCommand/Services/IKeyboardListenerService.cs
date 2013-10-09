// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    using System;

    /// <summary>
    /// The keyboard listener service.
    /// </summary>
    internal interface IKeyboardListenerService : IDisposable
    {
        /// <summary>
        /// Fired when any of the keys is pressed down.
        /// </summary>
        event EventHandler<RawKeyEventArgs> KeyDown;

        /// <summary>
        /// Fired when any of the keys is released.
        /// </summary>
        event EventHandler<RawKeyEventArgs> KeyUp;
    }
}