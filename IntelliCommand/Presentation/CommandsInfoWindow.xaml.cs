// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Threading;

    using IntelliCommand.Models;
    using IntelliCommand.Services;

    using Application = System.Windows.Application;

    /// <summary>
    /// Interaction logic for CommandsInfoWindow.xaml
    /// </summary>
    public partial class CommandsInfoWindow : Window, ICommandsInfoWindow
    {
        private readonly IPackageSettings packageSettings;
        private Window parentWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsInfoWindow"/> class.
        /// </summary>
        /// <param name="servicesProvider">The services provider.</param>
        internal CommandsInfoWindow(IAppServiceProvider servicesProvider)
        {
            this.DataContext = new CommandsPresenter(servicesProvider, this);
            this.InitializeComponent();

            this.packageSettings = servicesProvider.GetService<IPackageSettings>();
            this.packageSettings.SettingsChanged += (sender, args) => this.UpdateTheme();

            this.UpdateTheme();

            this.SizeChanged += this.OnSizeChanged;

            this.IsOpen = false;
        }

        public bool IsOpen
        {
            get
            {
                return this.parentWindow != null;
            }

            private set
            {
                this.UnsubscribeFromWindow();
                this.parentWindow = null;
                
                if (value)
                {
                    this.FindParentWindow();
                }

                if (this.parentWindow != null)
                {
                    this.FindParentWindow();
                    this.SubscribeToWindow();
                    this.PlaceWindow();
                }

                this.mainControl.Visibility = this.parentWindow == null ? Visibility.Hidden : Visibility.Visible;
            }
        }

        void ICommandsInfoWindow.SetCombinations(CurrentCommandsContainer commandsContainer)
        {
            this.Dispatcher.Invoke(
                DispatcherPriority.DataBind,
                new Action(
                    () =>
                        {
                            IList<KeyCombination> chordCombinations = null;
                            IList<CommandViewModel> commands = null;

                            if (commandsContainer != null)
                            {
                                chordCombinations = commandsContainer.GetChordCombinations();
                                commands = commandsContainer.GetCommands();
                            }

                            if (chordCombinations != null && chordCombinations.Count > 0)
                            {
                                this.chordKeys.Text = string.Join(", ", chordCombinations);
                                this.chordKeysPanel.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                this.chordKeys.Text = null;
                                this.chordKeysPanel.Visibility = Visibility.Collapsed;
                            }

                            this.commandsPanel.Commands = commands;

                            this.IsOpen = (commands != null && commands.Count > 0)
                                          || (chordCombinations != null && chordCombinations.Count > 0);
                        }));
        }

        private void UpdateTheme()
        {
            this.Resources["BackgroundOpacity"] = ((double)this.packageSettings.WindowsOpacity) / 100d;

            if (this.packageSettings.SelectedWindowTheme == (int)Theme.Light)
            {
                this.Resources["BackgroundBrush"] = Brushes.White;
                this.Resources["TextBrush"] = Brushes.Black;
                this.Resources["HiglightedTextBrush"] = Brushes.CadetBlue;
            }
            else if (this.packageSettings.SelectedWindowTheme == (int)Theme.VsColors)
            {
                this.Resources["BackgroundBrush"] = this.Resources["VsBackgroundBrush"];
                this.Resources["TextBrush"] = this.Resources["VsTextBrush"];
                this.Resources["HiglightedTextBrush"] = this.Resources["VsHiglightedTextBrush"];
            }
            else
            {
                this.Resources["BackgroundBrush"] = Brushes.Black;
                this.Resources["TextBrush"] = Brushes.White;
                this.Resources["HiglightedTextBrush"] = Brushes.LightBlue;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            this.PlaceWindow();
        }

        private void SubscribeToWindow()
        {
            if (this.parentWindow != null)
            {
                this.parentWindow.LocationChanged += this.ParentWindowChange;
                this.parentWindow.Deactivated += this.ParentWindowChange;
                this.parentWindow.StateChanged += this.ParentWindowChange;
            }
        }

        private void UnsubscribeFromWindow()
        {
            if (this.parentWindow != null)
            {
                this.parentWindow.LocationChanged -= this.ParentWindowChange;
                this.parentWindow.Deactivated -= this.ParentWindowChange;
                this.parentWindow.StateChanged -= this.ParentWindowChange;
            }
        }

        private void ParentWindowChange(object sender, EventArgs eventArgs)
        {
            this.IsOpen = false;
        }

        private void PlaceWindow()
        {
            if (this.parentWindow != null)
            {
                double parentTop = this.parentWindow.Top;
                double parentLeft = this.parentWindow.Left;

                if (this.parentWindow.WindowState == WindowState.Maximized)
                {
                    var windowInterop = new WindowInteropHelper(this.parentWindow);
                    var screen = Screen.FromHandle(windowInterop.Handle);

                    parentTop = screen.WorkingArea.Top;
                    parentLeft = screen.WorkingArea.Left;
                }

                this.Top = parentTop + ((this.parentWindow.ActualHeight - this.ActualHeight) / 2);
                this.Left = parentLeft + ((this.parentWindow.ActualWidth - this.ActualWidth) / 2);
            }
        }

        private void FindParentWindow()
        {
            this.parentWindow = Application.Current.Windows
                .Cast<object>()
                .Where(w => w is Window && !Equals(w, this))
                .Cast<Window>()
                .FirstOrDefault(window => window.IsActive) ?? Application.Current.MainWindow;
        }
    }
}
