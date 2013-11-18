// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IntelliCommand.Models;
    using IntelliCommand.Services;

    /// <summary>
    /// Container of commands which we need to show the user.
    /// </summary>
    internal class CurrentCommandsContainer
    {
        private readonly IPackageSettings packageSettings;
        private readonly Dictionary<string, CommandViewModel> commandViewModels = new Dictionary<string, CommandViewModel>();
        private readonly List<KeyCombination> chordCombinationCandidates = new List<KeyCombination>();

        public CurrentCommandsContainer(IPackageSettings packageSettings)
        {
            this.packageSettings = packageSettings;
        }

        public void Add(string commandName, string commandScope, KeyCombination keyCombination)
        {
            if (commandName == null)
            {
                throw new ArgumentNullException("commandName");
            }

            if (commandScope == null)
            {
                throw new ArgumentNullException("commandScope");
            }

            if (keyCombination == null)
            {
                throw new ArgumentNullException("keyCombination");
            }

            CommandViewModel viewModel;
            if (this.commandViewModels.TryGetValue(commandName, out viewModel))
            {
                viewModel.AddScope(commandScope);
                viewModel.AddCombination(keyCombination);
            }
            else
            {
                var commandViewModel = new CommandViewModel(
                    commandName, commandScope, keyCombination, this.packageSettings);
                this.commandViewModels.Add(commandName, commandViewModel);
            }
        }

        public bool HasCommands()
        {
            return this.commandViewModels.Any();
        }

        public void AddChordCandidate(KeyCombination combination)
        {
            if (combination == null)
            {
                throw new ArgumentNullException("combination");
            }

            if (!this.chordCombinationCandidates.Contains(combination))
            {
                this.chordCombinationCandidates.Add(combination);
            }
        }

        public IList<CommandViewModel> GetCommands()
        {
            var commands = this.commandViewModels.Values;

            if ((int)SortOrder.Key == this.packageSettings.SelectedSortIndex)
            {
                return commands.OrderBy(x => x.KeyCombination).ThenBy(x => x.Name).ToList();
            }
            else
            {
                return commands.OrderBy(x => x.Name).ThenBy(x => x.KeyCombination).ToList();
            }
        }

        public IList<KeyCombination> GetChordCombinations()
        {
            // Some non-chord keys can override chord combinations
            return this.chordCombinationCandidates
                .Where(chordCombination => !this.commandViewModels.Values.Any(x => x.Contains(chordCombination)))
                .OrderBy(x => x).ToList();
        }
    }
}