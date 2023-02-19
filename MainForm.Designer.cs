namespace ACFAModelReplacer
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
            this.OpenFMS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportFMS = new System.Windows.Forms.ToolStripMenuItem();
            this.MTDDumpEFMS = new System.Windows.Forms.ToolStripMenuItem();
            this.BufferLayoutDumpEFMS = new System.Windows.Forms.ToolStripMenuItem();
            this.RecurseMTDDumpEFMS = new System.Windows.Forms.ToolStripMenuItem();
            this.RecurseBufferLayoutDumpEFMS = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMS = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutHMS = new System.Windows.Forms.ToolStripMenuItem();
            this.MainFormStatusStrip = new System.Windows.Forms.StatusStrip();
            this.ConversionResultStatusLabelMFSS = new System.Windows.Forms.ToolStripStatusLabel();
            this.ReplaceFLVER0FMS = new System.Windows.Forms.ToolStripMenuItem();
            this.Extract000FMS = new System.Windows.Forms.ToolStripMenuItem();
            this.MainFormMenuStrip.SuspendLayout();
            this.MainFormStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainFormMenuStrip
            // 
            this.MainFormMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.MainFormMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMS,
            this.HelpMS});
            this.MainFormMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainFormMenuStrip.Name = "MainFormMenuStrip";
            this.MainFormMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MainFormMenuStrip.Size = new System.Drawing.Size(759, 24);
            this.MainFormMenuStrip.TabIndex = 1;
            this.MainFormMenuStrip.Text = "MainFormMenuStrip";
            // 
            // FileMS
            // 
            this.FileMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.FileMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.FileMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileMS.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFMS,
            this.ReplaceFLVER0FMS,
            this.Extract000FMS,
            this.toolStripSeparator1,
            this.ExportFMS});
            this.FileMS.ForeColor = System.Drawing.SystemColors.Control;
            this.FileMS.Name = "FileMS";
            this.FileMS.Size = new System.Drawing.Size(37, 20);
            this.FileMS.Text = "File";
            // 
            // OpenFMS
            // 
            this.OpenFMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.OpenFMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.OpenFMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OpenFMS.ForeColor = System.Drawing.SystemColors.Control;
            this.OpenFMS.Name = "OpenFMS";
            this.OpenFMS.Size = new System.Drawing.Size(180, 22);
            this.OpenFMS.Text = "Open";
            this.OpenFMS.ToolTipText = "Converts FLVER2 Models to FLVER0 Models";
            this.OpenFMS.Click += new System.EventHandler(this.OpenFMS_click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // ExportFMS
            // 
            this.ExportFMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ExportFMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ExportFMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ExportFMS.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MTDDumpEFMS,
            this.BufferLayoutDumpEFMS,
            this.RecurseMTDDumpEFMS,
            this.RecurseBufferLayoutDumpEFMS});
            this.ExportFMS.ForeColor = System.Drawing.SystemColors.Control;
            this.ExportFMS.Name = "ExportFMS";
            this.ExportFMS.Size = new System.Drawing.Size(180, 22);
            this.ExportFMS.Text = "Export";
            // 
            // MTDDumpEFMS
            // 
            this.MTDDumpEFMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.MTDDumpEFMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MTDDumpEFMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MTDDumpEFMS.ForeColor = System.Drawing.SystemColors.Control;
            this.MTDDumpEFMS.Name = "MTDDumpEFMS";
            this.MTDDumpEFMS.Size = new System.Drawing.Size(288, 22);
            this.MTDDumpEFMS.Text = "Dump FLVER0 MTDs";
            this.MTDDumpEFMS.ToolTipText = "Dump FLVER0 MTD files to mtds JSON in resources";
            this.MTDDumpEFMS.Click += new System.EventHandler(this.MTDDumpEFMS_Click);
            // 
            // BufferLayoutDumpEFMS
            // 
            this.BufferLayoutDumpEFMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.BufferLayoutDumpEFMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BufferLayoutDumpEFMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BufferLayoutDumpEFMS.ForeColor = System.Drawing.SystemColors.Control;
            this.BufferLayoutDumpEFMS.Name = "BufferLayoutDumpEFMS";
            this.BufferLayoutDumpEFMS.Size = new System.Drawing.Size(288, 22);
            this.BufferLayoutDumpEFMS.Text = "Dump FLVER0 Buffer Layouts";
            this.BufferLayoutDumpEFMS.ToolTipText = "Dump Buffer Layouts from MTD files into layouts JSON in resources";
            this.BufferLayoutDumpEFMS.Click += new System.EventHandler(this.BufferLayoutDumpEFMS_Click);
            // 
            // RecurseMTDDumpEFMS
            // 
            this.RecurseMTDDumpEFMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.RecurseMTDDumpEFMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.RecurseMTDDumpEFMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RecurseMTDDumpEFMS.ForeColor = System.Drawing.SystemColors.Control;
            this.RecurseMTDDumpEFMS.Name = "RecurseMTDDumpEFMS";
            this.RecurseMTDDumpEFMS.Size = new System.Drawing.Size(288, 22);
            this.RecurseMTDDumpEFMS.Text = "Recursively Dump FLVER0 MTDs";
            this.RecurseMTDDumpEFMS.ToolTipText = "Recursively dumps mtds from USRDIR to newmtds.json";
            this.RecurseMTDDumpEFMS.Click += new System.EventHandler(this.RecurseMTDDumpEFMS_Click);
            // 
            // RecurseBufferLayoutDumpEFMS
            // 
            this.RecurseBufferLayoutDumpEFMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.RecurseBufferLayoutDumpEFMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.RecurseBufferLayoutDumpEFMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RecurseBufferLayoutDumpEFMS.ForeColor = System.Drawing.SystemColors.Control;
            this.RecurseBufferLayoutDumpEFMS.Name = "RecurseBufferLayoutDumpEFMS";
            this.RecurseBufferLayoutDumpEFMS.Size = new System.Drawing.Size(288, 22);
            this.RecurseBufferLayoutDumpEFMS.Text = "Recursively Dump FLVER0 Buffer Layouts";
            this.RecurseBufferLayoutDumpEFMS.ToolTipText = "Recursively dumps buffer layouts from USRDIR to newlayouts.json";
            this.RecurseBufferLayoutDumpEFMS.Click += new System.EventHandler(this.RecurseBufferLayoutDumpEFMS_Click);
            // 
            // HelpMS
            // 
            this.HelpMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.HelpMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.HelpMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.HelpMS.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutHMS});
            this.HelpMS.ForeColor = System.Drawing.SystemColors.Control;
            this.HelpMS.Name = "HelpMS";
            this.HelpMS.Size = new System.Drawing.Size(44, 20);
            this.HelpMS.Text = "Help";
            // 
            // AboutHMS
            // 
            this.AboutHMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.AboutHMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AboutHMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AboutHMS.ForeColor = System.Drawing.SystemColors.Control;
            this.AboutHMS.Name = "AboutHMS";
            this.AboutHMS.Size = new System.Drawing.Size(107, 22);
            this.AboutHMS.Text = "About";
            this.AboutHMS.ToolTipText = "TODO: About this program";
            this.AboutHMS.Click += new System.EventHandler(this.AboutHMS_Click);
            // 
            // MainFormStatusStrip
            // 
            this.MainFormStatusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.MainFormStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConversionResultStatusLabelMFSS});
            this.MainFormStatusStrip.Location = new System.Drawing.Point(0, 22);
            this.MainFormStatusStrip.Name = "MainFormStatusStrip";
            this.MainFormStatusStrip.Size = new System.Drawing.Size(759, 22);
            this.MainFormStatusStrip.TabIndex = 2;
            // 
            // ConversionResultStatusLabelMFSS
            // 
            this.ConversionResultStatusLabelMFSS.Name = "ConversionResultStatusLabelMFSS";
            this.ConversionResultStatusLabelMFSS.Size = new System.Drawing.Size(0, 17);
            // 
            // ReplaceFLVER0FMS
            // 
            this.ReplaceFLVER0FMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ReplaceFLVER0FMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ReplaceFLVER0FMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ReplaceFLVER0FMS.ForeColor = System.Drawing.SystemColors.Control;
            this.ReplaceFLVER0FMS.Name = "ReplaceFLVER0FMS";
            this.ReplaceFLVER0FMS.Size = new System.Drawing.Size(180, 22);
            this.ReplaceFLVER0FMS.Text = "Replace FLVER0";
            this.ReplaceFLVER0FMS.ToolTipText = "Replaces FLVER0 model with FLVER2 model";
            this.ReplaceFLVER0FMS.Click += new System.EventHandler(this.ReplaceFLVER0FMS_Click);
            // 
            // Extract000FMS
            // 
            this.Extract000FMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Extract000FMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Extract000FMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Extract000FMS.ForeColor = System.Drawing.SystemColors.Control;
            this.Extract000FMS.Name = "Extract000FMS";
            this.Extract000FMS.Size = new System.Drawing.Size(180, 22);
            this.Extract000FMS.Text = "Extract 000";
            this.Extract000FMS.ToolTipText = "Decompresses 000 file to get AC4 model";
            this.Extract000FMS.Click += new System.EventHandler(this.Extract000FMS_Click);
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
            this.Text = "Armored Core: For Answer Model Replacer";
            this.Load += new System.EventHandler(this.MainForm_Load);
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
        private System.Windows.Forms.ToolStripMenuItem ExportFMS;
        private System.Windows.Forms.ToolStripMenuItem HelpMS;
        private System.Windows.Forms.ToolStripMenuItem AboutHMS;
        private System.Windows.Forms.StatusStrip MainFormStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ConversionResultStatusLabelMFSS;
        private System.Windows.Forms.ToolStripMenuItem OpenFMS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem RecurseMTDDumpEFMS;
        private System.Windows.Forms.ToolStripMenuItem RecurseBufferLayoutDumpEFMS;
        private System.Windows.Forms.ToolStripMenuItem MTDDumpEFMS;
        private System.Windows.Forms.ToolStripMenuItem BufferLayoutDumpEFMS;
        private System.Windows.Forms.ToolStripMenuItem ReplaceFLVER0FMS;
        private System.Windows.Forms.ToolStripMenuItem Extract000FMS;
    }
}

