using System.ComponentModel;

namespace MinecraftModUpdater.UI
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.curseModListBox = new System.Windows.Forms.ListBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.searchModNameTextBox = new System.Windows.Forms.TextBox();
            this.installedModlistBox = new System.Windows.Forms.ListBox();
            this.installModButton = new System.Windows.Forms.Button();
            this.uninstallModButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // curseModListBox
            // 
            this.curseModListBox.DisplayMember = "Name";
            this.curseModListBox.FormattingEnabled = true;
            this.curseModListBox.Location = new System.Drawing.Point(12, 134);
            this.curseModListBox.Name = "curseModListBox";
            this.curseModListBox.Size = new System.Drawing.Size(279, 251);
            this.curseModListBox.TabIndex = 0;
            this.curseModListBox.ValueMember = "Id";
            this.curseModListBox.SelectedIndexChanged += new System.EventHandler(this.selectedCurseMod);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(12, 108);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(67, 20);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "button1";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // searchModNameTextBox
            // 
            this.searchModNameTextBox.Location = new System.Drawing.Point(85, 108);
            this.searchModNameTextBox.Name = "searchModNameTextBox";
            this.searchModNameTextBox.Size = new System.Drawing.Size(206, 20);
            this.searchModNameTextBox.TabIndex = 2;
            this.searchModNameTextBox.TextChanged += new System.EventHandler(this.searchModNameTextBox_TextChanged);
            // 
            // installedModlistBox
            // 
            this.installedModlistBox.FormattingEnabled = true;
            this.installedModlistBox.Location = new System.Drawing.Point(522, 134);
            this.installedModlistBox.Name = "installedModlistBox";
            this.installedModlistBox.Size = new System.Drawing.Size(266, 251);
            this.installedModlistBox.TabIndex = 3;
            // 
            // installModButton
            // 
            this.installModButton.Enabled = false;
            this.installModButton.Location = new System.Drawing.Point(365, 193);
            this.installModButton.Name = "installModButton";
            this.installModButton.Size = new System.Drawing.Size(87, 34);
            this.installModButton.TabIndex = 4;
            this.installModButton.Text = "button2";
            this.installModButton.UseVisualStyleBackColor = true;
            // 
            // uninstallModButton
            // 
            this.uninstallModButton.Enabled = false;
            this.uninstallModButton.Location = new System.Drawing.Point(365, 289);
            this.uninstallModButton.Name = "uninstallModButton";
            this.uninstallModButton.Size = new System.Drawing.Size(87, 33);
            this.uninstallModButton.TabIndex = 5;
            this.uninstallModButton.Text = "button3";
            this.uninstallModButton.UseVisualStyleBackColor = true;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uninstallModButton);
            this.Controls.Add(this.installModButton);
            this.Controls.Add(this.installedModlistBox);
            this.Controls.Add(this.searchModNameTextBox);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.curseModListBox);
            this.Name = "MainView";
            this.Text = "MainView";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button installModButton;
        private System.Windows.Forms.Button uninstallModButton;
        private System.Windows.Forms.ListBox installedModlistBox;
        private System.Windows.Forms.TextBox searchModNameTextBox;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ListBox curseModListBox;

        #endregion
    }
}