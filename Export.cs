using System.IO;
using System.Numerics;
using System.Collections.Generic;
using SoulsFormats;
using Assimp;
using System.Linq;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Assimp.Unmanaged;
using System;

namespace SFModelConverter
{
    /// <summary>
    /// Class for exporting SoulsFormats models using Assimp.
    /// </summary>
    internal static class Export
    {
        /// <summary>
        /// Exports a FLVER0 model to the chosen model type using assimp.
        /// </summary>
        /// <param name="path">The path to a FLVER0 model.</param>
        /// <param name="type">The type to export.</param>
        /// <returns>Whether or not the export was successful.</returns>
        public static bool ExportModel(string path, string type)
        {
            string outPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.{GetExtension(type)}";

            if (!FLVER0.Is(path))
                return false;

            // Read model and make scene
            var model = FLVER0.Read(path);
            var scene = new Scene();
            var aiRootNode = new Node();
            scene.RootNode = aiRootNode;
            aiRootNode.Transform = Assimp.Matrix4x4.Identity;

            // Add bone nodes
            Node[] boneArray = new Node[model.Bones.Count];
            for (int i = 0; i < model.Bones.Count; i++)
            {
                var bone = model.Bones[i];
                Node parentNode;
                if (bone.ParentIndex == -1)
                {
                    parentNode = aiRootNode;
                }
                else
                {
                    parentNode = boneArray[bone.ParentIndex];
                }
                var aiNode = new Node(bone.Name, parentNode);

                // Get local transform
                aiNode.Transform = bone.ComputeLocalTransform().ToAssimpMatrix4x4();

                parentNode.Children.Add(aiNode);
                boneArray[i] = aiNode;
            }

            // Add materials
            foreach (var material in model.Materials)
            {
                Material newMaterial = new()
                {
                    Name = material.Name,
                };

                scene.Materials.Add(newMaterial);
            }

            // Add meshes
            for (int meshIndex = 0; meshIndex < model.Meshes.Count; meshIndex++)
            {
                var mesh = model.Meshes[meshIndex];
                var newMesh = new Mesh("Mesh_M" + meshIndex, PrimitiveType.Triangle);
                bool hasBones = mesh.BoneIndices.Length != 0;

                // Add bones
                if (hasBones)
                {
                    var aiBone = new Bone();
                    var boneNode = boneArray[mesh.Vertices[0].NormalW];
                    aiBone.Name = boneNode.Name;

                    // VertexWeights
                    for (int i = 0; i < newMesh.Vertices.Count; i++)
                    {
                        var aiVertexWeight = new VertexWeight(i, 1f);
                        aiBone.VertexWeights.Add(aiVertexWeight);
                    }

                    aiBone.OffsetMatrix = Assimp.Matrix4x4.Identity;
                    newMesh.Bones.Add(aiBone);
                }

                // Add vertices
                for (int vertexIndex = 0; vertexIndex < mesh.Vertices.Count; vertexIndex++)
                {
                    var vertex = mesh.Vertices[vertexIndex];
                    var bone = model.Bones[mesh.BoneIndices[vertex.NormalW]];

                    if (hasBones)
                    {
                        var transform = ModelUtil.ComputeTransformNonDynamic(model, mesh, vertex);
                        
                        newMesh.Vertices.Add(vertex.Position.ToAssimpVector3D(transform));
                        newMesh.Normals.Add(vertex.Normal.ToAssimpVector3D(transform));
                        newMesh.BiTangents.Add(vertex.Bitangent.ToAssimpVector3D(transform));
                        foreach (var tangent in vertex.Tangents)
                        {
                            newMesh.Tangents.Add(tangent.ToAssimpVector3D(transform));
                        }
                    }
                    else
                    {
                        newMesh.Vertices.Add(vertex.Position.ToAssimpVector3D());
                        newMesh.Normals.Add(vertex.Normal.ToAssimpVector3D());
                        newMesh.BiTangents.Add(vertex.Bitangent.ToAssimpVector3D());
                        foreach (var tangent in vertex.Tangents)
                        {
                            newMesh.Tangents.Add(tangent.ToAssimpVector3D());
                        }
                    }

                    // Add UVs
                    if (vertex.UVs.Count > 0)
                    {
                        var uv1 = vertex.UVs[0];
                        var aiTextureCoordinate = new Vector3D(uv1.X, uv1.Y, 0f);
                        newMesh.TextureCoordinateChannels[0].Add(aiTextureCoordinate);
                    }
                    else
                    {
                        var aiTextureCoordinate = new Vector3D(1, 1, 1);
                        newMesh.TextureCoordinateChannels[0].Add(aiTextureCoordinate);
                    }

                    if (vertex.UVs.Count > 1)
                    {
                        var uv2 = vertex.UVs[1];
                        var aiTextureCoordinate = new Vector3D(uv2.X, uv2.Y, 0f);
                        newMesh.TextureCoordinateChannels[1].Add(aiTextureCoordinate);
                    }
                    else
                    {
                        var aiTextureCoordinate = new Vector3D(1, 1, 1);
                        newMesh.TextureCoordinateChannels[1].Add(aiTextureCoordinate);
                    }

                    if (vertex.UVs.Count > 2)
                    {
                        var uv3 = vertex.UVs[2];
                        var aiTextureCoordinate = new Vector3D(uv3.X, uv3.Y, 0f);
                        newMesh.TextureCoordinateChannels[2].Add(aiTextureCoordinate);
                    }
                    else
                    {
                        var aiTextureCoordinate = new Vector3D(1, 1, 1);
                        newMesh.TextureCoordinateChannels[2].Add(aiTextureCoordinate);
                    }

                    if (vertex.UVs.Count > 3)
                    {
                        var uv4 = vertex.UVs[3];
                        var aiTextureCoordinate = new Vector3D(uv4.X, uv4.Y, 0f);
                        newMesh.TextureCoordinateChannels[3].Add(aiTextureCoordinate);
                    }
                    else
                    {
                        var aiTextureCoordinate = new Vector3D(1, 1, 1);
                        newMesh.TextureCoordinateChannels[3].Add(aiTextureCoordinate);
                    }

                    for (int uv = 0; uv < newMesh.TextureCoordinateChannelCount; uv++)
                    {
                        newMesh.UVComponentCount[uv] = 2;
                    }

                    // Add vertex colors
                    if (vertex.Colors.Count > 0)
                    {
                        var color = vertex.Colors[0];
                        newMesh.VertexColorChannels[0].Add(new Color4D(color.R, color.G, color.B, color.A));
                    }
                    if (vertex.Colors.Count > 1)
                    {
                        var color = vertex.Colors[1];
                        newMesh.VertexColorChannels[1].Add(new Color4D(color.R, color.G, color.B, color.A));
                    }
                }

                // Add faces
                var faceVertexIndices = mesh.GetFaceVertexIndices(model.Header.Version);
                foreach (int[] indices in faceVertexIndices)
                {
                    newMesh.Faces.Add(new Face(indices));
                }

                newMesh.MaterialIndex = mesh.MaterialIndex;
                scene.Meshes.Add(newMesh);

                var meshNode = new Node($"Mesh_{meshIndex}", scene.RootNode);
                meshNode.Transform = Assimp.Matrix4x4.Identity;

                meshNode.MeshIndices.Add(meshIndex);
                scene.RootNode.Children.Add(meshNode);
            }

            // Export scene to save path
            return new AssimpContext().ExportFile(scene, outPath, type);
        }

        /// <summary>
        /// Get the extension for a type.
        /// </summary>
        /// <param name="type">A supported Assimp type as a string.</param>
        /// <returns>A file extension without the dot.</returns>
        public static string GetExtension(string type)
        {
            return type switch
            {
                "fbx" => "fbx",
                "fbxa" => "fbx",
                "collada" => "dae",
                "obj" => "obj",
                _ => type
            };
        }
    }
}
