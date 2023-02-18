using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using SoulsFormats;
using System;

namespace ACFAModelReplacer
{
    internal static class Dumper
    {
        private static Dictionary<string, MTD> mtds = JsonConvert.DeserializeObject<Dictionary<string, MTD>>(File.ReadAllText($@"{Util.resFolderPath}\FLVER0\mtds.json"));
        private static Dictionary<string, List<FLVER0.BufferLayout>> layouts = new();
        private static List<string> bufferDupCheck = new();
        private static Dictionary<string, MTD> newMtds = new();

        // Dump one flv's buffer layouts
        public static void DumpBuffer(FLVER0 flv, string name) // DEBUG: Passing flv name for debug logging
        {
            foreach (var mesh in flv.Meshes)
            {
                var mtd = flv.Materials[mesh.MaterialIndex].MTD;
                var buffer = flv.Materials[mesh.MaterialIndex].Layouts[0];

                mtd = Path.GetFileNameWithoutExtension(mtd).ToLower();
                bool mtdBool = mtds.TryGetValue(mtd, out MTD mtdFile);

                if (!mtdBool) { continue; }
                string line = Path.GetFileName(mtdFile.ShaderPath);
                foreach (var member in buffer)
                {
                    line += $",|{member.Semantic},{member.Type},{member.Unk00},{member.Index},{member.Size}| ";
                }

                if (bufferDupCheck.Contains(line)) { Logger.Log($"{name} had a duplicate buffer"); continue; } // DEBUG: Log line inside if statement
                Logger.Log($"{name} had a new buffer"); // DEBUG: Log line
                bufferDupCheck.Add(line);

                string shaderName = Path.GetFileName(mtdFile.ShaderPath);
                if (!layouts.ContainsKey(shaderName))
                {
                    layouts.Add(shaderName, new List<FLVER0.BufferLayout>() { buffer });
                }
                else
                {
                    layouts[shaderName].Add(buffer);
                }
            }
        }

        // Dump all MTDs from user selection
        public static void DumpMTDs(List<string> mtdPaths)
        {
            foreach (string mtdFile in mtdPaths)
            {
                string fName = Path.GetFileNameWithoutExtension(mtdFile);
                MTD mtd = MTD.Read(mtdFile);
                newMtds.Add(fName.ToLower(), mtd);
            }

            var json = JsonConvert.SerializeObject(newMtds, Formatting.Indented);
            File.WriteAllText($"{Util.resFolderPath}/FLVER0/newmtds.json", json);
        }

        // Dump all flv buffer layouts from user flv selection
        public static void DumpBuffers(List<string> flvPaths)
        {
            foreach (string flvFile in flvPaths)
            {
                string fName = Path.GetFileNameWithoutExtension(flvFile);
                FLVER0 flv = FLVER0.Read(flvFile);
                DumpBuffer(flv, fName);
            }

            var json = JsonConvert.SerializeObject(newMtds, Formatting.Indented);
            File.WriteAllText($"{Util.resFolderPath}/FLVER0/newmtds.json", json);
        }

        // Recurse BND files to dump flv buffer layouts or mtds
        private static void DoBND(BND3 bnd, string bndName) // DEBUG: Passing BND name for debug logging
        {
            foreach (var bFile in bnd.Files)
            {
                if (FLVER0.Is(bFile.Bytes))
                {
                    try
                    {
                        try
                        {
                            DumpBuffer(FLVER0.Read(bFile.Bytes), bFile.Name); // DEBUG: Passing flv name for debug logging
                            Logger.Log($"{bFile.Name} from inside {bndName} successfully dumped"); // DEBUG: Log line
                        }
                        catch (NotImplementedException notie) // TODO: Find correct reading for Layout BoneIndices
                        {
                            Logger.LogExceptionWithDate(notie, $"{bFile.Name} has BoneIndices that are not implemented yet");
                            throw;
                        }
                    }
                    catch (InvalidDataException ide) // TODO: Figure out how to read 0x11 header for 104 flv files
                    {
                        Logger.LogExceptionWithDate(ide, $"{bFile.Name} failed to read properly and passed FLVER0 check");
                        //throw;
                    }
                }
                else if (MTD.Is(bFile.Bytes))
                {
                    MTD mtd = MTD.Read(bFile.Bytes);
                    newMtds.Add(bFile.Name.ToLower(), mtd);
                }
                else if (BND3.Is(bFile.Bytes))
                {
                    Logger.Log($"{bFile.Name} is a nested BND3 and will extract"); // DEBUG: Log line
                    DoBND(BND3.Read(bFile.Bytes), bFile.Name);
                }
            }
        }

        // Do a recursive dump searching for flv buffer layouts or mtds
        public static dynamic RecursiveDump<T>(string path, string intention)
        {
            if (path == null) { return null; }
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.bnd", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    Logger.LogWithDate($"{Path.GetFileName(file)} is a root BND3 and will extract");
                    DoBND(BND3.Read(file), Path.GetFileName(file)); // DEBUG: Passing BND name for debug logging
                }
            }
            else if (BND3.Is(path)) 
            {
                Logger.LogWithDate($"{Path.GetFileName(path)} is a root BND3 and will extract");
                DoBND(BND3.Read(path), Path.GetFileName(path)); // DEBUG: Passing BND name for debug logging
            }

            if (intention == "layout") { return layouts; };
            if (intention == "mtd") { return newMtds; };
            return null;
        }
    }
}
