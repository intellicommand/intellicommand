// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Shell
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using IntelliCommand.Presentation;
    using IntelliCommand.Services;

    using Microsoft.VisualStudio.Shell;

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class IntelliCommandOptionsDialogPage : DialogPage, IPackageSettings
    {
        public IntelliCommandOptionsDialogPage()
        {
            this.SettingsRegistryPath = "Intelli Command";
            this.ModifiersCombinationsShowDelay = 2000;
            this.ChordCombinationsShowDelay = 1500;
            this.SelectedSortIndex = 0;
            this.ShowCommandScopeName = true;
            this.WindowsOpacity = 90;
            this.SelectedWindowTheme = 0;
        }

        public event EventHandler SettingsChanged;

        public int ModifiersCombinationsShowDelay { get; set; }

        public int ChordCombinationsShowDelay { get; set; }

        public int SelectedSortIndex { get; set; }

        public bool ShowCommandScopeName { get; set; }

        public int WindowsOpacity { get; set; }

        public int SelectedWindowTheme { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get
            {
                var appServiceProvider = IntelliCommandPackage.GetAppServiceProvider();
                Debug.Assert(appServiceProvider != null, "appServiceProvider != null");
                if (appServiceProvider != null)
                {
                    var page = new GeneralOptionsUserControl(this);
                    return page;
                }

                return base.Window;
            }
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();
            this.RaiseSettingsChanged();
        }

        private void RaiseSettingsChanged()
        {
            EventHandler handler = this.SettingsChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}