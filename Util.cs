using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;

namespace ACFAModelReplacer
{
    internal static class Util
    {
        public static string envFolderPath = $"{Environment.CurrentDirectory}/";
        public static string resFolderPath = $"{Environment.CurrentDirectory}/res/";
        public static string acfaModelReplacerLog = $"{envFolderPath}/acfamodelreplacer.log";
        public static string stacktraceLog = $"{envFolderPath}/stacktrace.log";

        // Get single file
        public static string GetFilePath(string context)
        {
            CommonOpenFileDialog filePathDialog = new()
            {
                InitialDirectory = "C:\\Users",
                Title = $"Select your {context}",
        };

            if (filePathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return filePathDialog.FileName;
            }

            return null;
        }

        // Get single folder
        public static string GetFolderPath(string context)
        {
            CommonOpenFileDialog filePathDialog = new()
            {
                InitialDirectory = "C:\\Users",
                IsFolderPicker = true,
                Title = $"Select your {context}",
            };

            if (filePathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return filePathDialog.FileName;
            }

            return null;
        }

        // Get mutliple files
        public static List<string> GetFilePaths(string context)
        {
            CommonOpenFileDialog filePathDialog = new()
            {
                InitialDirectory = "C:\\Users",
                Multiselect = true,
                Title = $"Select your {context}"
            };

            if (filePathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                List<string> filePathList = new();
                foreach (string filePath in filePathDialog.FileNames)
                {
                    filePathList.Add(filePath);
                }
                return filePathList;
            }

            return null;
        }
    }
}
