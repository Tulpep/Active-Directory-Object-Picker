namespace ADPickerTester
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnInvoke = new System.Windows.Forms.Button();
            this.chklistDefaultTypes = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkMultiSelect = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chklistAllowedTypes = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chklistAllowedLocations = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chklistDefaultLocations = new System.Windows.Forms.CheckedListBox();
            this.chkShowAdvanced = new System.Windows.Forms.CheckBox();
            this.txtTargetComputer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chklistAttributes = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkSkipDcCheck = new System.Windows.Forms.CheckBox();
            this.comboPathProvider = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFeedback = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnInvoke
            // 
            this.btnInvoke.Location = new System.Drawing.Point(530, 172);
            this.btnInvoke.Name = "btnInvoke";
            this.btnInvoke.Size = new System.Drawing.Size(121, 39);
            this.btnInvoke.TabIndex = 1;
            this.btnInvoke.Text = "Directory Object Picker";
            this.btnInvoke.Click += new System.EventHandler(this.btnInvoke_Click);
            // 
            // chklistDefaultTypes
            // 
            this.chklistDefaultTypes.FormattingEnabled = true;
            this.chklistDefaultTypes.Location = new System.Drawing.Point(279, 25);
            this.chklistDefaultTypes.Name = "chklistDefaultTypes";
            this.chklistDefaultTypes.Size = new System.Drawing.Size(120, 124);
            this.chklistDefaultTypes.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Default Types";
            // 
            // chkMultiSelect
            // 
            this.chkMultiSelect.AutoSize = true;
            this.chkMultiSelect.Location = new System.Drawing.Point(279, 199);
            this.chkMultiSelect.Name = "chkMultiSelect";
            this.chkMultiSelect.Size = new System.Drawing.Size(81, 17);
            this.chkMultiSelect.TabIndex = 4;
            this.chkMultiSelect.Text = "Multi-Select";
            this.chkMultiSelect.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Allowed Types";
            // 
            // chklistAllowedTypes
            // 
            this.chklistAllowedTypes.FormattingEnabled = true;
            this.chklistAllowedTypes.Location = new System.Drawing.Point(12, 25);
            this.chklistAllowedTypes.Name = "chklistAllowedTypes";
            this.chklistAllowedTypes.Size = new System.Drawing.Size(120, 124);
            this.chklistAllowedTypes.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(138, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Allowed Locations";
            // 
            // chklistAllowedLocations
            // 
            this.chklistAllowedLocations.FormattingEnabled = true;
            this.chklistAllowedLocations.Location = new System.Drawing.Point(138, 25);
            this.chklistAllowedLocations.Name = "chklistAllowedLocations";
            this.chklistAllowedLocations.Size = new System.Drawing.Size(120, 124);
            this.chklistAllowedLocations.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(405, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Default Locations";
            // 
            // chklistDefaultLocations
            // 
            this.chklistDefaultLocations.FormattingEnabled = true;
            this.chklistDefaultLocations.Location = new System.Drawing.Point(405, 25);
            this.chklistDefaultLocations.Name = "chklistDefaultLocations";
            this.chklistDefaultLocations.Size = new System.Drawing.Size(120, 124);
            this.chklistDefaultLocations.TabIndex = 9;
            // 
            // chkShowAdvanced
            // 
            this.chkShowAdvanced.AutoSize = true;
            this.chkShowAdvanced.Location = new System.Drawing.Point(279, 181);
            this.chkShowAdvanced.Name = "chkShowAdvanced";
            this.chkShowAdvanced.Size = new System.Drawing.Size(105, 17);
            this.chkShowAdvanced.TabIndex = 11;
            this.chkShowAdvanced.Text = "Show Advanced";
            this.chkShowAdvanced.UseVisualStyleBackColor = true;
            // 
            // txtTargetComputer
            // 
            this.txtTargetComputer.Location = new System.Drawing.Point(15, 182);
            this.txtTargetComputer.Name = "txtTargetComputer";
            this.txtTargetComputer.Size = new System.Drawing.Size(243, 20);
            this.txtTargetComputer.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Target Computer";
            // 
            // chklistAttributes
            // 
            this.chklistAttributes.FormattingEnabled = true;
            this.chklistAttributes.Location = new System.Drawing.Point(531, 25);
            this.chklistAttributes.Name = "chklistAttributes";
            this.chklistAttributes.Size = new System.Drawing.Size(120, 124);
            this.chklistAttributes.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(531, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Attributes to Fetch";
            // 
            // chkSkipDcCheck
            // 
            this.chkSkipDcCheck.AutoSize = true;
            this.chkSkipDcCheck.Checked = true;
            this.chkSkipDcCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSkipDcCheck.Location = new System.Drawing.Point(279, 163);
            this.chkSkipDcCheck.Name = "chkSkipDcCheck";
            this.chkSkipDcCheck.Size = new System.Drawing.Size(98, 17);
            this.chkSkipDcCheck.TabIndex = 4;
            this.chkSkipDcCheck.Text = "Skip DC check";
            this.chkSkipDcCheck.UseVisualStyleBackColor = true;
            // 
            // comboPathProvider
            // 
            this.comboPathProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPathProvider.FormattingEnabled = true;
            this.comboPathProvider.Location = new System.Drawing.Point(403, 182);
            this.comboPathProvider.Name = "comboPathProvider";
            this.comboPathProvider.Size = new System.Drawing.Size(121, 21);
            this.comboPathProvider.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(405, 163);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Path Provider";
            // 
            // txtFeedback
            // 
            this.txtFeedback.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtFeedback.Location = new System.Drawing.Point(0, 222);
            this.txtFeedback.Multiline = true;
            this.txtFeedback.Name = "txtFeedback";
            this.txtFeedback.ReadOnly = true;
            this.txtFeedback.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFeedback.Size = new System.Drawing.Size(666, 237);
            this.txtFeedback.TabIndex = 15;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(666, 459);
            this.Controls.Add(this.txtFeedback);
            this.Controls.Add(this.comboPathProvider);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTargetComputer);
            this.Controls.Add(this.chkShowAdvanced);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chklistAttributes);
            this.Controls.Add(this.chklistDefaultLocations);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chklistAllowedLocations);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chklistAllowedTypes);
            this.Controls.Add(this.chkSkipDcCheck);
            this.Controls.Add(this.chkMultiSelect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chklistDefaultTypes);
            this.Controls.Add(this.btnInvoke);
            this.Name = "MainForm";
            this.Text = "Directory Object Picker Tester";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button btnInvoke;
        private System.Windows.Forms.CheckedListBox chklistDefaultTypes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkMultiSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox chklistAllowedTypes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox chklistAllowedLocations;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox chklistDefaultLocations;
        private System.Windows.Forms.CheckBox chkShowAdvanced;
        private System.Windows.Forms.TextBox txtTargetComputer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox chklistAttributes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkSkipDcCheck;
        private System.Windows.Forms.ComboBox comboPathProvider;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFeedback;
    }
}