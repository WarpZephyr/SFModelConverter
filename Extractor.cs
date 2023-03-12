using System;
using System.IO;
using SoulsFormats;
using SoulsFormats.AC4;

namespace SFModelConverter
{
    internal static class Extractor
    {
        /// <summary>
        /// Find which compressed type a file is and attempt to extract it if found
        /// </summary>
        /// <param name="path">A string containing the path to a compressed FromSoft file</param>
        public static void Extract(string path)
        {
            if (BND3.Is(path)) ExtractBND3(path);
            if (BND4.Is(path)) ExtractBND4(path);
            if (BHD5.IsBHD(path)) ExtractBHD5(path);
            if (BXF3.IsBHD(path)) ExtractBXF3(path);
            if (BXF4.IsBHD(path)) ExtractBXF4(path);
            if (Zero3.Is(path)) ExtractZero3(path);

            try { if (SoulsFormats.ACE3.BND0.Is(path)) ExtractACE3BND0(path); } 
            catch (Exception ace3Ex) 
            { 
                Logger.LogExceptionWithDate(ace3Ex, $"{Path.GetFileName(path)} magic is BND0 but failed to be read as ACE3 BND0");
                try { if (SoulsFormats.Kuon.BND0.Is(path)) ExtractKuonBND0(path); }
                catch (Exception kuonEx)
                {
                    Logger.LogExceptionWithDate(kuonEx, $"{Path.GetFileName(path)} magic is BND0 but failed to be read as Kuon BND0");
                    try { ExtractKuonDVDBND0(path); }
                    catch (Exception kuonDVDEx)
                    {
                        Logger.LogExceptionWithDate(kuonDVDEx, $"{Path.GetFileName(path)} magic is BND0 but failed to be read as Kuon DVDBND0");
                        Logger.LogExceptionWithDate(kuonDVDEx, $"{Path.GetFileName(path)} magic is BND0 but failed to extract on all supported BND0 types");
                    }
                }
            }
        }

        /// <summary>
        /// Extract a BND3 file from a given path
        /// </summary>
        /// <param name="path">A string containing the path to a BND3 file</param>
        public static void ExtractBND3(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a BND3");

            try
            {
                BND3 bnd3 = BND3.Read(path);
                string sourceDir = Path.GetDirectoryName(path);
                string filename = Path.GetFileName(path);
                string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
                foreach (BinderFile file in bnd3.Files)
                {
                    string outPath = $@"{targetDir}\{file.Name.Replace('/', '\\')}";
                    Directory.CreateDirectory(outPath);
                    File.WriteAllBytes(outPath, file.Bytes);
                }

                Logger.LogWithDate($"{Path.GetFileName(path)} extracted as a BND3 successfully");
            }
            catch (Exception ex)
            {
                Logger.LogExceptionWithDate(ex, $"{Path.GetFileName(path)} magic is BND3 but failed to be read");
            }
        }

        /// <summary>
        /// Extract a BND4 file from a given path
        /// </summary>
        /// <param name="path">A string containing the path to a BND4 file</param>
        public static void ExtractBND4(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a BND4");

            try
            {
                BND4 bnd4 = BND4.Read(path);
                string sourceDir = Path.GetDirectoryName(path);
                string filename = Path.GetFileName(path);
                string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
                foreach (BinderFile file in bnd4.Files)
                {
                    string outPath = $@"{targetDir}\{file.Name.Replace('/', '\\')}";
                    Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                    File.WriteAllBytes(outPath, file.Bytes);
                }

                Logger.LogWithDate($"{Path.GetFileName(path)} extracted as a BND4 successfully");
            }
            catch(Exception ex)
            {
                Logger.LogExceptionWithDate(ex, $"{Path.GetFileName(path)} magic is BND4 but failed to be read");
            }
        }

