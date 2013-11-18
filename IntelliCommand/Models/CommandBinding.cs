// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Models
{
    using System.Diagnostics;

    /// <summary>
    /// The CommandBinding class.
    /// </summary>
    public class CommandBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBinding"/> class.
        /// </summary>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <param name="keyCombinations">
        /// The key combinations.
        /// </param>
        public CommandBinding(string scope, KeyCombination[] keyCombinations)
        {
            Debug.Assert(keyCombinations.Length == 1 || keyCombinations.Length == 2, "keyCombinations.Length == 1 || keyCombinations.Length == 2");

            this.Scope = scope;
            this.KeyCombinations = keyCombinations;
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// Gets the key combinations.
        /// </summary>
        public KeyCombination[] KeyCombinations { get; private set; }
    }
}