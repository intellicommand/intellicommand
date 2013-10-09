// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// The commands container.
    /// </summary>
    public class CommandsContainer
    {
        private readonly int keyCombinationLevel;

        private readonly Dictionary<ModifierKeys, Dictionary<string, Dictionary<Key, List<CommandInfo>>>> container = new Dictionary<ModifierKeys, Dictionary<string, Dictionary<Key, List<CommandInfo>>>>();

        public CommandsContainer(int keyCombinationLevel)
        {
            this.keyCombinationLevel = keyCombinationLevel;
        }

        /// <summary>
        /// The add command info.
        /// </summary>
        /// <param name="commandInfo">
        /// The command Info.
        /// </param>
        public void Add(CommandInfo commandInfo)
        {
            if (!commandInfo.CommandBinding.KeyCombinations.Any() || string.IsNullOrEmpty(commandInfo.Name))
            {
                return;
            }

            KeyCombination combination = commandInfo.CommandBinding.KeyCombinations[this.keyCombinationLevel];

            Dictionary<string, Dictionary<Key, List<CommandInfo>>> groupByScope;
            if (!this.container.TryGetValue(combination.Modifiers, out groupByScope))
            {
                this.container.Add(combination.Modifiers, groupByScope = new Dictionary<string, Dictionary<Key, List<CommandInfo>>>());
            }

            Dictionary<Key, List<CommandInfo>> groupByKey;
            if (!groupByScope.TryGetValue(commandInfo.CommandBinding.Scope, out groupByKey))
            {
                groupByScope.Add(commandInfo.CommandBinding.Scope, groupByKey = new Dictionary<Key, List<CommandInfo>>());
            }

            List<CommandInfo> commands;
            if (!groupByKey.TryGetValue(combination.Key, out commands))
            {
                groupByKey.Add(combination.Key, commands = new List<CommandInfo>());
            }

            commands.Add(commandInfo);
        }

        public IEnumerable<CommandInfo> Filter(ModifierKeys modifierKeys, string scope, Key key)
        {
            Dictionary<Key, List<CommandInfo>> groupByKey = this.GetGroupsByKey(modifierKeys, scope);
            if (groupByKey != null)
            {
                List<CommandInfo> commands;
                if (groupByKey.TryGetValue(key, out commands))
                {
                    foreach (var commandInfo in commands)
                    {
                        if (commandInfo.VsCommand.IsAvailable)
                        {
                            yield return commandInfo;
                        }
                    }
                }
            }
        }

        public IEnumerable<CommandInfo> Filter(ModifierKeys modifierKeys, string scope)
        {
            Dictionary<Key, List<CommandInfo>> groupByKey = this.GetGroupsByKey(modifierKeys, scope);
            if (groupByKey != null)
            {
                foreach (var commands in groupByKey.Values)
                {
                    foreach (var commandInfo in commands)
                    {
                        if (commandInfo.VsCommand.IsAvailable)
                        {
                            yield return commandInfo;
                        }
                    }
                }
            }
        }

        private Dictionary<Key, List<CommandInfo>> GetGroupsByKey(ModifierKeys modifierKeys, string scope)
        {
            Dictionary<string, Dictionary<Key, List<CommandInfo>>> groupByScope;
            if (this.container.TryGetValue(modifierKeys, out groupByScope))
            {
                Dictionary<Key, List<CommandInfo>> groupByKey;
                if (groupByScope.TryGetValue(scope, out groupByKey))
                {
                    return groupByKey;
                }
            }

            return null;
        }
    }
}