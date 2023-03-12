using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using SoulsFormats;

namespace SFModelConverter
{
    internal static class FLVER2Convert
    {
        static Dictionary<string, MTD> mtds = JsonConvert.DeserializeObject<Dictionary<string, MTD>>(File.ReadAllText($@"{Util.resFolderPath}\FLVER0\mtds.json"));
        static Dictionary<string, List<FLVER0.BufferLayout>> layouts = JsonConvert.DeserializeObject<Dictionary<string, List<FLVER0.BufferLayout>>>(File.ReadAllText($@"{Util.resFolderPath}\FLVER0\layouts.json"));

        public static FLVER0.Mesh GetMesh(FLVER2.Mesh flver2Mesh, FLVER2 flver2Flv)
        {
            var mat = flver2Flv.Materials[flver2Mesh.MaterialIndex];
            mat.MTD = Path.GetFileName(mat.MTD).ToLower();

            FLVER0.Mesh meshd = new()
            {
                DefaultBoneIndex = (short)flver2Mesh.DefaultBoneIndex,
                Dynamic = flver2Mesh.Dynamic,
                MaterialIndex = (byte)flver2Mesh.MaterialIndex,
                LayoutIndex = 0,
                Vertices = flver2Mesh.Vertices,
                VertexIndices = flver2Mesh.FaceSets[0].Indices,
                CullBackfaces = flver2Mesh.FaceSets[0].CullBackfaces,
                TriangleStrip = System.Convert.ToByte(flver2Mesh.FaceSets[0].TriangleStrip),
                Unk46 = 0
            };

            for (int i = 0; i < 28; i++)
            {
                short index = -1;
                if (i < flver2Mesh.BoneIndices.Count)
                    index = (short)flver2Mesh.BoneIndices[i];
                meshd.BoneIndices[i] = index;
            }

            Console.WriteLine("Mesh conversion succeeded");
            return meshd;
        }

        public static FLVER0.Material GetMaterial(FLVER2.Material flver2Material)
        {
            var layout = GetLayout(flver2Material.MTD, out MTD mtd);
            var mat = new FLVER0.Material()
            {
                MTD = flver2Material.MTD,
                Name = flver2Material.Name,
                Layouts = new List<FLVER0.BufferLayout>() { layout },
                Textures = GetTextures(flver2Material.Textures, mtd)
            };
            return mat;
        }

        private static FLVER0.BufferLayout GetLayout(string mtd, out MTD mtdFile)
        {
            string mtdName = Path.GetFileNameWithoutExtension(mtd);
            if (!mtds.ContainsKey(mtdName))
            {
                if (mtdName.EndsWith("_skin"))
                    mtdName = "default_skin";
                else
                    mtdName = "default";
            }
            mtdFile = mtds[mtdName];
            var layout = layouts[Path.GetFileName(mtdFile.ShaderPath)][0];
            return layout;
        }

        private static List<FLVER0.Texture> GetTextures(List<FLVER2.Texture> flver2TextureList, MTD mtd)
        {
            List<FLVER0.Texture> flver0TextureList = new();
            foreach (var texture in mtd.Textures)
            {
                var flver2Texure = flver2TextureList.Find(x => x.Type == texture.Type);
                if (flver2Texure != null)
                    flver0TextureList.Add(new FLVER0.Texture() { Path = flver2Texure.Path, Type = flver2Texure.Type });
            }
            return flver0TextureList;
        }
    }
}
