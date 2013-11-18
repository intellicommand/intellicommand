// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    using System;

    /// <summary>
    /// Settings for current package, which we show to user in Options dialog.
    /// </summary>
    internal interface IPackageSettings
    {
        /// <summary>
        /// On settings changed event.
        /// </summary>
        event EventHandler SettingsChanged;

        /// <summary>
        /// Delay in milliseconds before showing the IntelliCommand window when the user holds a modifier key.
        /// </summary>
        int ModifiersCombinationsShowDelay { get; }

        /// <summary>
        /// Delay in milliseconds before showing the IntelliCommand window when the user presses the first part 
        /// of a shortcut chord.
        /// </summary>
        int ChordCombinationsShowDelay { get; }

        /// <summary>
        /// Selected sort index.
        /// </summary>
        int SelectedSortIndex { get; }

        /// <summary>
        /// Include command scopes in name.
        /// </summary>
        bool ShowCommandScopeName { get; }

        /// <summary>
        /// IntelliCommand transparency in %.
        /// </summary>
        int WindowsOpacity { get; }

        /// <summary>
        /// Selected window theme.
        /// </summary>
        int SelectedWindowTheme { get; }
    }
}