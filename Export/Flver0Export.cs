using Newtonsoft.Json;
using SoulsFormats;
using System.Collections.Generic;
using System.IO;
using Utilities;

namespace SFModelConverter
{
    public class Flver0Export
    {
        /// <summary>
        /// A path to mtds.json.
        /// </summary>
        private static string MtdsJsonPath = $"{PathUtil.ResFolderPath}\\FLVER0\\mtds.json";

        /// <summary>
        /// A path to layouts.json.
        /// </summary>
        private static string LayoutsJsonPath = $"{PathUtil.ResFolderPath}\\FLVER0\\layouts.json";

        /// <summary>
        /// The loaded mtds.
        /// </summary>
        private static Dictionary<string, MTD> Mtds = JsonConvert.DeserializeObject<Dictionary<string, MTD>>(File.ReadAllText(MtdsJsonPath));

        /// <summary>
        /// The loaded layouts.
        /// </summary>
        private static Dictionary<string, List<FLVER0.BufferLayout>> Layouts = JsonConvert.DeserializeObject<Dictionary<string, List<FLVER0.BufferLayout>>>(File.ReadAllText(LayoutsJsonPath));

        public static bool ExportModel(string path, List<string> mtdPaths)
        {
            string outPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.flv";
            string backupPath = $"{outPath}.bak";

            FLVER0 newModel;
            if (FLVER2.Is(path))
            {
                var model = FLVER2.Read(path);
                if (model.Materials.Count != mtdPaths.Count)
                {
                    return false;
                }
                newModel = ToFlver0(model, mtdPaths);
            }
            else
            {
                return false;
            }

            if (File.Exists(outPath))
                if (!File.Exists(backupPath))
                    File.Move(outPath, backupPath);

            newModel.Write(outPath);
            return true;
        }

        public static FLVER0 ToFlver0(FLVER2 model, List<string> mtdPaths)
        {
            FLVER0 newModel = new()
            {
                Bones = model.Bones,
                Dummies = model.Dummies,
                Header = new FLVER0.FLVERHeader()
                {
                    BigEndian = true,
                    BoundingBoxMax = model.Header.BoundingBoxMax,
                    BoundingBoxMin = model.Header.BoundingBoxMin,
                    Unicode = model.Header.Unicode,
                    Unk4A = 1,
                    Unk4B = 0,
                    Unk4C = 65535,
                    Unk5C = 0,
                    Version = 18,
                    VertexIndexSize = 16
                }
            };

            foreach (var mesh in model.Meshes)
            {
                FLVER0.Mesh newMesh = new()
                {
                    DefaultBoneIndex = (short)mesh.DefaultBoneIndex,
                    Dynamic = mesh.Dynamic,
                    MaterialIndex = (byte)mesh.MaterialIndex,
                    LayoutIndex = 0,
                    Vertices = mesh.Vertices,
                    VertexIndices = mesh.FaceSets[0].Indices,
                    CullBackfaces = mesh.FaceSets[0].CullBackfaces,
                    TriangleStrip = System.Convert.ToByte(mesh.FaceSets[0].TriangleStrip),
                    Unk46 = 0
                };

                for (int i = 0; i < 28; i++)
                {
                    short index = -1;
                    if (i < mesh.BoneIndices.Count)
                        index = (short)mesh.BoneIndices[i];
                    newMesh.BoneIndices[i] = index;
                }

                newModel.Meshes.Add(newMesh);
            }

            for (int i = 0; i < mtdPaths.Count; i++)
            {
                var material = model.Materials[i];
                var newLayout = GetLayout(mtdPaths[i], out MTD mtd);

                var newMaterial = new FLVER0.Material()
                {
                    MTD = material.MTD,
                    Name = material.Name,
                    Layouts = new List<FLVER0.BufferLayout>() { newLayout },
                    Textures = GetTextures(material.Textures, mtd)
                };

                newModel.Materials.Add(newMaterial);
            }

            newModel.Dummies = model.Dummies;
            newModel.Bones = model.Bones;
            return newModel;
        }

        private static FLVER0.BufferLayout GetLayout(string path, out MTD mtd)
        {
            string mtdName = Path.GetFileNameWithoutExtension(path);
            if (!Mtds.ContainsKey(mtdName))
            {
                if (mtdName.EndsWith("_skin"))
                    mtdName = "default_skin";
                else
                    mtdName = "default";
            }
            mtd = Mtds[mtdName];
            var newLayout = Layouts[Path.GetFileName(mtd.ShaderPath)][0];
            return newLayout;
        }

        private static List<FLVER0.Texture> GetTextures(List<FLVER2.Texture> textures, MTD mtd)
        {
            List<FLVER0.Texture> newTextures = new();
            foreach (var texture in mtd.Textures)
            {
                var texure = textures.Find(x => x.Type == texture.Type);
                if (texure != null)
                    newTextures.Add(new FLVER0.Texture() { Path = texure.Path, Type = texure.Type });
            }
            return newTextures;
        }
    }
}
