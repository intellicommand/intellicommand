// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Input;

    /// <summary>
    /// The command binding parser.
    /// </summary>
    public class CommandBindingParser
    {
        private const string ScopeSeparator = "::";

        private readonly Dictionary<string, Key> knownKeys = new Dictionary<string, Key>()
            {
                { "Left Arrow", Key.Left },
                { "Right Arrow", Key.Right },
                { "Down Arrow", Key.Down },
                { "Up Arrow", Key.Up },
                { "Del", Key.Delete },
                { "Ins", Key.Insert },
                { "Bkspce", Key.Back },
                { "Break", Key.Cancel }, 
                { "\\", Key.Separator },
                { "Space", Key.Space },
                { "PgDn", Key.PageDown },
                { "PgUp", Key.PageUp },
                { "Esc", Key.Escape },
                { "Num *", Key.Multiply },
                { "Num .", Key.Decimal },
                { "Num /", Key.Divide },
                { "Num +", Key.Add },
                { "Num -", Key.Subtract },
                { "Num 0", Key.NumPad0 },
                { "Num 1", Key.NumPad1 },
                { "Num 2", Key.NumPad2 },
                { "Num 3", Key.NumPad3 },
                { "Num 4", Key.NumPad4 },
                { "Num 5", Key.NumPad5 },
                { "Num 6", Key.NumPad6 },
                { "Num 7", Key.NumPad7 },
                { "Num 8", Key.NumPad8 },
                { "Num 9", Key.NumPad9 },
                { "F1", Key.F1 },
                { "F2", Key.F2 },
                { "F3", Key.F3 },
                { "F4", Key.F4 },
                { "F5", Key.F5 },
                { "F6", Key.F6 },
                { "F7", Key.F7 },
                { "F8", Key.F8 },
                { "F9", Key.F9 },
                { "F10", Key.F10 },
                { "F11", Key.F11 },
                { "F12", Key.F12 },
                { "Enter", Key.Enter },
                { "Tab", Key.Tab },
                { "Home", Key.Home },
                { "End", Key.End },
            };

        private readonly Dictionary<string, ModifierKeys> knownModifierKeys = new Dictionary<string, ModifierKeys>()
            {
                { "Ctrl", ModifierKeys.Control },
                { "Alt", ModifierKeys.Alt },
                { "Shift", ModifierKeys.Shift }
            };

        /// <summary>
        /// Parse the key combinations from <paramref name="binding"/>.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>The keys combination</returns>
        /// <remarks>
        /// See description of the bindings on the MSDN http://msdn.microsoft.com/en-us/library/envdte.command.bindings.aspx
        /// </remarks>
        public CommandBinding ParseBinding(string binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException("binding");
            }

            int indexOfScopeSeparator = binding.IndexOf(ScopeSeparator, StringComparison.InvariantCultureIgnoreCase);

            if (indexOfScopeSeparator < 0)
            {
                Debug.Fail("Current binding doesn't contain the scope separator.");
                return null;
            }

            // Split the combinations
            var keyCombinations = binding.Substring(indexOfScopeSeparator + ScopeSeparator.Length).Split(new[] { ", " }, StringSplitOptions.None);

            var commandKeyCombinations = new List<KeyCombination>();

            foreach (var keyCombination in keyCombinations)
            {
                Key commandKey;
                var commandModifierKeys = ModifierKeys.None;

                int previousPlusIndex = -1;
                do
                {
                    var currentPlusIndex = keyCombination.IndexOf('+', previousPlusIndex + 1);
                    if (currentPlusIndex < 0)
                    {
                        break;
                    }

                    var modifierKeyString = keyCombination.Substring(previousPlusIndex + 1, currentPlusIndex - (previousPlusIndex + 1));

                    ModifierKeys modifierKeys;
                    if (this.knownModifierKeys.TryGetValue(modifierKeyString, out modifierKeys))
                    {
                        commandModifierKeys |= modifierKeys;
                    }
                    else
                    {
                        break;
                    }

                    previousPlusIndex = currentPlusIndex;
                }
                while (previousPlusIndex > 0 && previousPlusIndex < keyCombination.Length);

                var keyString = keyCombination.Substring(previousPlusIndex + 1, keyCombination.Length - (previousPlusIndex + 1));

                if (!this.knownKeys.TryGetValue(keyString, out commandKey))
                {
                    Debug.Assert(keyString.Length == 1, "keyString.Length == 1");

                    commandKey = KeyInterop.KeyFromVirtualKey(NativeMethods.IntVkKeyScan(keyString.ToLower()[0]));
                }

                commandKeyCombinations.Add(new KeyCombination(commandModifierKeys, commandKey, keyCombination));
            }

            return new CommandBinding(binding.Substring(0, indexOfScopeSeparator), commandKeyCombinations.ToArray());
        }
    }
}