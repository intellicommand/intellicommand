// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Presentation
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Input;
    using System.Windows.Threading;

    using IntelliCommand.Models;
    using IntelliCommand.Services;

    using CommandBinding = IntelliCommand.Models.CommandBinding;

    /// <summary>
    /// The CommandsPresenter class.
    /// </summary>
    internal class CommandsPresenter : IDisposable
    {
        private readonly object locker = new object();

        private readonly IAppServiceProvider servicesProvider;

        private readonly ICommandsInfoWindow window;

        private readonly IKeyboardListenerService listenerService;
        private readonly Dispatcher dispatcher;
        private readonly ICommandScopeService commandScopeService;
        private readonly IPackageSettings packageSettings;
        private readonly CommandsContainer commandInfos;

        // State field: contains information if we are showing chord sequence
        private bool inChordSequence = false;

        // State field: Latest handeled modifiers combination
        private ModifierKeys modifiersCombination = ModifierKeys.None;

        // State field: Start time of latest handeled combination
        private DateTime? combinationStart = null;

        // Settings: delay for showing simple key combinations
        private int keyModifiersCombinationDelay;

        // Settings: delay for showing chord combinations
        private int chordCombinationDelay;

        private Timer showChordKeyTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsPresenter"/> class.
        /// </summary>
        /// <param name="servicesProvider">
        /// The services Provider.
        /// </param>
        /// <param name="window">
        /// The window.
        /// </param>
        public CommandsPresenter(IAppServiceProvider servicesProvider, ICommandsInfoWindow window)
        {
            this.window = window;

            this.servicesProvider = servicesProvider;
            this.dispatcher = servicesProvider.GetService<Dispatcher>();
            this.commandScopeService = servicesProvider.GetService<ICommandScopeService>();
            this.listenerService = servicesProvider.GetService<IKeyboardListenerService>();
            this.packageSettings = servicesProvider.GetService<IPackageSettings>();

            this.packageSettings.SettingsChanged += this.PackageSettingsOnSettingsChanged;
            this.PackageSettingsOnSettingsChanged(this, EventArgs.Empty);

            this.commandInfos = new CommandInfosLoader(this.servicesProvider).LoadCommands();
            
            this.listenerService.KeyDown += this.ListenerServiceOnKeyDown;
            this.listenerService.KeyUp += this.ListenerServiceOnKeyUp;
        }

        ~CommandsPresenter()
        {
            this.Dispose(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeTimer();
                this.listenerService.KeyDown -= this.ListenerServiceOnKeyDown;
                this.listenerService.KeyUp -= this.ListenerServiceOnKeyUp;
                this.packageSettings.SettingsChanged -= this.PackageSettingsOnSettingsChanged;
            }
        }

        private void DisposeTimer()
        {
            if (this.showChordKeyTimer != null)
            {
                this.showChordKeyTimer.Dispose();
                this.showChordKeyTimer = null;
            }
        }

        private void ListenerServiceOnKeyDown(object sender, RawKeyEventArgs args)
        {
            this.dispatcher.BeginInvoke(new Action(() => this.HandleKeyDown(args)));
        }

        private void ListenerServiceOnKeyUp(object sender, RawKeyEventArgs args)
        {
            this.dispatcher.BeginInvoke(new Action(() => this.HandleKeyUp(args)));
        }

        private void PackageSettingsOnSettingsChanged(object sender, EventArgs eventArgs)
        {
            this.keyModifiersCombinationDelay = this.packageSettings.ModifiersCombinationsShowDelay;
            this.chordCombinationDelay = this.packageSettings.ChordCombinationsShowDelay;
        }

        private void HandleKeyDown(RawKeyEventArgs args)
        {
            lock (this.locker)
            {
                var isModifierKeys = KeyExtensions.IsModifierKeys(args.Key);
                var modifierKeys = Keyboard.Modifiers;

                // If current key down is modifier key and if we are not in chord sequence
                // we need to find all chord combinations and possible commands for current modifer keys
                if (isModifierKeys && !this.inChordSequence)
                {
                    var fNewModifiersCombination = this.modifiersCombination != modifierKeys;

                    if (fNewModifiersCombination)
                    {
                        this.combinationStart = DateTime.Now;
                        this.modifiersCombination = modifierKeys;
                    }

                    if ((this.window.IsOpen && fNewModifiersCombination)
                        || (this.combinationStart.HasValue && (DateTime.Now - this.combinationStart.Value).TotalMilliseconds >= this.keyModifiersCombinationDelay))
                    {
                        this.combinationStart = null;
                        this.window.SetCombinations(this.GetCombinations(modifierKeys));
                    }
                }
                else if (!isModifierKeys && modifierKeys != ModifierKeys.None && !this.inChordSequence)
                {
                    this.DisposeTimer();

                    CurrentCommandsContainer chordCommands = this.GetChordCombinations(modifierKeys, args.Key);

                    this.inChordSequence = chordCommands != null && chordCommands.HasCommands();

                    if (this.inChordSequence && !this.window.IsOpen && this.chordCombinationDelay > 0)
                    {
                        this.showChordKeyTimer = new Timer(
                            (state) =>
                                {
                                    lock (this.locker)
                                    {
                                        if (this.showChordKeyTimer != null)
                                        {
                                            this.DisposeTimer();
                                            this.window.SetCombinations(chordCommands);
                                        }
                                    }
                                },
                            null,
                            this.chordCombinationDelay,
                            Timeout.Infinite);
                    }
                    else
                    {
                        this.window.SetCombinations(chordCommands);
                    }
                }
                else if (!isModifierKeys && (modifierKeys == ModifierKeys.None || this.inChordSequence))
                {
                    this.inChordSequence = false;
                    this.DisposeTimer();

                    // If this is not a modifier keys and user doesn't press any of modifier keys now
                    // or if this is second combination of chord key we need to hide window if it is open. 
                    if (this.window.IsOpen)
                    {
                        this.window.SetCombinations(null);
                    }
                }
            }
        }

        private void HandleKeyUp(RawKeyEventArgs args)
        {
            if (KeyExtensions.IsModifierKeys(args.Key))
            {
                lock (this.locker)
                {
                    if (!this.inChordSequence)
                    {
                        if (this.modifiersCombination != ModifierKeys.None)
                        {
                            this.modifiersCombination -= KeyExtensions.ToModifierKeys(args.Key);
                        }

                        if (this.modifiersCombination == ModifierKeys.None)
                        {
                            this.combinationStart = null;
                        }

                        this.window.SetCombinations(this.GetCombinations(this.modifiersCombination));
                    }
                }
            }
        }

        private CurrentCommandsContainer GetCombinations(ModifierKeys modifierKeys)
        {
            // Get combinations for pressed modifier keys
            if (modifierKeys == ModifierKeys.None)
            {
                return null;
            }

            var currentCommandsSet = new CurrentCommandsContainer(this.packageSettings);

            foreach (var scope in this.commandScopeService.GetCurrentScopes())
            {
                foreach (var commandInfo in this.commandInfos.Filter(modifierKeys, scope))
                {
                    CommandBinding commandBinding = commandInfo.CommandBinding;

                    if (commandBinding.KeyCombinations.Length == 1)
                    {
                        currentCommandsSet.Add(commandInfo.Name, commandBinding.Scope, commandBinding.KeyCombinations[0]);
                    }
                    else
                    {
                        currentCommandsSet.AddChordCandidate(commandBinding.KeyCombinations[0]);
                    }
                }
            }

            return currentCommandsSet;
        }

        private CurrentCommandsContainer GetChordCombinations(ModifierKeys modifierKeys, Key key)
        {
            // Filter command infos by specific modifier keys and key
            // And find in them only chord combinations
            Debug.Assert(modifierKeys != ModifierKeys.None, "modifierKeys != ModifierKeys.None");

            var commandsSet = new CurrentCommandsContainer(this.packageSettings);

            foreach (var scope in this.commandScopeService.GetCurrentScopes())
            {
                foreach (var commandInfo in this.commandInfos.Filter(modifierKeys, scope, key))
                {
                    CommandBinding commandBinding = commandInfo.CommandBinding;

                    if (commandBinding.KeyCombinations.Length == 1)
                    {
                        return null;
                    }

                    commandsSet.Add(commandInfo.Name, commandBinding.Scope, commandBinding.KeyCombinations[1]);
                }
            }

            return commandsSet;
        }
    }
}
