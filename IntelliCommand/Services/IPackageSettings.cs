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
        /// Delay in milliseconds before show IntelliCommand window when user hold modifiers key.
        /// </summary>
        int ModifiersCombinationsShowDelay { get; }

        /// <summary>
        /// Delay in milliseconds before show IntelliCommand window when user press first combination of chord keys.
        /// </summary>
        int ChordCombinationsShowDelay { get; }

        /// <summary>
        /// Selected sort index.
        /// </summary>
        int SelectedSortIndex { get; }

        /// <summary>
        /// Inclide command scopes in name.
        /// </summary>
        bool ShowCommandScopeName { get; }

        /// <summary>
        /// Intelli Command transparent in %.
        /// </summary>
        int WindowsOpacity { get; }

        /// <summary>
        /// Selected window theme.
        /// </summary>
        int SelectedWindowTheme { get; }
    }
}