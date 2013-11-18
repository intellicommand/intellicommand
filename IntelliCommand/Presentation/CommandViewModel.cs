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
        private readonly List<KeyCombination> combinations = new List<KeyCombination>();

        /// <summary>
        /// Create new command view model.
        /// </summary>
        /// <param name="commandName">Command UI name.</param>
        /// <param name="scope">The default scope where this command was found the first time.</param>
        /// <param name="combination">The first combination which was found for this command.</param>
        /// <param name="packageSettings">Current Visual Studio settings.</param>
        public CommandViewModel(string commandName, string scope, KeyCombination combination, IPackageSettings packageSettings)
        {
            this.commandName = commandName;
            this.packageSettings = packageSettings;
            this.AddScope(scope);
            this.AddCombination(combination);
        }
        
        /// <summary>
        /// Formatted command name. If the user wants to see the scope of the command, it will be included in brackets.
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
        /// First command combination which can work for this command.
        /// </summary>
        public KeyCombination KeyCombination
        {
            get
            {
                if (this.combinations.Count == 1)
                {
                    return this.combinations[0];
                }

                Debug.Assert(this.combinations.Count <= 2, "combinations.Count");

                return this.combinations.OrderByDescending(x => x.Modifiers).First();
            }
        }

        /// <summary>
        /// Add scope for the command view model.
        /// </summary>
        /// <param name="scope">Scope which also accepts the current key combination.</param>
        public void AddScope(string scope)
        {
            if (!this.scopes.Contains(scope))
            {
                this.scopes.Add(scope);
            }
        }

        /// <summary>
        /// Add another combinations which can invoke this command.
        /// </summary>
        /// <param name="combination"></param>
        public void AddCombination(KeyCombination combination)
        {
            if (!this.combinations.Contains(combination))
            {
                this.combinations.Add(combination);
            }
        }

        /// <summary>
        /// Check if a combination exists in the command view model container.
        /// </summary>
        /// <param name="keyCombination">The key combination.</param>
        /// <returns>Key combination is known combination.</returns>
        public bool Contains(KeyCombination keyCombination)
        {
            return this.combinations.Contains(keyCombination);
        }
    }
}