        /// <summary>
        /// Extract a BDT file with it's Ds1 - ACVD BHD5 file from a given Ds1 - ACVD BHD5 path and user selected BDT path
        /// </summary>
        /// <param name="path">A string containing the path to a Ds1 - ACVD BHD5 file</param>
        public static void ExtractBHD5(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a BHD5, guessing as Ds1 - ACVD BHD5");

            try
            {
                BHD5 bhd5 = BHD5.Read(path, BHD5.Game.DarkSouls1);
                string bdtPath = Util.GetFilePath("BDT file", "BDT file (*.bdt)|*.bdt|All files (*.*)|*.*");
                if (!BHD5.IsBDT(bdtPath))
                {
                    Logger.LogWithDate($"User selected file: {Path.GetFileName(bdtPath)} did not read as a BHD5 BDF3 BDT");
                    return;
                }

                FileStream bdtStream = File.OpenRead(bdtPath);
                string sourceDir = Path.GetDirectoryName(path);
                string filename = Path.GetFileName(path);
                string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
                foreach (BHD5.Bucket bucket in bhd5.Buckets)
                {
                    foreach (BHD5.FileHeader header in bucket)
                    {
                        byte[] file = header.ReadFile(bdtStream);
                        string outPath = $@"{targetDir}\{header.FileNameHash}";
                        Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                        File.WriteAllBytes(outPath, file);
                    }
                }

                Logger.LogWithDate($"{Path.GetFileName(bdtPath)} with {Path.GetFileName(path)} extracted as a BDT with a Ds1 - ACVD BHD5 successfully");
            }
            catch(Exception ex)
            {
                Logger.LogExceptionWithDate(ex, $"{Path.GetFileName(path)} magic is BHD5 but {Path.GetFileName(path)} or the selected BDT failed to be read as a Ds1 - ACVD BHD5");
            }
        }

        /// <summary>
        /// Extract a BDT file with it's BXF3 file from a given BXF3 path and user selected BDT path
        /// </summary>
        /// <param name="path">A string containing the path to a BXF3 file</param>
        public static void ExtractBXF3(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a BXF3");
            string bdtPath = Util.GetFilePath("BDT file", "BDT file (*.bdt)|*.bdt|All files (*.*)|*.*");
            if (!BXF3.IsBDT(bdtPath))
            {
                Logger.LogWithDate($"User selected file: {Path.GetFileName(bdtPath)} did not read as a BXF3 BDT");
                return;
            }

            try
            {
                
                BXF3 bxf = BXF3.Read(path, bdtPath);
                string sourceDir = Path.GetDirectoryName(path);
                string filename = Path.GetFileName(path);
                string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
                foreach (BinderFile file in bxf.Files)
                {
                    string outPath = $@"{targetDir}\{file.Name.Replace('/', '\\')}";
                    Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                    File.WriteAllBytes(outPath, file.Bytes);
                }

                Logger.LogWithDate($"{Path.GetFileName(bdtPath)} with {Path.GetFileName(path)} extracted as a BDT with BXF3 successfully");
            }
            catch(Exception ex)
            {
                Logger.LogExceptionWithDate(ex, $"{Path.GetFileName(path)} and {Path.GetFileName(bdtPath)} magic is BXF3 and BDT but {Path.GetFileName(path)} or {Path.GetFileName(bdtPath)} failed to be read");
            }
        }

        /// <summary>
        /// Extract a BDT file with it's BXF4 file from a given BXF4 path and user selected BDT path
        /// </summary>
        /// <param name="path">A string containing the path to a BXF4 file</param>
        public static void ExtractBXF4(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a BXF4");
            string bdtPath = Util.GetFilePath("BDT file", "BDT file (*.bdt)|*.bdt|All files (*.*)|*.*");
            if (!BXF4.IsBDT(bdtPath))
            {
                Logger.LogWithDate($"User selected file: {Path.GetFileName(bdtPath)} did not read as a BXF4 BDT");
                return;
            }

            try
            {
                BXF4 bxf = BXF4.Read(path, bdtPath);
                string sourceDir = Path.GetDirectoryName(path);
                string filename = Path.GetFileName(path);
                string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
                foreach (BinderFile file in bxf.Files)
                {
                    string outPath = $@"{targetDir}\{file.Name.Replace('/', '\\')}";
                    Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                    File.WriteAllBytes(outPath, file.Bytes);
                }

                Logger.LogWithDate($"{Path.GetFileName(bdtPath)} with {Path.GetFileName(path)} extracted as a BDT with BFX4 successfully");
            }
            catch(Exception ex)
            {
                Logger.LogExceptionWithDate(ex, $"{Path.GetFileName(path)} and {Path.GetFileName(bdtPath)} magic is BXF4 and BDT but {Path.GetFileName(path)} or {Path.GetFileName(bdtPath)} failed to be read");
            }
        }

