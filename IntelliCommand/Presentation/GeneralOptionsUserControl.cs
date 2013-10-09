// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Presentation
{
    using System;
    using System.Windows.Forms;

    using IntelliCommand.Shell;

    public partial class GeneralOptionsUserControl : UserControl
    {
        private readonly IntelliCommandOptionsDialogPage intelliCommandOptionsDialogPage;

        public GeneralOptionsUserControl(IntelliCommandOptionsDialogPage intelliCommandOptionsDialogPage)
        {
            if (intelliCommandOptionsDialogPage == null)
            {
                throw new ArgumentNullException("intelliCommandOptionsDialogPage");
            }

            this.intelliCommandOptionsDialogPage = intelliCommandOptionsDialogPage;
            this.InitializeComponent();

            this.numericModifiersCombinationsShowDelay.Value = this.intelliCommandOptionsDialogPage.ModifiersCombinationsShowDelay;
            this.numericChordCombinationsShowDelay.Value = this.intelliCommandOptionsDialogPage.ChordCombinationsShowDelay;
            this.checkBoxShowScopeName.Checked = this.intelliCommandOptionsDialogPage.ShowCommandScopeName;

            if (this.comboBoxSort.Items.Count > this.intelliCommandOptionsDialogPage.SelectedSortIndex
                && this.intelliCommandOptionsDialogPage.SelectedSortIndex >= 0)
            {
                this.comboBoxSort.SelectedIndex = this.intelliCommandOptionsDialogPage.SelectedSortIndex;
            }
            else
            {
                this.comboBoxSort.SelectedIndex = 0;
            }

            if (this.comboBoxTheme.Items.Count > this.intelliCommandOptionsDialogPage.SelectedWindowTheme
                && this.intelliCommandOptionsDialogPage.SelectedWindowTheme >= 0)
            {
                this.comboBoxTheme.SelectedIndex = this.intelliCommandOptionsDialogPage.SelectedWindowTheme;
            }
            else
            {
                this.comboBoxTheme.SelectedIndex = 0;
            }

            this.numericOpacity.Value = this.intelliCommandOptionsDialogPage.WindowsOpacity;
        }

        private void NumericWindowShowDelayValueChanged(object sender, EventArgs e)
        {
            this.intelliCommandOptionsDialogPage.ModifiersCombinationsShowDelay = (int)this.numericModifiersCombinationsShowDelay.Value;
        }

        private void CheckBoxShowCommandScopeCheckedChanged(object sender, EventArgs e)
        {
            this.intelliCommandOptionsDialogPage.ShowCommandScopeName = this.checkBoxShowScopeName.Checked;
        }

        private void ComboBoxSortSelectedIndexChanged(object sender, EventArgs e)
        {
            this.intelliCommandOptionsDialogPage.SelectedSortIndex = this.comboBoxSort.SelectedIndex;
        }

        private void NumericChordCombinationsShowDelayValueChanged(object sender, EventArgs e)
        {
            this.intelliCommandOptionsDialogPage.ChordCombinationsShowDelay = (int)this.numericChordCombinationsShowDelay.Value;
        }

        private void NumericOpacityValueChanged(object sender, EventArgs e)
        {
            this.intelliCommandOptionsDialogPage.WindowsOpacity = (int)this.numericOpacity.Value;
        }

        private void ComboBoxThemeSelectedIndexChanged(object sender, EventArgs e)
        {
            this.intelliCommandOptionsDialogPage.SelectedWindowTheme = this.comboBoxTheme.SelectedIndex;
        }
    }
}
