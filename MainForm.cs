using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using CustomForms;
using Newtonsoft.Json;
using SoulsFormats;
using SoulsFormats.Other;
using Utilities;

namespace SFModelConverter
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Loaded mtds for dumping purposes.
        /// </summary>
        private static Dictionary<string, MTD> Mtds;

        /// <summary>
        /// The current model type.
        /// </summary>
        private static string CurrentModelType = "FLVER0";

        /// <summary>
        /// A path to mtds.json.
        /// </summary>
        private static string MtdsJsonPath = $"{PathUtil.ResFolderPath}\\{CurrentModelType}\\mtds.json";

        public MainForm()
        {
            InitializeComponent();
            var menuRenderer = new SelectableColorToolStripRenderer();
            MainFormMenuStrip.Renderer = menuRenderer;

            ((ToolStripDropDownMenu)FileMS.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)MenuExport.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)MenuDump.DropDown).ShowImageMargin = false;

            if (File.Exists(MtdsJsonPath))
                Mtds = JsonConvert.DeserializeObject<Dictionary<string, MTD>>(File.ReadAllText(MtdsJsonPath));
            else
                StatusLabel.Text = "Warning: Could not find mtds.json in res folder";

            Logger.CreateLog();
        }

        private void MenuCheckModel_Click(object sender, EventArgs e)
        {
            string path = PathUtil.GetFilePath("FLVER file");
            if (path == null)
                return;

            if (FLVER0.Is(path))
                StatusLabel.Text = "FLVER0 was detected";
            else if (FLVER2.Is(path))
                StatusLabel.Text = "FLVER2 was detected";
            else if (MDL4.Is(path))
                StatusLabel.Text = "MDL4 was detected";
            else if (MDL.Is(path))
                StatusLabel.Text = "MDL was detected";
            else if (SMD4.Is(path))
                StatusLabel.Text = "SMD4 was detected";
            else
                StatusLabel.Text = "Nothing was detected";
        }

        private void MenuExportFbx_Click(object sender, EventArgs e)
        {
            AssimpExportModel("fbx");
        }

        private void MenuExportAcsiiFbx_Click(object sender, EventArgs e)
        {
            AssimpExportModel("fbxa");
        }

        private void MenuExportColladaDae_Click(object sender, EventArgs e)
        {
            AssimpExportModel("collada");
        }

        private void MenuExportObj_Click(object sender, EventArgs e)
        {
            AssimpExportModel("obj");
        }

        private void MenuExportSMD4_Click(object sender, EventArgs e)
        {
            string path = PathUtil.GetFilePath("C:\\Users", "Choose a model to export",
                "FLVER file (*.flv)|*.flv|FLVER file (*.flver)|*.flver|MDL4 file (*.mdl)|*.mdl|All files (*.*)|*.*");
            if (path == null)
                return;

            if (Smd4Export.ExportModel(path))
                StatusLabel.Text = $"Model exported successfully";
            else
                StatusLabel.Text = $"Model failed export";
        }

        private void MenuExportFLVER0_Click(object sender, EventArgs e)
        {
            string path = PathUtil.GetFilePath("C:\\Users", "Choose a model to export",
                "FLVER2 file (*.flv)|*.flv|FLVER2 file (*.flver)|*.flver|All files (*.*)|*.*");
            if (path == null)
                return;

            var mtdPaths = new List<string>();
            mtdPaths.AddRange(MtdPathsTextBox.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

            if (Flver0Export.ExportModel(path, mtdPaths))
                StatusLabel.Text = $"Model exported successfully";
            else
                StatusLabel.Text = $"Model failed export";
        }

        private void MenuDumpMtd_Click(object sender, EventArgs e)
        {
            string[] paths = PathUtil.GetFilePaths("C:\\Users", "MTD files");
            if (paths == null)
                return;

            Dictionary<string, MTD> newMtds = new();

            foreach (string path in paths)
            {
                string fName = Path.GetFileNameWithoutExtension(path);
                MTD mtd = MTD.Read(path);
                newMtds.Add(fName.ToLower(), mtd);
            }

            var json = JsonConvert.SerializeObject(newMtds, Formatting.Indented);
            string outPath = PathUtil.GetSavePath("C:\\Users", "Choose where to save dumped mtds", "Json (*.json)|*.json");
            if (outPath == null)
            {
                StatusLabel.Text = "Canceled mtd dump";
                return;
            }

            File.WriteAllText(outPath, json);
            StatusLabel.Text = $"Finished dumping mtds successfully.";
        }

        private void MenuDumpLayout_Click(object sender, EventArgs e)
        {
            string[] paths = PathUtil.GetFilePaths("C:\\Users", "FLVER files");
            if (paths == null)
                return;

            var layouts = new Dictionary<string, List<FLVER0.BufferLayout>>();

            foreach (string path in paths)
            {
                FLVER0 model = FLVER0.Read(path);
                foreach (var mesh in model.Meshes)
                {
                    var mtdPath = model.Materials[mesh.MaterialIndex].MTD;
                    var buffer = model.Materials[mesh.MaterialIndex].Layouts[0];

                    mtdPath = Path.GetFileNameWithoutExtension(mtdPath).ToLower();
                    bool existing = Mtds.TryGetValue(mtdPath, out MTD mtdFile);
                    if (!existing)
                        continue;

                    string line = Path.GetFileName(mtdFile.ShaderPath);
                    foreach (var member in buffer)
                        line += $",|{member.Semantic},{member.Type},{member.Unk00},{member.Index},{member.Size}| ";

                    //BufferDuplicateCheck.Add(line);

                    string shaderName = Path.GetFileName(mtdFile.ShaderPath);
                    if (!layouts.ContainsKey(shaderName))
                        layouts.Add(shaderName, new List<FLVER0.BufferLayout>() { buffer });
                    else
                        layouts[shaderName].Add(buffer);
                }
            }

            var json = JsonConvert.SerializeObject(layouts, Formatting.Indented);
            string outPath = PathUtil.GetSavePath("C:\\Users", "Choose where to save dumped buffer layouts.", "Json (*.json)|*.json");
            if (outPath == null)
            {
                StatusLabel.Text = "Canceled mtd dump";
                return;
            }

            File.WriteAllText(outPath, json);

            StatusLabel.Text = $"Finished dumping buffer layouts successfully.";
        }

        private void AssimpExportModel(string type)
        {
            string path = PathUtil.GetFilePath("C:\\Users", "Choose a model to export",
                "FLVER file (*.flv)|*.flv|FLVER file (*.flver)|*.flver|MDL4 file (*.mdl)|*.mdl|SMD4 file (*.smd)|*.smd|All files (*.*)|*.*");
            if (path == null)
                return;

            if (AssimpExport.ExportModel(path, type))
                StatusLabel.Text = $"Model exported successfully";
            else
                StatusLabel.Text = $"Model failed export";
        }

        private void MenuGetModelMtdPaths_Click(object sender, EventArgs e)
        {
            string path = PathUtil.GetFilePath("C:\\Users", "Choose a model to get the MTD paths of.");
            if (path == null)
                return;

            if (FLVER0.Is(path))
            {
                var model = FLVER0.Read(path);
                foreach (var material in model.Materials)
                {
                    if (MtdPathsTextBox.Text != string.Empty)
                    {
                        MtdPathsTextBox.Text += $"\r\n{material.MTD}";
                    }
                    else
                    {
                        MtdPathsTextBox.Text = $"{material.MTD}";
                    }
                }
            }
            else if (FLVER2.Is(path))
            {
                var model = FLVER2.Read(path);
                foreach (var material in model.Materials)
                {
                    if (MtdPathsTextBox.Text != string.Empty)
                    {
                        MtdPathsTextBox.Text += $"\r\n{material.MTD}";
                    }
                    else
                    {
                        MtdPathsTextBox.Text = $"{material.MTD}";
                    }
                }
            }
            else
                StatusLabel.Text = "No compatible model formats were found.";
        }
    }
}
