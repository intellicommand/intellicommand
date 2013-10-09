// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Raw KeyEvent arguments.
    /// </summary>
    public class RawKeyEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawKeyEventArgs"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public RawKeyEventArgs(Key key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets WPF Key of the key.
        /// </summary>
        public Key Key { get; private set; }
    }
}