﻿namespace GitUI.Hotkey
{
  partial class ControlHotkeys
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lHotkey = new System.Windows.Forms.Label();
      this.bApply = new System.Windows.Forms.Button();
      this.bClear = new System.Windows.Forms.Button();
      this.cmbSettings = new System.Windows.Forms.ComboBox();
      this.lHotkeyableItems = new System.Windows.Forms.Label();
      this.listMappings = new System.Windows.Forms.ListView();
      this.txtHotkey = new GitUI.Hotkey.TextboxHotkey();
      this.SuspendLayout();
      // 
      // lHotkey
      // 
      this.lHotkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lHotkey.AutoSize = true;
      this.lHotkey.Location = new System.Drawing.Point(3, 355);
      this.lHotkey.Name = "lHotkey";
      this.lHotkey.Size = new System.Drawing.Size(52, 17);
      this.lHotkey.TabIndex = 1;
      this.lHotkey.Text = "Hotkey";
      // 
      // bApply
      // 
      this.bApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.bApply.Location = new System.Drawing.Point(152, 380);
      this.bApply.Name = "bApply";
      this.bApply.Size = new System.Drawing.Size(66, 26);
      this.bApply.TabIndex = 2;
      this.bApply.Text = "Apply";
      this.bApply.UseVisualStyleBackColor = true;
      this.bApply.Click += new System.EventHandler(this.bApply_Click);
      // 
      // bClear
      // 
      this.bClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.bClear.Location = new System.Drawing.Point(224, 380);
      this.bClear.Name = "bClear";
      this.bClear.Size = new System.Drawing.Size(66, 26);
      this.bClear.TabIndex = 3;
      this.bClear.Text = "Clear";
      this.bClear.UseVisualStyleBackColor = true;
      this.bClear.Click += new System.EventHandler(this.bClear_Click);
      // 
      // cmbSettings
      // 
      this.cmbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbSettings.FormattingEnabled = true;
      this.cmbSettings.Location = new System.Drawing.Point(3, 23);
      this.cmbSettings.Name = "cmbSettings";
      this.cmbSettings.Size = new System.Drawing.Size(287, 24);
      this.cmbSettings.TabIndex = 4;
      this.cmbSettings.SelectedIndexChanged += new System.EventHandler(this.cmbSettings_SelectedIndexChanged);
      // 
      // lHotkeyableItems
      // 
      this.lHotkeyableItems.AutoSize = true;
      this.lHotkeyableItems.Location = new System.Drawing.Point(3, 3);
      this.lHotkeyableItems.Name = "lHotkeyableItems";
      this.lHotkeyableItems.Size = new System.Drawing.Size(116, 17);
      this.lHotkeyableItems.TabIndex = 5;
      this.lHotkeyableItems.Text = "Hotkeyable Items";
      // 
      // listMappings
      // 
      this.listMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listMappings.Location = new System.Drawing.Point(6, 53);
      this.listMappings.Name = "listMappings";
      this.listMappings.Size = new System.Drawing.Size(284, 293);
      this.listMappings.TabIndex = 6;
      this.listMappings.UseCompatibleStateImageBehavior = false;
      this.listMappings.SelectedIndexChanged += new System.EventHandler(this.listMappings_SelectedIndexChanged);
      // 
      // txtHotkey
      // 
      this.txtHotkey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtHotkey.KeyData = System.Windows.Forms.Keys.None;
      this.txtHotkey.Location = new System.Drawing.Point(56, 352);
      this.txtHotkey.Name = "txtHotkey";
      this.txtHotkey.Size = new System.Drawing.Size(234, 22);
      this.txtHotkey.TabIndex = 0;
      this.txtHotkey.Text = "None";
      // 
      // ControlHotkeys
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.listMappings);
      this.Controls.Add(this.lHotkeyableItems);
      this.Controls.Add(this.cmbSettings);
      this.Controls.Add(this.bClear);
      this.Controls.Add(this.bApply);
      this.Controls.Add(this.lHotkey);
      this.Controls.Add(this.txtHotkey);
      this.Name = "ControlHotkeys";
      this.Size = new System.Drawing.Size(295, 409);
      this.Load += new System.EventHandler(this.ControlHotkeys_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private TextboxHotkey txtHotkey;
    private System.Windows.Forms.Label lHotkey;
    private System.Windows.Forms.Button bApply;
    private System.Windows.Forms.Button bClear;
    private System.Windows.Forms.ComboBox cmbSettings;
    private System.Windows.Forms.Label lHotkeyableItems;
    private System.Windows.Forms.ListView listMappings;
  }
}