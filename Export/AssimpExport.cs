﻿using System.IO;
using System.Collections.Generic;
using SoulsFormats;
using Assimp;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Newtonsoft.Json;
using Utilities;

namespace SFModelConverter
{
    /// <summary>
    /// Class for exporting SoulsFormats models using Assimp.
    /// </summary>
    internal static class AssimpExport
    {
        /// <summary>
        /// Exports a supported model type to the chosen model type using assimp.
        /// </summary>
        /// <param name="path">The path to a supported model.</param>
        /// <param name="type">The type to export.</param>
        /// <returns>Whether or not the export was successful.</returns>
        public static bool ExportModel(string path, string type)
        {
            string outPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.{GetExtension(type)}";
            string backupPath = $"{outPath}.bak";

            // Read model and make scene
            Scene scene;
            if (FLVER0.Is(path))
            {
                scene = ToAssimpScene(FLVER0.Read(path));
            }
            else if (FLVER2.Is(path))
            {
                scene = ToAssimpScene(FLVER2.Read(path));
            }
            else if (MDL4.Is(path))
            {
                scene = ToAssimpScene(MDL4.Read(path));
            }
            else if (SMD4.Is(path))
            {
                scene = ToAssimpScene(SMD4.Read(path));
            }
            else
            {
                return false;
            }

            // Backup file
            if (File.Exists(outPath))
                if (!File.Exists(backupPath))
                    File.Move(outPath, backupPath);

            // Export scene to save path
            return new AssimpContext().ExportFile(scene, outPath, type);
        }

