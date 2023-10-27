using System.Numerics;
using SoulsFormats;
using System.IO;
using SoulsFormats.Other;
using Assimp;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using Utilities;
using Newtonsoft.Json;
using System;

namespace SFModelConverter
{
    /// <summary>
    /// Class for converting SoulsFormats models to other formats using Assimp, might be replaced by Export class
    /// </summary>
    internal static class Convert
    {
        /// <summary>
        /// The current model type.
        /// </summary>
        private static string CurrentModelType = "FLVER0";

        /// <summary>
        /// A path to mtds.json.
        /// </summary>
        private static string MtdsJsonPath = $"{PathUtil.ResFolderPath}\\{CurrentModelType}\\mtds.json";

        /// <summary>
        /// A path to layouts.json.
        /// </summary>
        private static string LayoutsJsonPath = $"{PathUtil.ResFolderPath}\\{CurrentModelType}\\layouts.json";

        /// <summary>
        /// The loaded mtds.
        /// </summary>
        private static Dictionary<string, MTD> Mtds = JsonConvert.DeserializeObject<Dictionary<string, MTD>>(File.ReadAllText(MtdsJsonPath));

        /// <summary>
        /// The loaded layouts.
        /// </summary>
        private static Dictionary<string, List<FLVER0.BufferLayout>> Layouts = JsonConvert.DeserializeObject<Dictionary<string, List<FLVER0.BufferLayout>>>(File.ReadAllText(LayoutsJsonPath));

        // Temporary until full solution is found
        public static void ReplaceFlver0Flver2(string flver2ModelPath)
        {
            string donorPath = PathUtil.GetFilePath("C:\\Users", "Select a FLVER0 model to replace");
            FLVER0 donor = FLVER0.Read(donorPath);
            FLVER2 model = FLVER2.Read(flver2ModelPath);

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
                    Version = 20,
                    VertexIndexSize = 16
                }
            };

            foreach (var mesh in model.Meshes)
            {
                newModel.Meshes.Add(GetMesh(mesh));
            }

            foreach (var material in model.Materials)
            {
                newModel.Materials.Add(GetMaterial(material));
            }

            newModel.Dummies = donor.Dummies;
            newModel.Bones = donor.Bones;

            if (!File.Exists($"{donorPath}.bak"))
            {
                File.Copy(donorPath, $"{donorPath}.bak");
            }

            newModel.Write(donorPath);
        }

        public static FLVER0.Mesh GetMesh(FLVER2.Mesh mesh)
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

            return newMesh;
        }

        public static FLVER0.Material GetMaterial(FLVER2.Material material)
        {
            var newLayout = GetLayout(material.MTD, out MTD mtd);
            var newMaterial = new FLVER0.Material()
            {
                //MTD = material.MTD,
                MTD = material.MTD,
                Name = material.Name,
                Layouts = new List<FLVER0.BufferLayout>() { newLayout },
                Textures = GetTextures(material.Textures, mtd)
            };
            return newMaterial;
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
