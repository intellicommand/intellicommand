// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Presentation
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    using IntelliCommand.Models;
    using IntelliCommand.Services;

    /// <summary>
    /// View Model container for the showing command with combinations.
    /// </summary>
    internal class CommandViewModel
    {
        private readonly string commandName;

        private readonly IPackageSettings packageSettings;

        private readonly List<string> scopes = new List<string>();
        private readonly List<KeyCombination> combinationes = new List<KeyCombination>();

        /// <summary>
        /// Create new command view model.
        /// </summary>
        /// <param name="commandName">Command UI name.s</param>
        /// <param name="scope">The default scope where this command was find first time.</param>
        /// <param name="combination">The combination which was found for this command at first.</param>
        /// <param name="packageSettings">Current Visual Studio settings.</param>
        public CommandViewModel(string commandName, string scope, KeyCombination combination, IPackageSettings packageSettings)
        {
            this.commandName = commandName;
            this.packageSettings = packageSettings;
            this.AddScope(scope);
            this.AddCombination(combination);
        }
        
        /// <summary>
        /// Formatted command name. If user want to see scope of current command - it will be included in brackets.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.packageSettings.ShowCommandScopeName)
                {
                    return string.Format(
                        CultureInfo.CurrentUICulture, "{0} ({1})", this.commandName, string.Join(", ", this.scopes));
                }

                return this.commandName;
            }
        }

        /// <summary>
        /// First command combination which can work for current command.
        /// </summary>
        public KeyCombination KeyCombination
        {
            get
            {
                if (this.combinationes.Count == 1)
                {
                    return this.combinationes[0];
                }

                Debug.Assert(this.combinationes.Count <= 2, "combinationes.Count");

                return this.combinationes.OrderByDescending(x => x.Modifiers).First();
            }
        }

        /// <summary>
        /// Add scope for current command view model.
        /// </summary>
        /// <param name="scope">Scope which also accept current key combination.</param>
        public void AddScope(string scope)
        {
            if (!this.scopes.Contains(scope))
            {
                this.scopes.Add(scope);
            }
        }

        /// <summary>
        /// Add one more combination which can invoke current command.
        /// </summary>
        /// <param name="combination"></param>
        public void AddCombination(KeyCombination combination)
        {
            if (!this.combinationes.Contains(combination))
            {
                this.combinationes.Add(combination);
            }
        }

        /// <summary>
        /// Check if current combination exists in current command view model container.
        /// </summary>
        /// <param name="keyCombination">The key combination.</param>
        /// <returns>Key combination is known combination.</returns>
        public bool Contains(KeyCombination keyCombination)
        {
            return this.combinationes.Contains(keyCombination);
        }
    }
}