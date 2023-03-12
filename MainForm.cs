using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Assimp;
using Newtonsoft.Json;
using SoulsFormats;
using SoulsFormats.AC4;
using SoulsFormats.Other;

namespace SFModelConverter
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

            // Disable image beside menu strip sub items
            ((ToolStripDropDownMenu)FileMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)ExportFMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)ConvertFMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)DumpFMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)FlipMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)SwapMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)HelpMS.DropDown).ShowImageMargin = false;

            Logger.CreateLog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        // Convert FLVER2 model to FLVER0 model
        private void OpenFMS_click(object sender, EventArgs e)
        {
            StatusLabel.Text = "Not yet implemented - use replace";
            string flver2ModelPath = Util.GetFilePath("FLVER2 Model to convert");
            //FLVER0 flver0 = Converter.ReplaceFlver0Flver2(flver2ModelPath);
            //string savePath = Util.GetSavePath("");
            //flver0.Write(savePath);
            //ConversionResultStatusLabelMFSS.Text = "Conversion Successful";
        }

        // Replace FLVER0 with model from FLVER2
        private void ReplaceFLVER0FMS_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "";
            string flver2ModelPath = Util.GetFilePath("FLVER2 Model to convert");
            //string flver0DonorModelPath = Util.GetFilePath("FLVER0 Model to replace");
            if (flver2ModelPath == null) { StatusLabel.Text = "FLVER2 model path was null"; return; }
            StatusLabel.Text = "Converting....";
            Convert.ReplaceFlver0Flver2(flver2ModelPath);
            //if (!File.Exists($"{flver0DonorModelPath}.bak")) File.Copy(flver0DonorModelPath, $"{flver0DonorModelPath}.bak");
            //replacedFlver0Model.Write(flver0DonorModelPath);
            StatusLabel.Text = "Conversion Successful";
        }

        // Dump a selection of mtds
        private void MTDDumpEFMS_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "";
            string[] mtdPaths = Util.GetFilePaths("MTD files");
            if (mtdPaths == null) { StatusLabel.Text = "Selected path was null"; return; }
            StatusLabel.Text = "Wait for MTD dump....";
            Dumper.DumpMTDs(mtdPaths);
            StatusLabel.Text = $"Finished dumping mtds to {Util.resFolderPath}/FLVER0/newmtds.json successfully";
        }

        // Dump buffer layouts from a selection of flvs
        private void BufferLayoutDumpEFMS_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "";
            string[] flvPaths = Util.GetFilePaths("FLV files");
            if (flvPaths == null) { StatusLabel.Text = "Selected path was null"; return; }
            StatusLabel.Text = "Wait for buffer layout dump....";
            Dumper.DumpBuffers(flvPaths);
            StatusLabel.Text = $"Finished dumping buffer layouts to {Util.resFolderPath}/FLVER0/newlayouts.json successfully";
        }

        // Recursively dump mtds from material bnd
        private void RecurseMTDDumpEFMS_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "";
            string gameDir = Util.GetFolderPath("Armored Core: For Answer USRDIR folder");
            string mtdBND = $"{gameDir}/bind/material.bnd";
            if (gameDir == null || mtdBND == null) { StatusLabel.Text = "Selected path or material path was null"; return; }
            StatusLabel.Text = "Wait for recursive MTD dump....";
            Dictionary<string, MTD> newMtds = Dumper.RecursiveDump<Dictionary<string, MTD>>(mtdBND, "mtd");
            if (newMtds == null) { StatusLabel.Text = "New mtds were returned empty"; return; }
            var json = JsonConvert.SerializeObject(newMtds, Formatting.Indented);
            File.WriteAllText($"{Util.resFolderPath}/FLVER0/newmtds.json", json);
            StatusLabel.Text = $"Finished dumping mtds to {Util.resFolderPath}/FLVER0/newmtds.json successfully";
        }

        // Recursively dump buffer layouts from all flvs in model directory
        private void RecurseBufferLayoutDumpEFMS_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "";
            string gameDir = Util.GetFolderPath("Armored Core: For Answer USRDIR folder");
            string modelDir = $"{gameDir}/bind/model/";
            if (gameDir == null || modelDir == null) { StatusLabel.Text = "Selected path or model path was null"; return; }
            StatusLabel.Text = "Wait a long time for recursive buffer layout dump....";
            Dictionary<string, List<FLVER0.BufferLayout>> newLayouts = Dumper.RecursiveDump<Dictionary<string, List<FLVER0.BufferLayout>>>(modelDir, "layout");
            if (newLayouts == null) { StatusLabel.Text = "New layouts were returned empty"; return; }
            var json = JsonConvert.SerializeObject(newLayouts, Formatting.Indented);
            File.WriteAllText($"{Util.resFolderPath}/FLVER0/newlayouts.json", json);
            StatusLabel.Text = $"Finished dumping buffer layouts to {Util.resFolderPath}/FLVER0/newlayouts.json successfully";
        }

        // Decompresses multiple file types
        private void ExtractFileMS_Click(object sender, EventArgs e)
        {
            string path = Util.GetFilePath("compressed SoulsFormats file to extract",
                "BND0/3/4 file (*.bnd)|*.bnd|" +
                "BND0/3/4 file (*.bin)|*.bin|" +
                "BXF3/4 file (*.bhd)|*.bhd|" +
                "BHD5 file (*.bhd)|*.bhd|" +
                "BHD5 file (*.bhd5)|*.bhd5|" +
                "Zero3 file (*.000)|*.000|" +
                "All files (*.*)|*.*"
            );

            if (path == null) return;
            Extractor.Extract(path);
            StatusLabel.Text = $"Extraction completed, check {Path.GetFileName(Logger.log)}";
        }

        // Model Conversion Buttons
        private void ConvertFBXCFMS_Click(object sender, EventArgs e) { ConvertModelFile("fbx"); }
        private void ConvertFBXACFMS_Click(object sender, EventArgs e) { ConvertModelFile("fbxa"); }
        private void ConvertDAECFMS_Click(object sender, EventArgs e) { ConvertModelFile("collada"); }
        private void ConvertOBJCFMS_Click(object sender, EventArgs e) { ConvertModelFile("OBJ"); }

        // Model Export Buttons
        private void ExportFBXEFMS_Click(object sender, EventArgs e) { ExportModelFile("fbx"); }
        private void ExportFBXAEFMS_Click(object sender, EventArgs e) { ExportModelFile("fbxa"); }
        private void ExportDAEEFMS_Click(object sender, EventArgs e) { ExportModelFile("collada"); }
        private void ExportOBJEFMS_Click(object sender, EventArgs e) { ExportModelFile("obj"); }

        /// <summary>
        /// Fast conversion from model file to export type model file
        /// Handles all conversion in one method
        /// </summary>
        /// <param name="type">A string containing the type to export to</param>
        private void ConvertModelFile(string type)
        {
            // Clear status label and get model path
            StatusLabel.Text = "";
            string path = Util.GetFilePath("Choose a model to export", "FLVER0 file (*.flv)|*.flv|FLVER0 file (*.flver)|*.flver|MDL4 file (*.mdl)|*.mdl|All files (*.*)|*.*");
            if (path == null) return;

            // Make extension to name file and status text type to display
            string extension = type switch
            {
                "fbx" => "fbx",
                "fbxa" => "fbx",
                "collada" => "dae",
                "obj" => "obj",
                _ => type
            };
            string statustext = type switch
            {
                "fbx" => "FBX",
                "fbxa" => "Ascii FBX",
                "collada" => "Collada DAE",
                "obj" => "OBJ",
                _ => type,
            };

            // Update user and make save path
            StatusLabel.Text = $"Converting model to {statustext}...";
            string savePath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.{extension}";

            // Attempt conversion
            bool success;
            if (FLVER0.Is(path)) success = Convert.Export(FLVER0.Read(path), savePath, type);
            else
            {
                StatusLabel.Text = "This file is not a model that can be converted right now, or not a model at all";
                return;
            }

            // Tell user if export succeeded
            if (success) StatusLabel.Text = $"Model converted to {statustext} successfully";
            else StatusLabel.Text = $"Model failed conversion to {statustext}";
        }

        /// <summary>
        /// Export file using many methods
        /// Slow conversion
        /// </summary>
        /// <param name="type">A string containing the type to export to</param>
        private void ExportModelFile(string type)
        {
            // Clear status label and get model path
            StatusLabel.Text = "";
            string path = Util.GetFilePath("Choose a model to export", "FLVER0 file (*.flv)|*.flv|FLVER0 file (*.flver)|*.flver|MDL4 file (*.mdl)|*.mdl|All files (*.*)|*.*");
            if (path == null) return;

            // Make status text type to display
            string statustext = type switch
            {
                "fbx" => "FBX",
                "fbxa" => "Ascii FBX",
                "collada" => "Collada DAE",
                "obj" => "OBJ",
                _ => type,
            };

            // Update user
            StatusLabel.Text = $"Exporting model to {statustext}...";

            // Attempt export
            bool success = Export.ExportModel(path, type, GetFlipOptions(), GetSwapOptions());
            // Tell user if export succeeded
            if (success) StatusLabel.Text = $"Model exported to {statustext} successfully";
            else StatusLabel.Text = $"Model failed export to {statustext}";
        }

        /// <summary>
        /// Retrieves all the bools from each flip option
        /// </summary>
        /// <returns>Array of flip option bool arrays</returns>
        private bool[][] GetFlipOptions()
        {
            bool[] vertexFlips = { FlipVertexX.Checked, FlipVertexY.Checked, FlipVertexZ.Checked };
            bool[] normalFlips = { FlipNormalX.Checked, FlipNormalY.Checked, FlipNormalZ.Checked };
            bool[] tangentFlips = { FlipTangentX.Checked, FlipTangentY.Checked, FlipTangentZ.Checked, FlipTangentW.Checked };
            bool[] bitangentFlips = { FlipBiTangentX.Checked, FlipBiTangentY.Checked, FlipBiTangentZ.Checked, FlipBiTangentW.Checked };
            return new bool[][] { vertexFlips, normalFlips, tangentFlips, bitangentFlips };
        }

        /// <summary>
        /// Retrieves all the bools from each swap option
        /// </summary>
        /// <returns>Array of swap option bool arrays</returns>
        private bool[][] GetSwapOptions()
        {
            bool[] vertexSwaps = { SwapVertexXZ.Checked, SwapVertexXY.Checked, SwapVertexYZ.Checked };
            bool[] normalSwaps = { SwapNormalXZ.Checked, SwapNormalXY.Checked, SwapNormalYZ.Checked };
            bool[] tangentSwaps = { SwapTangentXZ.Checked, SwapTangentXY.Checked, SwapTangentXW.Checked, SwapTangentYZ.Checked, SwapTangentYW.Checked, SwapTangentZW.Checked };
            bool[] bitangentSwaps = { SwapBiTangentXZ.Checked, SwapBiTangentXY.Checked, SwapBiTangentXW.Checked, SwapBiTangentYZ.Checked, SwapBiTangentYW.Checked, SwapBiTangentZW.Checked };
            return new bool[][] { vertexSwaps, normalSwaps, tangentSwaps, bitangentSwaps };
        }

        private void CheckFLVERFMS_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "";
            string path = Util.GetFilePath("FLVER file");
            if (FLVER0.Is(path)) StatusLabel.Text = "FLVER0";
            if (FLVER2.Is(path)) StatusLabel.Text = "FLVER2";
        }

        // TODO: Show About form message
        private void AboutHMS_Click(object sender, EventArgs e)
        {

        }
    }
}
