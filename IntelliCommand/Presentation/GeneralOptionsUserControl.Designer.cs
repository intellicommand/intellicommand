namespace IntelliCommand.Presentation
{
    partial class GeneralOptionsUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblWindowsShowDelay = new System.Windows.Forms.Label();
            this.numericModifiersCombinationsShowDelay = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericChordCombinationsShowDelay = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxTheme = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericOpacity = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxShowScopeName = new System.Windows.Forms.CheckBox();
            this.comboBoxSort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericModifiersCombinationsShowDelay)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericChordCombinationsShowDelay)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWindowsShowDelay
            // 
            this.lblWindowsShowDelay.AutoSize = true;
            this.lblWindowsShowDelay.Location = new System.Drawing.Point(6, 21);
            this.lblWindowsShowDelay.Name = "lblWindowsShowDelay";
            this.lblWindowsShowDelay.Size = new System.Drawing.Size(143, 13);
            this.lblWindowsShowDelay.TabIndex = 1;
            this.lblWindowsShowDelay.Text = "Key combinations delay (ms):";
            // 
            // numericModifiersCombinationsShowDelay
            // 
            this.numericModifiersCombinationsShowDelay.Increment = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericModifiersCombinationsShowDelay.Location = new System.Drawing.Point(155, 19);
            this.numericModifiersCombinationsShowDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericModifiersCombinationsShowDelay.Name = "numericModifiersCombinationsShowDelay";
            this.numericModifiersCombinationsShowDelay.Size = new System.Drawing.Size(96, 20);
            this.numericModifiersCombinationsShowDelay.TabIndex = 2;
            this.numericModifiersCombinationsShowDelay.ValueChanged += new System.EventHandler(this.NumericWindowShowDelayValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.numericChordCombinationsShowDelay);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericModifiersCombinationsShowDelay);
            this.groupBox1.Controls.Add(this.lblWindowsShowDelay);
            this.groupBox1.Location = new System.Drawing.Point(7, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(619, 77);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Intelli Command Window Show Delays";
            // 
            // numericChordCombinationsShowDelay
            // 
            this.numericChordCombinationsShowDelay.Increment = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericChordCombinationsShowDelay.Location = new System.Drawing.Point(185, 47);
            this.numericChordCombinationsShowDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericChordCombinationsShowDelay.Name = "numericChordCombinationsShowDelay";
            this.numericChordCombinationsShowDelay.Size = new System.Drawing.Size(96, 20);
            this.numericChordCombinationsShowDelay.TabIndex = 4;
            this.numericChordCombinationsShowDelay.ValueChanged += new System.EventHandler(this.NumericChordCombinationsShowDelayValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Chord key combinations delay (ms):";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBoxTheme);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericOpacity);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.checkBoxShowScopeName);
            this.groupBox2.Controls.Add(this.comboBoxSort);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(7, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(619, 132);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Window View Customization";
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTheme.FormattingEnabled = true;
            this.comboBoxTheme.Items.AddRange(new object[] {
            "Dark",
            "Light",
            "VS Colors"});
            this.comboBoxTheme.Location = new System.Drawing.Point(97, 46);
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTheme.TabIndex = 12;
            this.comboBoxTheme.SelectedIndexChanged += new System.EventHandler(this.ComboBoxThemeSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Window Theme:";
            // 
            // numericOpacity
            // 
            this.numericOpacity.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericOpacity.Location = new System.Drawing.Point(117, 20);
            this.numericOpacity.Name = "numericOpacity";
            this.numericOpacity.Size = new System.Drawing.Size(71, 20);
            this.numericOpacity.TabIndex = 10;
            this.numericOpacity.ValueChanged += new System.EventHandler(this.NumericOpacityValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Window Opacity (%):";
            // 
            // checkBoxShowScopeName
            // 
            this.checkBoxShowScopeName.AutoSize = true;
            this.checkBoxShowScopeName.Location = new System.Drawing.Point(9, 100);
            this.checkBoxShowScopeName.Name = "checkBoxShowScopeName";
            this.checkBoxShowScopeName.Size = new System.Drawing.Size(159, 17);
            this.checkBoxShowScopeName.TabIndex = 7;
            this.checkBoxShowScopeName.Text = "Show scopes for commands";
            this.checkBoxShowScopeName.UseVisualStyleBackColor = true;
            this.checkBoxShowScopeName.CheckedChanged += new System.EventHandler(this.CheckBoxShowCommandScopeCheckedChanged);
            // 
            // comboBoxSort
            // 
            this.comboBoxSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSort.FormattingEnabled = true;
            this.comboBoxSort.Items.AddRange(new object[] {
            "Command Name",
            "Key Combination"});
            this.comboBoxSort.Location = new System.Drawing.Point(109, 73);
            this.comboBoxSort.Name = "comboBoxSort";
            this.comboBoxSort.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSort.TabIndex = 6;
            this.comboBoxSort.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSortSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Sort commands by:";
            // 
            // GeneralOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GeneralOptionsUserControl";
            this.Size = new System.Drawing.Size(629, 402);
            ((System.ComponentModel.ISupportInitialize)(this.numericModifiersCombinationsShowDelay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericChordCombinationsShowDelay)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericOpacity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblWindowsShowDelay;
        private System.Windows.Forms.NumericUpDown numericModifiersCombinationsShowDelay;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericChordCombinationsShowDelay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxShowScopeName;
        private System.Windows.Forms.ComboBox comboBoxSort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericOpacity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxTheme;
        private System.Windows.Forms.Label label1;

    }
}
