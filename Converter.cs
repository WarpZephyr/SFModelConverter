using System.Numerics;
using SoulsFormats;
using Assimp;

namespace ACFAModelReplacer
{
    internal static class Converter
    {
        /*public static FLVER2 ConvertFbxFlver2(object fbxModel)
        {
            FLVER2 ds1Model;
            return ds1Model;
        }*/

        public static FLVER0 ConvertFlver2Flver0(FLVER2 flver2Model)
        {
            FLVER0 flver0Model = new()
            {
                Bones = flver2Model.Bones,
                Dummies = flver2Model.Dummies,
                Header = new FLVER0.FLVERHeader()
                {
                    BoundingBoxMax = flver2Model.Header.BoundingBoxMax,
                    BoundingBoxMin = flver2Model.Header.BoundingBoxMin,
                    Unicode = flver2Model.Header.Unicode,
                    VertexIndexSize = 16
                }
            };

            flver2Model.Meshes.ForEach(x => flver0Model.Meshes.Add(FLVER2Convert.GetMesh(x, flver2Model)));
            flver2Model.Materials.ForEach(x => flver0Model.Materials.Add(FLVER2Convert.GetMaterial(x)));
            return flver0Model;
        }

        // Temporary until full solution is found
        public static FLVER0 ReplaceFlver0Flver2(string flver2ModelPath, string flver0DonorModelPath)
        {
            FLVER2 flver2Model = FLVER2.Read(flver2ModelPath);
            FLVER0 flver0Model = ConvertFlver2Flver0(flver2Model);
            FLVER0 flver0DonorModel = FLVER0.Read(flver0DonorModelPath);

            flver0DonorModel.Header.Version = flver0Model.Header.Version;
            flver0DonorModel.Materials = flver0Model.Materials;
            flver0DonorModel.Dummies = flver0Model.Dummies;
            flver0DonorModel.Bones = flver0Model.Bones;

            foreach (var mesh in flver0DonorModel.Meshes)
            {
                mesh.MaterialIndex = 0;
                mesh.Dynamic = 0;
                mesh.DefaultBoneIndex = flver0Model.Meshes[0].DefaultBoneIndex;
                foreach (var vert in mesh.Vertices)
                {
                    vert.UVs.Add(vert.UVs[0]);
                    vert.Tangents.Add(Vector4.Zero);
                    vert.NormalW = 0;
                }
                for (int i = 0; i < 28; i++)
                {
                    mesh.BoneIndices[i] = 0;
                }     
            }

            return flver0DonorModel;
        }
    }
}