        /// <summary>
        /// Converts a FLVER0 into an Assimp Scene.
        /// </summary>
        public static Scene ToAssimpScene(FLVER0 model)
        {
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

                // Prepare a bone map
                var boneMap = new Dictionary<int, Bone>(mesh.BoneIndices.Length);

                // Add vertices
                for (int vertexIndex = 0; vertexIndex < mesh.Vertices.Count; vertexIndex++)
                {
                    var vertex = mesh.Vertices[vertexIndex];

                    // If the mesh has bones set the weights of this vertex to the correct bone
                    if (hasBones)
                    {
                        // Get the local bone indice from NormalW then the bone array bone indice from the mesh
                        var boneIndice = mesh.BoneIndices[vertex.NormalW];
                        var bone = model.Bones[boneIndice];
                        var transform = bone.ComputeTransform(model.Bones);

                        newMesh.Vertices.Add(vertex.Position.ToAssimpVector3D(transform));
                        newMesh.Normals.Add(vertex.Normal.ToAssimpVector3D());
                        newMesh.BiTangents.Add(vertex.Bitangent.ToAssimpVector3D());
                        foreach (var tangent in vertex.Tangents)
                        {
                            newMesh.Tangents.Add(tangent.ToAssimpVector3D());
                        }

                        // If the bone map does not already have the bone add it
                        if (!boneMap.ContainsKey(boneIndice))
                        {
                            var aiBone = new Bone();
                            var boneNode = boneArray[boneIndice];
                            aiBone.Name = boneNode.Name;

                            Matrix4x4.Invert(transform, out Matrix4x4 transformInverse);
                            aiBone.OffsetMatrix = transformInverse.ToAssimpMatrix4x4();
                            boneMap.Add(boneIndice, aiBone);
                        }

                        // Add this vertex weight to it's bone
                        boneMap[boneIndice].VertexWeights.Add(new VertexWeight(vertexIndex, 1f));
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

                newMesh.Bones.AddRange(boneMap.Values);

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

            return scene;
        }

        /// <summary>
        /// Converts a FLVER2 into an Assimp Scene.
        /// </summary>
        public static Scene ToAssimpScene(FLVER2 model)
        {
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
                bool hasBones = mesh.BoneIndices.Count != 0;

                // Prepare a bone map
                var boneMap = new Dictionary<int, Bone>(mesh.BoneIndices.Count);

                // Add vertices
                for (int vertexIndex = 0; vertexIndex < mesh.Vertices.Count; vertexIndex++)
                {
                    var vertex = mesh.Vertices[vertexIndex];

                    // If the mesh has bones set the weights of this vertex to the correct bone
                    if (hasBones)
                    {
                        // Get the local bone indice from NormalW then the bone array bone indice from the mesh
                        var boneIndice = mesh.BoneIndices[vertex.NormalW];
                        var bone = model.Bones[boneIndice];
                        var transform = bone.ComputeTransform(model.Bones);

                        newMesh.Vertices.Add(vertex.Position.ToAssimpVector3D(transform));
                        newMesh.Normals.Add(vertex.Normal.ToAssimpVector3D());
                        newMesh.BiTangents.Add(vertex.Bitangent.ToAssimpVector3D());
                        foreach (var tangent in vertex.Tangents)
                        {
                            newMesh.Tangents.Add(tangent.ToAssimpVector3D());
                        }

                        // If the bone map does not already have the bone add it
                        if (!boneMap.ContainsKey(boneIndice))
                        {
                            var aiBone = new Bone();
                            var boneNode = boneArray[boneIndice];
                            aiBone.Name = boneNode.Name;

                            Matrix4x4.Invert(transform, out Matrix4x4 transformInverse);
                            aiBone.OffsetMatrix = transformInverse.ToAssimpMatrix4x4();
                            boneMap.Add(boneIndice, aiBone);
                        }

                        // Add this vertex weight to it's bone
                        boneMap[boneIndice].VertexWeights.Add(new VertexWeight(vertexIndex, 1f));
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

                newMesh.Bones.AddRange(boneMap.Values);

                // Add faces
                foreach (var faceset in mesh.FaceSets)
                {
                    var indices = faceset.Triangulate(mesh.Vertices.Count < ushort.MaxValue);
                    for (int i = 0; i < indices.Count - 2; i += 3)
                    {
                        newMesh.Faces.Add(new Face(new int[] { indices[i], indices[i + 1], indices[i + 2] }));
                    }
                }

                newMesh.MaterialIndex = mesh.MaterialIndex;
                scene.Meshes.Add(newMesh);

                var meshNode = new Node($"Mesh_{meshIndex}", scene.RootNode);
                meshNode.Transform = Assimp.Matrix4x4.Identity;

                meshNode.MeshIndices.Add(meshIndex);
                scene.RootNode.Children.Add(meshNode);
            }

            return scene;
        }

        /// <summary>
        /// Converts an MDL4 into an Assimp Scene.
        /// </summary>
        public static Scene ToAssimpScene(MDL4 model)
        {
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

                // Prepare a bone map
                var boneMap = new Dictionary<int, Bone>(mesh.BoneIndices.Length);

                // Add vertices
                for (int vertexIndex = 0; vertexIndex < mesh.Vertices.Count; vertexIndex++)
                {
                    var vertex = mesh.Vertices[vertexIndex];

                    // If the mesh has bones set the weights of this vertex to the correct bone
                    if (hasBones)
                    {
                        // Get the local bone indice from NormalW then the bone array bone indice from the mesh
                        var boneIndice = mesh.BoneIndices[(int)vertex.Normal.W];
                        var bone = model.Bones[boneIndice];
                        var transform = bone.ComputeTransform(model.Bones);

                        newMesh.Vertices.Add(vertex.Position.ToAssimpVector3D(transform));
                        newMesh.Normals.Add(vertex.Normal.ToAssimpVector3D());
                        newMesh.BiTangents.Add(vertex.Bitangent.ToAssimpVector3D());
                        newMesh.Tangents.Add(vertex.Tangent.ToAssimpVector3D());

                        // If the bone map does not already have the bone add it
                        if (!boneMap.ContainsKey(boneIndice))
                        {
                            var aiBone = new Bone();
                            var boneNode = boneArray[boneIndice];
                            aiBone.Name = boneNode.Name;

                            Matrix4x4.Invert(transform, out Matrix4x4 transformInverse);

                            aiBone.OffsetMatrix = transformInverse.ToAssimpMatrix4x4();
                            boneMap.Add(boneIndice, aiBone);
                        }

                        // Add this vertex weight to it's bone
                        boneMap[boneIndice].VertexWeights.Add(new VertexWeight(vertexIndex, 1f));
                    }
                    else
                    {
                        newMesh.Vertices.Add(vertex.Position.ToAssimpVector3D());
                        newMesh.Normals.Add(vertex.Normal.ToAssimpVector3D());
                        newMesh.BiTangents.Add(vertex.Bitangent.ToAssimpVector3D());
                        newMesh.Tangents.Add(vertex.Tangent.ToAssimpVector3D());
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
                    var color = vertex.Color;
                    newMesh.VertexColorChannels[0].Add(new Color4D(color.R, color.G, color.B, color.A));
                }

                newMesh.Bones.AddRange(boneMap.Values);

                // Add faces
                var faceVertexIndices = mesh.GetFaceVertexIndices();
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

            return scene;
        }

        /// <summary>
        /// Converts an SMD4 into an Assimp Scene.
        /// </summary>
        public static Scene ToAssimpScene(SMD4 model)
        {
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
            Material newMaterial = new()
            {
                Name = "default"
            };
            scene.Materials.Add(newMaterial);

            // Add meshes
            for (int meshIndex = 0; meshIndex < model.Meshes.Count; meshIndex++)
            {
                var mesh = model.Meshes[meshIndex];
                var newMesh = new Mesh("Mesh_M" + meshIndex, PrimitiveType.Triangle);
                bool hasBones = mesh.BoneIndices.Length != 0;

                // Prepare a bone map
                var boneMap = new Dictionary<int, Bone>(mesh.BoneIndices.Length);

                // Add vertices
                for (int vertexIndex = 0; vertexIndex < mesh.Vertices.Count; vertexIndex++)
                {
                    var vertex = mesh.Vertices[vertexIndex];

                    // If the mesh has bones set the weights of this vertex to the correct bone
                    if (hasBones)
                    {
                        // Get the local bone indice from BoneIndex then the bone array bone indice from the mesh
                        var boneIndice = mesh.BoneIndices[vertex.BoneIndex];
                        var bone = model.Bones[boneIndice];
                        var transform = bone.ComputeTransform(model.Bones);

                        newMesh.Vertices.Add(vertex.Position.ToAssimpVector3D(transform));

                        // If the bone map does not already have the bone add it
                        if (!boneMap.ContainsKey(boneIndice))
                        {
                            var aiBone = new Bone();
                            var boneNode = boneArray[boneIndice];
                            aiBone.Name = boneNode.Name;

                            Matrix4x4.Invert(transform, out Matrix4x4 transformInverse);
                            aiBone.OffsetMatrix = transformInverse.ToAssimpMatrix4x4();
                            boneMap.Add(boneIndice, aiBone);
                        }

                        // Add this vertex weight to it's bone
                        boneMap[boneIndice].VertexWeights.Add(new VertexWeight(vertexIndex, 1f));
                    }
                    else
                    {
                    
                        newMesh.Vertices.Add(vertex.Position.ToAssimpVector3D());
                    }
                }

                newMesh.Bones.AddRange(boneMap.Values);

                // Add faces
                var faceVertexIndices = mesh.GetFaces();
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

            return scene;
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
