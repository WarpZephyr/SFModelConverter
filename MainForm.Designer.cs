namespace SFModelConverter
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
            this.MenuReplace = new System.Windows.Forms.ToolStripMenuItem();
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
            this.MenuReplace,
            this.MenuCheckModel,
            this.toolStripSeparator1,
            this.MenuExport,
            this.MenuDump});
            this.FileMS.ForeColor = System.Drawing.SystemColors.Control;
            this.FileMS.Name = "FileMS";
            this.FileMS.Size = new System.Drawing.Size(37, 20);
            this.FileMS.Text = "File";
            // 
            // MenuReplace
            // 
            this.MenuReplace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuReplace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuReplace.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuReplace.Name = "MenuReplace";
            this.MenuReplace.Size = new System.Drawing.Size(180, 22);
            this.MenuReplace.Text = "Replace";
            this.MenuReplace.ToolTipText = "Replaces ACFA FLVER0 model with FLVER2 model";
            this.MenuReplace.Click += new System.EventHandler(this.MenuReplace_Click);
            // 
            // MenuCheckModel
            // 
            this.MenuCheckModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.MenuCheckModel.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuCheckModel.Name = "MenuCheckModel";
            this.MenuCheckModel.Size = new System.Drawing.Size(180, 22);
            this.MenuCheckModel.Text = "Check Model Type";
            this.MenuCheckModel.Click += new System.EventHandler(this.MenuCheckModel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
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
            this.MenuExportObj});
            this.MenuExport.ForeColor = System.Drawing.SystemColors.Control;
            this.MenuExport.Name = "MenuExport";
            this.MenuExport.Size = new System.Drawing.Size(180, 22);
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
            this.MenuExportFBX.ToolTipText = "Export imported model to FBX";
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
            this.MenuExportAcsiiFbx.ToolTipText = "Export imported model to Ascii FBX";
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
            this.MenuExportColladaDae.ToolTipText = "Export imported model to Collada DAE";
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
            this.MenuExportObj.ToolTipText = "Export imported model to OBJ";
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
            this.MenuDump.Size = new System.Drawing.Size(180, 22);
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
            this.MenuDumpMtd.Size = new System.Drawing.Size(180, 22);
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
            this.MenuDumpLayout.Size = new System.Drawing.Size(180, 22);
            this.MenuDumpLayout.Text = "Dump layouts";
            this.MenuDumpLayout.ToolTipText = "Dump Buffer Layouts from MTD files into layouts JSON in resources";
            this.MenuDumpLayout.Click += new System.EventHandler(this.MenuDumpLayout_Click);
            // 
            // MainFormStatusStrip
            // 
            this.MainFormStatusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.MainFormStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.MainFormStatusStrip.Location = new System.Drawing.Point(0, 22);
            this.MainFormStatusStrip.Name = "MainFormStatusStrip";
            this.MainFormStatusStrip.Size = new System.Drawing.Size(759, 22);
            this.MainFormStatusStrip.TabIndex = 2;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(759, 44);
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
        private System.Windows.Forms.ToolStripMenuItem MenuReplace;
        private System.Windows.Forms.ToolStripMenuItem MenuExport;
        private System.Windows.Forms.ToolStripMenuItem MenuCheckModel;
        private System.Windows.Forms.ToolStripMenuItem MenuExportColladaDae;
        private System.Windows.Forms.ToolStripMenuItem MenuExportFBX;
        private System.Windows.Forms.ToolStripMenuItem MenuExportObj;
        private System.Windows.Forms.ToolStripMenuItem MenuExportAcsiiFbx;
    }
}

