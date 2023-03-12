using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows.Forms;

namespace SFModelConverter
{
    internal static class Util
    {
        public static string envFolderPath = $"{Environment.CurrentDirectory}/";
        public static string resFolderPath = $"{envFolderPath}res/";

        /// <summary>
        /// Get a single file from the user.
        /// </summary>
        /// <param name="context">An optional argument of a string containing context of what file you want to ask the user to open</param>
        /// <param name="filters">The filetype filters to apply in the dialog box, set to all files by default</param>
        /// <returns>A string containing the path to a file the user selects</returns>
        public static string GetFilePath(string context = null, string filters = "All files (*.*)|*.*")
        {
            OpenFileDialog filePathDialog = new()
            {
                InitialDirectory = "C:\\Users",
                Title = $"{context ?? "Select file"}",
                Filter = filters
            };

            if (filePathDialog.ShowDialog() == DialogResult.OK)
            {
                return filePathDialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Get a single folder from the user.
        /// </summary>
        /// <param name="context">An optional argument of a string containing context of what folder you want to ask the user to open</param>
        /// <returns>A string containing the path to a folder the user selects</returns>
        public static string GetFolderPath(string context = null)
        {
            CommonOpenFileDialog filePathDialog = new()
            {
                InitialDirectory = "C:\\Users",
                IsFolderPicker = true,
                Title = $"{context ?? "Select folder"}",
            };

            if (filePathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return filePathDialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Get a multiple files from the user.
        /// </summary>
        /// <param name="context">An optional argument of a string containing context of what files you want to ask the user to open</param>
        /// <param name="filters">The filetype filters to apply in the dialog box, set to all files by default</param>
        /// <returns>A string array containing all the paths the user selects</returns>
        public static string[] GetFilePaths(string context = null, string filters = "All files (*.*)|*.*")
        {
            OpenFileDialog filePathDialog = new()
            {
                InitialDirectory = "C:\\Users",
                Multiselect = true,
                Title = $"{context ?? "Select files"}",
                Filter = filters
            };

            if (filePathDialog.ShowDialog() == DialogResult.OK)
            {
                return filePathDialog.FileNames;
            }

            return null;
        }

        /// <summary>
        /// Get a save path from the user.
        /// </summary>
        /// <param name="context">An optional argument of a string containing context of what you want to ask the user to save</param>
        /// <param name="filters">The filetype filters to apply in the dialog box, set to all files by default</param>
        /// <returns>A string containing the path where the user wants to save a file</returns>
        public static string GetSavePath(string context = null, string filters = "All files (*.*)|*.*")
        {
            SaveFileDialog saveFileDialog = new()
            {
                InitialDirectory = "C:\\Users",
                Title = $"{context ?? "Choose where to save file"}",
                Filter = filters
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}
