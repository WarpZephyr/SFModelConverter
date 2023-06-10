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
                Bone newBone = new()
                {
                    Name = bone.Name,
                    OffsetMatrix = bone.ComputeLocalTransform().ToAssimpMatrix4x4()
                };

                // Get weights
                foreach (FLVER0.Mesh mesh in model.Meshes)
                {
                    foreach (int boneIndice in mesh.BoneIndices)
                    {
                        if (boneIndice == -1)
                            continue;
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
                foreach (int[] array in mesh.GetFaceVertexIndices(model.Header.Version))
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
            FLVER2 oldmodel = FLVER2.Read(flver2ModelPath);

            FLVER0 newmodel = new()
            {
                Bones = oldmodel.Bones,
                Dummies = oldmodel.Dummies,
                Header = new FLVER0.FLVERHeader()
                {
                    BigEndian = true,
                    BoundingBoxMax = oldmodel.Header.BoundingBoxMax,
                    BoundingBoxMin = oldmodel.Header.BoundingBoxMin,
                    Unicode = oldmodel.Header.Unicode,
                    Unk4A = 1,
                    Unk4B = 0,
                    Unk4C = 65535,
                    Unk5C = 0,
                    Version = 20,
                    VertexIndexSize = 16
                }
            };

            oldmodel.Meshes.ForEach(x => newmodel.Meshes.Add(GetMesh(x, oldmodel)));
            oldmodel.Materials.ForEach(x => newmodel.Materials.Add(GetMaterial(x)));

            string donorPath = PathUtil.GetFilePath("C:\\Users", "Select a FLVER0 model to replace");
            FLVER0 donor = FLVER0.Read(donorPath);

            //flver0Model.Materials = flver0DonorModel.Materials;
            newmodel.Dummies = donor.Dummies;
            newmodel.Bones = donor.Bones;

            foreach (var mesh in newmodel.Meshes)
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
            if (!File.Exists($"{donorPath}.bak")) File.Copy(donorPath, $"{donorPath}.bak");
            newmodel.Write(donorPath);
        }

        public static FLVER0.Mesh GetMesh(FLVER2.Mesh mesh, FLVER2 model)
        {
            var material = model.Materials[mesh.MaterialIndex];
            material.MTD = Path.GetFileName(material.MTD).ToLower();

            FLVER0.Mesh meshd = new()
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
                meshd.BoneIndices[i] = index;
            }

            return meshd;
        }

        public static FLVER0.Material GetMaterial(FLVER2.Material material)
        {
            var layout = GetLayout(material.MTD, out MTD mtd);
            var mat = new FLVER0.Material()
            {
                MTD = material.MTD,
                Name = material.Name,
                Layouts = new List<FLVER0.BufferLayout>() { layout },
                Textures = GetTextures(material.Textures, mtd)
            };
            return mat;
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
            var layout = Layouts[Path.GetFileName(mtd.ShaderPath)][0];
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
