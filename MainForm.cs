using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using SoulsFormats;

namespace ACFAModelReplacer
{
    public partial class MainForm : Form
    {
        // MainForm Constructor
        public MainForm()
        {
            InitializeComponent();
            // Use override to change colors of selected Strip items to dark mode and disable shadows
            OverrideToolStripRenderer toolStripOverrideRenderer = new();
            MainFormMenuStrip.Renderer = toolStripOverrideRenderer;

            // Disable Shadows on Dropdowns
            //FileMS.DropDown.DropShadowEnabled = false; // TODO: Fix random location changing when shadow is disabled
            ExportFMS.DropDown.DropShadowEnabled = false;
            HelpMS.DropDown.DropShadowEnabled = false;

            // Disable image beside menu strip sub items
            ((ToolStripDropDownMenu)FileMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)ExportFMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)HelpMS.DropDown).ShowImageMargin = false;
        }

        // Create log file on form load
        private void MainForm_Load(object sender, EventArgs e)
        {
            Logger.createLog();
        }

        // Replace FLVER0 with model from FLVER2
        private void OpenFMS_click(object sender, EventArgs e)
        {
            ConversionResultStatusLabelMFSS.Text = "";
            string flver2ModelPath = Util.GetFilePath("FLVER2 Model to convert");
            string flver0DonorModelPath = Util.GetFilePath("FLVER0 Model to replace");
            if (flver2ModelPath == null || flver0DonorModelPath == null) { ConversionResultStatusLabelMFSS.Text = "FLVER2 model path or FLVER0 Donor path was null"; return; }
            ConversionResultStatusLabelMFSS.Text = "Converting....";
            FLVER0 replacedFlver0Model = Converter.ReplaceFlver0Flver2(flver2ModelPath, flver0DonorModelPath);
            if (!File.Exists($"{flver0DonorModelPath}.bak")) File.Copy(flver0DonorModelPath, $"{flver0DonorModelPath}.bak");
            replacedFlver0Model.Write(flver0DonorModelPath);
            ConversionResultStatusLabelMFSS.Text = "Conversion Successful";
        }

        // TODO: Show About form message
        private void AboutHMS_Click(object sender, EventArgs e)
        {

        }

        // Dump a selection of mtds
        private void MTDDumpEFMS_Click(object sender, EventArgs e)
        {
            ConversionResultStatusLabelMFSS.Text = "";
            List<string> mtdPaths = Util.GetFilePaths("MTD files");
            if (mtdPaths == null) { ConversionResultStatusLabelMFSS.Text = "Selected path was null"; return; }
            ConversionResultStatusLabelMFSS.Text = "Wait for MTD dump....";
            Dumper.DumpMTDs(mtdPaths);
            ConversionResultStatusLabelMFSS.Text = $"Finished dumping mtds to {Util.resFolderPath}/FLVER0/newmtds.json successfully";
        }

        // Dump buffer layouts from a selection of flvs
        private void BufferLayoutDumpEFMS_Click(object sender, EventArgs e)
        {
            ConversionResultStatusLabelMFSS.Text = "";
            List<string> flvPaths = Util.GetFilePaths("FLV files");
            if (flvPaths == null) { ConversionResultStatusLabelMFSS.Text = "Selected path was null"; return; }
            ConversionResultStatusLabelMFSS.Text = "Wait for buffer layout dump....";
            Dumper.DumpBuffers(flvPaths);
            ConversionResultStatusLabelMFSS.Text = $"Finished dumping buffer layouts to {Util.resFolderPath}/FLVER0/newlayouts.json successfully";
        }

        // Recursively dump mtds from material bnd
        private void RecurseMTDDumpEFMS_Click(object sender, EventArgs e)
        {
            ConversionResultStatusLabelMFSS.Text = "";
            string gameDir = Util.GetFolderPath("Armored Core: For Answer USRDIR folder");
            string mtdBND = $"{gameDir}/bind/material.bnd";
            if (gameDir == null || mtdBND == null) { ConversionResultStatusLabelMFSS.Text = "Selected path or material path was null"; return; }
            ConversionResultStatusLabelMFSS.Text = "Wait for recursive MTD dump....";
            Dictionary<string, MTD> newMtds = Dumper.RecursiveDump<Dictionary<string, MTD>>(mtdBND, "mtd");
            if (newMtds == null) { ConversionResultStatusLabelMFSS.Text = "New mtds were returned empty"; return; }
            var json = JsonConvert.SerializeObject(newMtds, Formatting.Indented);
            File.WriteAllText($"{Util.resFolderPath}/FLVER0/newmtds.json", json);
            ConversionResultStatusLabelMFSS.Text = $"Finished dumping mtds to {Util.resFolderPath}/FLVER0/newmtds.json successfully";
        }

        // Recursively dump buffer layouts from all flvs in model directory
        private void RecurseBufferLayoutDumpEFMS_Click(object sender, EventArgs e)
        {
            ConversionResultStatusLabelMFSS.Text = "";
            string gameDir = Util.GetFolderPath("Armored Core: For Answer USRDIR folder");
            string modelDir = $"{gameDir}/bind/model/";
            if (gameDir == null || modelDir == null) { ConversionResultStatusLabelMFSS.Text = "Selected path or model path was null"; return; }
            ConversionResultStatusLabelMFSS.Text = "Wait a long time for recursive buffer layout dump....";
            Dictionary<string, List<FLVER0.BufferLayout>> newLayouts = Dumper.RecursiveDump<Dictionary<string, List<FLVER0.BufferLayout>>>(modelDir, "layout");
            if (newLayouts == null) { ConversionResultStatusLabelMFSS.Text = "New layouts were returned empty"; return; }
            var json = JsonConvert.SerializeObject(newLayouts, Formatting.Indented);
            File.WriteAllText($"{Util.resFolderPath}/FLVER0/newlayouts.json", json);
            ConversionResultStatusLabelMFSS.Text = $"Finished dumping buffer layouts to {Util.resFolderPath}/FLVER0/newlayouts.json successfully";
        }
    }
}
