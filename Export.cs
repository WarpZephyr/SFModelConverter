using System.IO;
using System.Numerics;
using System.Collections.Generic;
using SoulsFormats;
using Assimp;
using System;
using Newtonsoft.Json.Bson;
using System.Linq;

namespace SFModelConverter
{
    /// <summary>
    /// Class for exporting SoulsFormats models using Assimp.
    /// </summary>
    internal static class Export
    {
        /// <summary>
        /// Adds materials from a FLVER0 to an assimp scene.
        /// </summary>
        /// <param name="model">A FLVER0.</param>
        /// <param name="scene">An assimp scene.</param>
        public static void AddMaterials(FLVER0 model, Scene scene)
        {
            foreach (var material in model.Materials)
            {
                Material newMaterial = new()
                {
                    Name = material.Name,
                };

                scene.Materials.Add(newMaterial);
            }
        }

        /// <summary>
        /// Add bones from a FLVER0 to an assimp scene.
        /// </summary>
        /// <param name="model">A FLVER0.</param>
        /// <param name="rootNode">The root node of the scene.</param>
        public static void AddBones(FLVER0 model, Node rootNode)
        {
            Node[] boneArray = new Node[model.Bones.Count];
            //Assign bones
            for (int i = 0; i < model.Bones.Count; i++)
            {
                var bone = model.Bones[i];
                Node parentNode;
                if (bone.ParentIndex == -1)
                    parentNode = rootNode;
                else
                    parentNode = boneArray[bone.ParentIndex];
                var aiNode = new Node($"Bone_{i}_{bone.Name}", parentNode);

                //Get local transform
                aiNode.Transform = bone.ComputeTransform(model.Bones).ToAssimpMatrix4x4();

                parentNode.Children.Add(aiNode);
                boneArray[i] = aiNode;
            }
        }

        /// <summary>
        /// Adds the vertices from a FLVER0 mesh to an assimp mesh.
        /// </summary>
        /// <param name="model">A FLVER0.</param>
        /// <param name="mesh">A mesh from the provided FLVER0.</param>
        /// <param name="newMesh">An assimp mesh.</param>
        public static void AddVertices(FLVER0 model, FLVER0.Mesh mesh, Mesh newMesh, Dictionary<int, Bone> boneMap)
        {
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var vertex = mesh.Vertices[i];
                if (mesh.BoneIndices.Length != 0)
                {
                    var transform = ModelUtil.ComputeTransformNonDynamic(model, mesh, vertex);
                    newMesh.Vertices.Add(ModelUtil.Vector3TransformedToVector3D(vertex.Position, transform));
                    newMesh.Normals.Add(ModelUtil.Vector3TransformedToVector3D(vertex.Normal, transform));
                    newMesh.BiTangents.Add(ModelUtil.Vector4TransformedToVector3D(vertex.Bitangent, transform));
                    foreach (var tangent in vertex.Tangents)
                        newMesh.Tangents.Add(ModelUtil.Vector4TransformedToVector3D(tangent, transform));
                }
                else
                {
                    newMesh.Vertices.Add(ModelUtil.Vector3ToVector3D(vertex.Position));
                    newMesh.Normals.Add(ModelUtil.Vector3ToVector3D(vertex.Normal));
                    newMesh.BiTangents.Add(ModelUtil.Vector4ToVector3D(vertex.Bitangent));
                    foreach(var tangent in vertex.Tangents)
                        newMesh.Tangents.Add(ModelUtil.Vector4ToVector3D(tangent));
                }

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

                // Add UVs
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
                    newMesh.VertexColorChannels[0].Add(new(color.R, color.G, color.B, color.A));
                }
                if (vertex.Colors.Count > 1)
                {
                    var color = vertex.Colors[1];
                    newMesh.VertexColorChannels[1].Add(new(color.R, color.G, color.B, color.A));
                }

                // Add bone weights
                var boneIndex = vertex.NormalW;
                if (boneIndex == -1)
                    continue;

                var boneWeight = 1f;

                if (!boneMap.Keys.Contains(boneIndex))
                {
                    var newBone = new Bone();
                    var rawBone = model.Bones[mesh.BoneIndices[boneIndex]];

                    newBone.Name = $"({mesh.BoneIndices[boneIndex]})" + rawBone.Name;
                    newBone.VertexWeights.Add(new VertexWeight(i, boneWeight));

                    var invTransform = rawBone.ComputeTransform(model.Bones).ToAssimpMatrix4x4();

                    newBone.OffsetMatrix = invTransform;

                    boneMap[boneIndex] = newBone;
                }

                if (!boneMap[boneIndex].VertexWeights.Any(x => x.VertexID == i))
                    boneMap[boneIndex].VertexWeights.Add(new VertexWeight(i, boneWeight));
            }
        }

        /// <summary>
        /// Adds meshes from an imported FLVER0 to an assimp scene.
        /// </summary>
        /// <param name="model">A FLVER0.</param>
        /// <param name="scene">An assimp scene.</param>
        public static void AddMeshes(FLVER0 model, Scene scene)
        {
            int meshCounter = 0;
            foreach (var mesh in model.Meshes)
            {
                var boneMap = new Dictionary<int, Bone>();
                var newMesh = new Mesh("Mesh_M" + meshCounter, PrimitiveType.Triangle);

                // Add vertices
                AddVertices(model, mesh, newMesh, boneMap);

                //Add bones to mesh
                newMesh.Bones.AddRange(boneMap.Values);

                // Add faces
                foreach (int[] indices in mesh.GetFaceVertexIndices(model.Header.Version))
                {
                    newMesh.Faces.Add(new Face(indices));
                }

                newMesh.MaterialIndex = mesh.MaterialIndex;
                scene.Meshes.Add(newMesh);

                var node = new Node()
                {
                    Name = $"Mesh_{meshCounter}",
                };

                node.MeshIndices.Add(meshCounter);
                scene.RootNode.Children.Add(node);
                meshCounter++;
            }
        }

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
            var scene = new Scene()
            {
                RootNode = new Node()
            };

            // Export model to scene
            AddMaterials(model, scene);
            AddBones(model, scene.RootNode);
            AddMeshes(model, scene);

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
