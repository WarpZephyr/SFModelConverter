using System.Numerics;
using SoulsFormats;
using System.IO;
using SoulsFormats.Other;
using Assimp;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace SFModelConverter
{
    /// <summary>
    /// Class for converting SoulsFormats models to other formats using Assimp, might be replaced by Export class
    /// </summary>
    internal static class Convert
    {
        /// <summary>
        /// Converts a FLVER0 model into a FBX file, saving the FBX file to the specified path
        /// </summary>
        /// <param name="model">A FLVER0 model</param>
        /// <param name="savePath">A string containing the path of which to save a DAE file</param>
        /// <param name="type">A string containing the type of file to export to</param>
        public static bool Export(FLVER0 model, string savePath, string type)
        {
            Scene scene = new()
            {
                RootNode = new Node()
            };

            // Add materials
            foreach (FLVER0.Material material in model.Materials)
            {
                Material newMaterial = new()
                {
                    Name = material.Name,
                };

                scene.Materials.Add(newMaterial);
            }

            // Create bones
            List<Bone> newBones = new();
            foreach (var bone in model.Bones)
            {
                var boneMatrix = bone.ComputeLocalTransform();
                Assimp.Matrix4x4 newOffsetMatrix = new()
                {
                    A1 = boneMatrix.M11,
                    A2 = boneMatrix.M12,
                    A3 = boneMatrix.M13,
                    A4 = boneMatrix.M14,
                    B1 = boneMatrix.M21,
                    B2 = boneMatrix.M22,
                    B3 = boneMatrix.M23,
                    B4 = boneMatrix.M24,
                    C1 = boneMatrix.M31,
                    C2 = boneMatrix.M32,
                    C3 = boneMatrix.M33,
                    C4 = boneMatrix.M34,
                    D1 = boneMatrix.M41,
                    D2 = boneMatrix.M42,
                    D3 = boneMatrix.M43,
                    D4 = boneMatrix.M44,
                };

                Bone newBone = new()
                {
                    Name = bone.Name,
                    OffsetMatrix = newOffsetMatrix
                };

                // Get weights
                foreach (FLVER0.Mesh mesh in model.Meshes)
                {
                    foreach (int boneIndice in mesh.BoneIndices)
                    {
                        if (boneIndice == -1) continue;
                        for (int j = 0; j < mesh.Vertices.Count; ++j)
                        {
                            var vertex = mesh.Vertices[j];
                            if (vertex.NormalW == boneIndice)
                            {
                                VertexWeight vertexWeight = new(j, 1);
                                newBone.VertexWeights.Add(vertexWeight);
                            }
                        }
                    }
                }

                // Add bone to bone list
                newBones.Add(newBone);
            }

            // Add meshes
            int meshCounter = 0;
            foreach (FLVER0.Mesh mesh in model.Meshes)
            {
                Mesh newMesh = new("Mesh_M" + meshCounter, PrimitiveType.Triangle);
                foreach (FLVER.Vertex vertex in mesh.Vertices)
                {
                    // Add vertex positions
                    if (vertex.NormalW != -1)
                    {
                        // Get bone
                        FLVER.Bone bone = model.Bones[mesh.BoneIndices[vertex.NormalW]];
                        System.Numerics.Matrix4x4 transform = bone.ComputeLocalTransform();
                        while (bone.ParentIndex != 1)
                        {
                            bone = model.Bones[bone.ParentIndex];
                            transform *= bone.ComputeLocalTransform();
                        } 

                        // Add vertex position
                        Vector3 position = vertex.Position;
                        Vector3 newPosition = Vector3.Transform(position, transform);

                        newMesh.Vertices.Add(new Vector3D(-newPosition.Z, newPosition.Y, -newPosition.X));

                        // Add vertex normal
                        Vector3 normal = vertex.Normal;
                        Vector3 newNormal = Vector3.Transform(normal, transform);
                        newMesh.Normals.Add(new Vector3D(-newNormal.Z, newNormal.Y, -newNormal.X));

                        // Add vertex tangent
                        if (vertex.Tangents.Count != 0)
                        {
                            Vector4 tangent = vertex.Tangents[0];
                            Vector4 newTangent = Vector4.Transform(tangent, transform);
                            newMesh.Tangents.Add(new Vector3D(-newTangent.Z, newTangent.Y, -newTangent.X));
                        }

                        // Add vertex bitangent
                        Vector4 bitangent = vertex.Bitangent;
                        Vector4 newBitangent = Vector4.Transform(bitangent, transform);
                        newMesh.BiTangents.Add(new Vector3D(-newBitangent.Z, newBitangent.Y, -newBitangent.X));
                    }
                    else
                    {
                        // Add vertex position
                        Vector3 position = vertex.Position;
                        newMesh.Vertices.Add(new Vector3D(-position.Z, position.Y, -position.X));

                        // Add vertex normal
                        Vector3 normal = vertex.Normal;
                        newMesh.Normals.Add(new Vector3D(-normal.Z, normal.Y, -normal.X));

                        // Add vertex tangent
                        if (vertex.Tangents.Count != 0)
                        {
                            Vector4 tangent = vertex.Tangents[0];
                            newMesh.Tangents.Add(new Vector3D(-tangent.Z, tangent.Y, -tangent.X));
                        }

                        // Add vertex bitangent
                        Vector4 bitangent = vertex.Bitangent;
                        newMesh.BiTangents.Add(new Vector3D(-bitangent.Z, bitangent.Y, -bitangent.X));
                    }

                    // Add vertex UVs
                    List<Vector3> UVs = vertex.UVs;
                    for (int i = 0; i < UVs.Count; ++i)
                    {
                        Vector3 UV = UVs[i];
                        newMesh.TextureCoordinateChannels[i].Add(new Vector3D(UV.X, 1 - UV.Y, 0));
                    }

                    // Add vertex colors
                    List<FLVER.VertexColor> colors = vertex.Colors;
                    for (int i = 0; i < vertex.Colors.Count; ++i)
                    {
                        // Add vertex color
                        var color = vertex.Colors[i];
                        Color4D newColor = new(color.R, color.G, color.B, color.A);
                        newMesh.VertexColorChannels[i].Add(newColor);
                    }
                }

                // Add bones
                /*for (int j = 0; j < mesh.BoneIndices.Length; ++j)
                {
                    if (j == -1) continue;
                    newMesh.Bones.Add(newBones[j]);
                }*/

                newMesh.Bones.Add(new Bone());

                // Add faces
                foreach (int[] array in mesh.GetFacesIndices(model.Header.Version))
                {
                    newMesh.Faces.Add(new Face(array));
                }

                int meshIndex = mesh.MaterialIndex;
                newMesh.MaterialIndex = meshIndex;

                scene.Meshes.Add(newMesh);

                Node node = new()
                {
                    Name = $"M_{meshCounter}_{model.Materials[meshIndex].Name}",
                };

                node.MeshIndices.Add(meshCounter);
                scene.RootNode.Children.Add(node);
                meshCounter++;
            }

            AssimpContext exporter = new();
            bool success = exporter.ExportFile(scene, savePath, type);
            return success;
        }

        // Temporary until full solution is found
        public static void ReplaceFlver0Flver2(string flver2ModelPath)
        {
            FLVER2 flver2Model = FLVER2.Read(flver2ModelPath);

            FLVER0 flver0Model = new()
            {
                Bones = flver2Model.Bones,
                Dummies = flver2Model.Dummies,
                Header = new FLVER0.FLVERHeader()
                {
                    BigEndian = true,
                    BoundingBoxMax = flver2Model.Header.BoundingBoxMax,
                    BoundingBoxMin = flver2Model.Header.BoundingBoxMin,
                    Unicode = flver2Model.Header.Unicode,
                    Unk4A = 1,
                    Unk4B = 0,
                    Unk4C = 65535,
                    Unk5C = 0,
                    Version = 20,
                    VertexIndexSize = 16
                }
            };

            flver2Model.Meshes.ForEach(x => flver0Model.Meshes.Add(FLVER2Convert.GetMesh(x, flver2Model)));
            flver2Model.Materials.ForEach(x => flver0Model.Materials.Add(FLVER2Convert.GetMaterial(x)));

            string flver0DonorModelPath = Util.GetFilePath("FLVER0 model you wish to replace");
            FLVER0 flver0DonorModel = FLVER0.Read(flver0DonorModelPath);

            //flver0Model.Materials = flver0DonorModel.Materials;
            flver0Model.Dummies = flver0DonorModel.Dummies;
            flver0Model.Bones = flver0DonorModel.Bones;

            foreach (var mesh in flver0Model.Meshes)
            {
                mesh.MaterialIndex = 0;
                mesh.Dynamic = 0;
                //mesh.DefaultBoneIndex = flver0DonorModel.Meshes[0].DefaultBoneIndex;
                mesh.DefaultBoneIndex = 0;
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
            if (!File.Exists($"{flver0DonorModelPath}.bak")) File.Copy(flver0DonorModelPath, $"{flver0DonorModelPath}.bak");
            flver0Model.Write(flver0DonorModelPath);
        }
    }
}
