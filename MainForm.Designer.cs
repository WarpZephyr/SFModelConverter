﻿namespace SFModelConverter
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
            this.MainFormMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMS = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCheckModel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExportFBX = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExportAcsiiFbx = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExportColladaDae = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExportObj = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDump = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDumpMtd = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDumpLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.MainFormStatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuExportSMD4 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuGetModelMtdPaths = new System.Windows.Forms.ToolStripMenuItem();
            this.MtdsLabel = new System.Windows.Forms.Label();
            this.MtdPathsTextBox = new System.Windows.Forms.TextBox();
            this.MenuExportFLVER0 = new System.Windows.Forms.ToolStripMenuItem();
            this.MainFormMenuStrip.SuspendLayout();
            this.MainFormStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainFormMenuStrip
            // 
            this.MainFormMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.MainFormMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMS});
            this.MainFormMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainFormMenuStrip.Name = "MainFormMenuStrip";
            this.MainFormMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MainFormMenuStrip.Size = new System.Drawing.Size(759, 24);
            this.MainFormMenuStrip.TabIndex = 1;
            this.MainFormMenuStrip.Text = "MainFormMenuStrip";
            // 
            // FileMS
            // 
            this.FileMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.FileMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.FileMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileMS.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuGetModelMtdPaths,
            this.MenuCheckModel,
            this.toolStripSeparator1,
            this.MenuExport,
            this.MenuDump});
            this.FileMS.ForeColor = System.Drawing.SystemColors.Control;
            this.FileMS.Name = "FileMS";
            this.FileMS.Size = new System.Drawing.Size(37, 20);
            this.FileMS.Text = "File";
            // 
            // MenuCheckModel
            // 
            this.MenuCheckModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuCheckModel.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuCheckModel.Name = "MenuCheckModel";
            this.MenuCheckModel.Size = new System.Drawing.Size(189, 22);
            this.MenuCheckModel.Text = "Check Model Type";
            this.MenuCheckModel.Click += new System.EventHandler(this.MenuCheckModel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // MenuExport
            // 
            this.MenuExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuExportFBX,
            this.MenuExportAcsiiFbx,
            this.MenuExportColladaDae,
            this.MenuExportObj,
            this.MenuExportSMD4,
            this.MenuExportFLVER0});
            this.MenuExport.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuExport.Name = "MenuExport";
            this.MenuExport.Size = new System.Drawing.Size(189, 22);
            this.MenuExport.Text = "Export";
            this.MenuExport.ToolTipText = "Export model files";
            // 
            // MenuExportFBX
            // 
            this.MenuExportFBX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuExportFBX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuExportFBX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuExportFBX.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuExportFBX.Name = "MenuExportFBX";
            this.MenuExportFBX.Size = new System.Drawing.Size(180, 22);
            this.MenuExportFBX.Text = "FBX";
            this.MenuExportFBX.ToolTipText = "Export to FBX";
            this.MenuExportFBX.Click += new System.EventHandler(this.MenuExportFbx_Click);
            // 
            // MenuExportAcsiiFbx
            // 
            this.MenuExportAcsiiFbx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuExportAcsiiFbx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuExportAcsiiFbx.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuExportAcsiiFbx.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuExportAcsiiFbx.Name = "MenuExportAcsiiFbx";
            this.MenuExportAcsiiFbx.Size = new System.Drawing.Size(180, 22);
            this.MenuExportAcsiiFbx.Text = "Ascii FBX";
            this.MenuExportAcsiiFbx.ToolTipText = "Export to Ascii FBX";
            this.MenuExportAcsiiFbx.Click += new System.EventHandler(this.MenuExportAcsiiFbx_Click);
            // 
            // MenuExportColladaDae
            // 
            this.MenuExportColladaDae.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuExportColladaDae.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuExportColladaDae.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuExportColladaDae.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuExportColladaDae.Name = "MenuExportColladaDae";
            this.MenuExportColladaDae.Size = new System.Drawing.Size(180, 22);
            this.MenuExportColladaDae.Text = "Collada DAE";
            this.MenuExportColladaDae.ToolTipText = "Export to Collada DAE";
            this.MenuExportColladaDae.Click += new System.EventHandler(this.MenuExportColladaDae_Click);
            // 
            // MenuExportObj
            // 
            this.MenuExportObj.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuExportObj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuExportObj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuExportObj.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuExportObj.Name = "MenuExportObj";
            this.MenuExportObj.Size = new System.Drawing.Size(180, 22);
            this.MenuExportObj.Text = "OBJ";
            this.MenuExportObj.ToolTipText = "Export to OBJ";
            this.MenuExportObj.Click += new System.EventHandler(this.MenuExportObj_Click);
            // 
            // MenuDump
            // 
            this.MenuDump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuDump.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuDump.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuDump.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuDumpMtd,
            this.MenuDumpLayout});
            this.MenuDump.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuDump.Name = "MenuDump";
            this.MenuDump.Size = new System.Drawing.Size(189, 22);
            this.MenuDump.Text = "Dump";
            this.MenuDump.ToolTipText = "Dump model MTDs and Buffer Layouts";
            // 
            // MenuDumpMtd
            // 
            this.MenuDumpMtd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuDumpMtd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuDumpMtd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuDumpMtd.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuDumpMtd.Name = "MenuDumpMtd";
            this.MenuDumpMtd.Size = new System.Drawing.Size(148, 22);
            this.MenuDumpMtd.Text = "Dump MTDs";
            this.MenuDumpMtd.ToolTipText = "Dump MTDs to mtds.json";
            this.MenuDumpMtd.Click += new System.EventHandler(this.MenuDumpMtd_Click);
            // 
            // MenuDumpLayout
            // 
            this.MenuDumpLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuDumpLayout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuDumpLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuDumpLayout.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuDumpLayout.Name = "MenuDumpLayout";
            this.MenuDumpLayout.Size = new System.Drawing.Size(148, 22);
            this.MenuDumpLayout.Text = "Dump layouts";
            this.MenuDumpLayout.ToolTipText = "Dump Buffer Layouts from MTD files into layouts JSON in resources";
            this.MenuDumpLayout.Click += new System.EventHandler(this.MenuDumpLayout_Click);
            // 
            // MainFormStatusStrip
            // 
            this.MainFormStatusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.MainFormStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.MainFormStatusStrip.Location = new System.Drawing.Point(0, 307);
            this.MainFormStatusStrip.Name = "MainFormStatusStrip";
            this.MainFormStatusStrip.Size = new System.Drawing.Size(759, 22);
            this.MainFormStatusStrip.TabIndex = 2;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MenuExportSMD4
            // 
            this.MenuExportSMD4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuExportSMD4.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuExportSMD4.Name = "MenuExportSMD4";
            this.MenuExportSMD4.Size = new System.Drawing.Size(180, 22);
            this.MenuExportSMD4.Text = "SMD4";
            this.MenuExportSMD4.ToolTipText = "Export to FromSoftware SMD4";
            this.MenuExportSMD4.Click += new System.EventHandler(this.MenuExportSMD4_Click);
            // 
            // MenuGetModelMtdPaths
            // 
            this.MenuGetModelMtdPaths.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuGetModelMtdPaths.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuGetModelMtdPaths.Name = "MenuGetModelMtdPaths";
            this.MenuGetModelMtdPaths.Size = new System.Drawing.Size(189, 22);
            this.MenuGetModelMtdPaths.Text = "Get Model MTD Paths";
            this.MenuGetModelMtdPaths.ToolTipText = "Gets MTD paths from FLVERs";
            this.MenuGetModelMtdPaths.Click += new System.EventHandler(this.MenuGetModelMtdPaths_Click);
            // 
            // MtdsLabel
            // 
            this.MtdsLabel.AutoSize = true;
            this.MtdsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MtdsLabel.Location = new System.Drawing.Point(0, 24);
            this.MtdsLabel.Name = "MtdsLabel";
            this.MtdsLabel.Size = new System.Drawing.Size(64, 13);
            this.MtdsLabel.TabIndex = 4;
            this.MtdsLabel.Text = "MTD Paths:";
            // 
            // MtdPathsTextBox
            // 
            this.MtdPathsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MtdPathsTextBox.Location = new System.Drawing.Point(0, 37);
            this.MtdPathsTextBox.Multiline = true;
            this.MtdPathsTextBox.Name = "MtdPathsTextBox";
            this.MtdPathsTextBox.Size = new System.Drawing.Size(759, 270);
            this.MtdPathsTextBox.TabIndex = 5;
            // 
            // MenuExportFLVER0
            // 
            this.MenuExportFLVER0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuExportFLVER0.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuExportFLVER0.Name = "MenuExportFLVER0";
            this.MenuExportFLVER0.Size = new System.Drawing.Size(180, 22);
            this.MenuExportFLVER0.Text = "FLVER0";
            this.MenuExportFLVER0.ToolTipText = "Export to FromSoftware FLVER0";
            this.MenuExportFLVER0.Click += new System.EventHandler(this.MenuExportFLVER0_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(759, 329);
            this.Controls.Add(this.MtdPathsTextBox);
            this.Controls.Add(this.MtdsLabel);
            this.Controls.Add(this.MainFormStatusStrip);
            this.Controls.Add(this.MainFormMenuStrip);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.KeyPreview = true;
            this.MainMenuStrip = this.MainFormMenuStrip;
            this.Name = "MainForm";
            this.Text = "SoulsFormats Model Converter";
            this.MainFormMenuStrip.ResumeLayout(false);
            this.MainFormMenuStrip.PerformLayout();
            this.MainFormStatusStrip.ResumeLayout(false);
            this.MainFormStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip MainFormMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMS;
        private System.Windows.Forms.ToolStripMenuItem MenuDump;
        private System.Windows.Forms.StatusStrip MainFormStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuDumpMtd;
        private System.Windows.Forms.ToolStripMenuItem MenuDumpLayout;
        private System.Windows.Forms.ToolStripMenuItem MenuExport;
        private System.Windows.Forms.ToolStripMenuItem MenuCheckModel;
        private System.Windows.Forms.ToolStripMenuItem MenuExportColladaDae;
        private System.Windows.Forms.ToolStripMenuItem MenuExportFBX;
        private System.Windows.Forms.ToolStripMenuItem MenuExportObj;
        private System.Windows.Forms.ToolStripMenuItem MenuExportAcsiiFbx;
        private System.Windows.Forms.ToolStripMenuItem MenuExportSMD4;
        private System.Windows.Forms.ToolStripMenuItem MenuGetModelMtdPaths;
        private System.Windows.Forms.Label MtdsLabel;
        private System.Windows.Forms.TextBox MtdPathsTextBox;
        private System.Windows.Forms.ToolStripMenuItem MenuExportFLVER0;
    }
}

