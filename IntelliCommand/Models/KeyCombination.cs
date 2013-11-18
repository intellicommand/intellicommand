// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Models
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// The KeyCombination class.
    /// </summary>
    public class KeyCombination : IComparable, IComparable<KeyCombination>
    {
        private readonly string stringView;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyCombination"/> class.
        /// </summary>
        /// <param name="modifierKeys">
        /// The modifier keys.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="stringView">
        /// String representation of current key.
        /// </param>
        public KeyCombination(ModifierKeys modifierKeys, Key key, string stringView)
        {
            this.Modifiers = modifierKeys;
            this.Key = key;

            this.stringView = stringView;
        }

        /// <summary>
        /// Gets the modifier keys.
        /// </summary>
        public ModifierKeys Modifiers { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public Key Key { get; private set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.stringView;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((KeyCombination)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Modifiers.GetHashCode() * 397) ^ this.Key.GetHashCode();
            }
        }

        /// <inheritdoc />
        public int CompareTo(KeyCombination other)
        {
            if (other == null)
            {
                return 1;
            }

            return string.Compare(this.ToString(), other.ToString(), StringComparison.CurrentCulture);
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            return this.CompareTo(obj as KeyCombination);
        }

        protected bool Equals(KeyCombination other)
        {
            return this.Modifiers.Equals(other.Modifiers) && this.Key.Equals(other.Key);
        }
    }
}