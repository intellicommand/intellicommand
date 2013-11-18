// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Models
{
    using System.Windows.Input;

    /// <summary>
    /// Extension methods for <see cref="Key"/> and <see cref="ModifierKeys"/>.
    /// </summary>
    public static class KeyExtensions
    {
        /// <summary>
        /// Check whether <paramref name="key"/> contains key which is <see cref="ModifierKeys"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>If this is a modifier key.</returns>
        public static bool IsModifierKeys(Key key)
        {
            return ToModifierKeys(key) != ModifierKeys.None;
        }

        public static ModifierKeys ToModifierKeys(Key key)
        {
            switch (key)
            {
                case Key.LeftAlt:
                case Key.RightAlt:
                    return ModifierKeys.Alt;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return ModifierKeys.Control;
                case Key.LeftShift:
                case Key.RightShift:
                    return ModifierKeys.Shift;
                case Key.LWin:
                case Key.RWin:
                    return ModifierKeys.Windows;
            }

            return ModifierKeys.None;
        }
    }
}