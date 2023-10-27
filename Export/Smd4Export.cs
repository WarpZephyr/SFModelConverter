using SoulsFormats;
using System.IO;

namespace SFModelConverter
{
    public static class Smd4Export
    {
        public static bool ExportModel(string path)
        {
            string outPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.smd";
            string backupPath = $"{outPath}.bak";

            SMD4 newModel;
            if (FLVER0.Is(path))
            {
                newModel = ToSmd4(FLVER0.Read(path));
            }
            else if (MDL4.Is(path))
            {
                newModel = ToSmd4(MDL4.Read(path));
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

        public static SMD4 ToSmd4(FLVER0 model)
        {
            // Add Header
            SMD4 newModel = new SMD4();
            newModel.Header.BoundingBoxMin = model.Header.BoundingBoxMin;
            newModel.Header.BoundingBoxMax = model.Header.BoundingBoxMax;

            // Add UnkIndices
            newModel.UnkIndices.Add(0);

            // Add Bones
            foreach (var bone in model.Bones)
            {
                SMD4.Bone newBone = new SMD4.Bone();
                newBone.Name = bone.Name;
                newBone.Translation = bone.Translation;
                newBone.Rotation = bone.Rotation;
                newBone.Scale = bone.Scale;
                newBone.BoundingBoxMin = bone.BoundingBoxMin;
                newBone.BoundingBoxMax = bone.BoundingBoxMax;
                newBone.ParentIndex = bone.ParentIndex;
                newBone.ChildIndex = bone.ChildIndex;
                newBone.NextSiblingIndex = bone.NextSiblingIndex;
                newBone.PreviousSiblingIndex = bone.PreviousSiblingIndex;
                newModel.Bones.Add(newBone);
            }

            // Add meshes
            foreach (var mesh in model.Meshes)
            {
                SMD4.Mesh newMesh = new SMD4.Mesh();
                newMesh.Dynamic = mesh.Dynamic;
                newMesh.MaterialIndex = mesh.MaterialIndex;
                newMesh.CullBackfaces = mesh.CullBackfaces;
                newMesh.TriangleStrip = mesh.TriangleStrip;
                newMesh.DefaultBoneIndex = mesh.DefaultBoneIndex;

                // Add Vertex Indices
                foreach (var vertexIndice in mesh.VertexIndices)
                {
                    newMesh.VertexIndices.Add((ushort)vertexIndice);
                }

                // Add Vertices
                foreach (var vertex in mesh.Vertices)
                {
                    SMD4.Vertex newVertex = new SMD4.Vertex();
                    newVertex.Position = vertex.Position;
                    newVertex.BoneIndex = (short)vertex.NormalW;
                    newMesh.Vertices.Add(newVertex);
                }

                newModel.Meshes.Add(newMesh);
            }

            return newModel;
        }

        public static SMD4 ToSmd4(MDL4 model)
        {
            // Add Header
            SMD4 newModel = new SMD4();
            newModel.Header.BoundingBoxMin = model.Header.BoundingBoxMin;
            newModel.Header.BoundingBoxMax = model.Header.BoundingBoxMax;

            // Add UnkIndices
            newModel.UnkIndices.Add(0);

            // Add Bones
            foreach (var bone in model.Bones)
            {
                SMD4.Bone newBone = new SMD4.Bone();
                newBone.Name = bone.Name;
                newBone.Translation = bone.Translation;
                newBone.Rotation = bone.Rotation;
                newBone.Scale = bone.Scale;
                newBone.BoundingBoxMin = bone.BoundingBoxMin;
                newBone.BoundingBoxMax = bone.BoundingBoxMax;
                newBone.ParentIndex = bone.ParentIndex;
                newBone.ChildIndex = bone.ChildIndex;
                newBone.NextSiblingIndex = bone.NextSiblingIndex;
                newBone.PreviousSiblingIndex = bone.PreviousSiblingIndex;
                newModel.Bones.Add(newBone);
            }

            // Add meshes
            foreach (var mesh in model.Meshes)
            {
                SMD4.Mesh newMesh = new SMD4.Mesh();
                newMesh.Dynamic = 0;
                newMesh.MaterialIndex = mesh.MaterialIndex;
                newMesh.CullBackfaces = mesh.CullBackfaces;
                newMesh.TriangleStrip = mesh.TriangleStrip;
                newMesh.DefaultBoneIndex = mesh.DefaultBoneIndex;

                // Add Vertex Indices
                foreach (var vertexIndice in mesh.VertexIndices)
                {
                    newMesh.VertexIndices.Add(vertexIndice);
                }

                // Add Vertices
                foreach (var vertex in mesh.Vertices)
                {
                    SMD4.Vertex newVertex = new SMD4.Vertex();
                    newVertex.Position = vertex.Position;
                    newVertex.BoneIndex = (short)vertex.Normal.W;
                    newMesh.Vertices.Add(newVertex);
                }

                newModel.Meshes.Add(newMesh);
            }

            return newModel;
        }
    }
}