        /// <summary>
        /// Extract a singular Zero3 file from a given path
        /// </summary>
        /// <param name="path">A string containing the path to a singular Zero3 file</param>
        public static void ExtractZero3(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a Zero3");

            try
            {
                Zero3 zero3 = Zero3.Read(path);
                string sourceDir = Path.GetDirectoryName(path);
                string filename = Path.GetFileName(path);
                string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
                foreach (Zero3.File file in zero3.Files)
                {
                    string outPath = $@"{targetDir}\{file.Name.Replace('/', '\\')}";
                    Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                    File.WriteAllBytes(outPath, file.Bytes);
                }

                Logger.LogWithDate($"{Path.GetFileName(path)} extracted as a singular Zero3 successfully");
            }
            catch(Exception ex)
            {
                Logger.LogExceptionWithDate(ex, $"{Path.GetFileName(path)} magic is Zero3 but {Path.GetFileName(path)} failed to be read");
            }
        }

        /// <summary>
        /// Extract a ACE3 BND0 file from a given path
        /// </summary>
        /// <param name="path">A string containing the path to a ACE3 BN0 file</param>
        public static void ExtractACE3BND0(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a BND0, guessing Another Century: Episode 3 BND0");

            SoulsFormats.ACE3.BND0 bnd0 = SoulsFormats.ACE3.BND0.Read(path);
            string sourceDir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
            foreach (SoulsFormats.ACE3.BND0.File file in bnd0.Files)
            {
                string outPath = $@"{targetDir}\{file.ID}";
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                File.WriteAllBytes(outPath, file.Bytes);
            }

            Logger.LogWithDate($"{Path.GetFileName(path)} extracted as a Another Century: Episode 3 BND0 successfully");
        }

        /// <summary>
        /// Extract a Kuon BND0 file from a given path
        /// </summary>
        /// <param name="path">A string containing the path to a Kuon BND4 file</param>
        public static void ExtractKuonBND0(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a BND0, guessing Kuon BND0");

            SoulsFormats.Kuon.BND0 bnd0 = SoulsFormats.Kuon.BND0.Read(path);
            string sourceDir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
            foreach (SoulsFormats.Kuon.BND0.File file in bnd0.Files)
            {
                string outPath = $@"{targetDir}\{file.ID}";
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                File.WriteAllBytes(outPath, file.Bytes);
            }

            Logger.LogWithDate($"{Path.GetFileName(path)} extracted as a Kuon BND0 successfully");
        }

        /// <summary>
        /// Extract a Kuon DVDBND0 file from a given path
        /// </summary>
        /// <param name="path">A string containing the path to a Kuon DVDBND0 file</param>
        public static void ExtractKuonDVDBND0(string path)
        {
            Logger.LogWithDate($"{Path.GetFileName(path)} detected as a BND0, guessing Kuon DVDBND0");

            SoulsFormats.Kuon.DVDBND0 bnd0 = SoulsFormats.Kuon.DVDBND0.Read(path);
            string sourceDir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            string targetDir = $"{sourceDir}\\{filename.Replace('.', '-')}";
            foreach (SoulsFormats.Kuon.DVDBND0.File file in bnd0.Files)
            {
                string outPath = $@"{targetDir}\{file.ID}";
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                File.WriteAllBytes(outPath, file.Bytes);
            }

            Logger.LogWithDate($"{Path.GetFileName(path)} extracted as a Kuon DVDBND0 successfully");
        }
    }
}
