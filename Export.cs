using Assimp;
using Assimp.Configs;
using SoulsFormats;
using SoulsFormats.Other;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Numerics;

namespace SFModelConverter
{
    /// <summary>
    /// Class for exporting SoulsFormats models using Assimp, uses an OOP approach
    /// </summary>
    internal static class Export
    {
        /// <summary>
        /// Adds materials from a FromSoftware model to an assimp scene
        /// </summary>
        /// <param name="model">A FromSoftware model</param>
        /// <param name="scene">An assimp scene</param>
        public static void AddMaterials(FLVER0 model, Scene scene)
        {
            // Add materials
            foreach (FLVER0.Material material in model.Materials)
            {
                Material newMaterial = new()
                {
                    Name = material.Name,
                };

                scene.Materials.Add(newMaterial);
            }
        }

        /// <summary>
        /// Computes the transform a vertex should have from its bone and that bone's parent bones
        /// </summary>
        /// <param name="model">A FromSoftware model</param>
        /// <param name="mesh">A mesh from the FromSoftware model</param>
        /// <param name="vertex">A vertex from the mesh of the FromSoftware model</param>
        /// <returns>A transform for vertex from its bone and that bone's parent bones</returns>
        public static System.Numerics.Matrix4x4 ComputeTransform(FLVER0 model, FLVER0.Mesh mesh, FLVER.Vertex vertex)
        {
            FLVER.Bone bone = model.Bones[mesh.BoneIndices[vertex.NormalW]];
            System.Numerics.Matrix4x4 transform = bone.ComputeLocalTransform();
            while (bone.ParentIndex != 1)
            {
                bone = model.Bones[bone.ParentIndex];
                transform *= bone.ComputeLocalTransform();
            }

            return transform;
        }

        /// <summary>
        /// Adds all the UVs in a FromSoftware model vertex to the texture coordinate channels of an assimp mesh
        /// </summary>
        /// <param name="UVs">A list of UVs from a FromSoftware model vertex</param>
        /// <param name="textureCoordChannels">Texture coordinate channels from an assimp mesh</param>
        public static void AddVertexUVs(List<Vector3> UVs, List<Vector3D>[] textureCoordChannels)
        {
            for (int i = 0; i < UVs.Count; ++i)
            {
                Vector3 UV = UVs[i];
                textureCoordChannels[i].Add(new Vector3D(-UV.X, 1 - UV.Y, 0));
            }
        }

        /// <summary>
        /// Transforms each, then dds all the UVs in a FromSoftware model vertex to the texture coordinate channels of an assimp mesh
        /// </summary>
        /// <param name="UVs">A list of UVs from a FromSoftware model vertex</param>
        /// <param name="textureCoordChannels">Texture coordinate channels from an assimp mesh</param>
        public static void AddTransformedVertexUVs(List<Vector3> UVs, List<Vector3D>[] textureCoordChannels, System.Numerics.Matrix4x4 transform)
        {
            for (int i = 0; i < UVs.Count; ++i)
            {
                Vector3 UV = Vector3.Transform(UVs[i], transform);
                textureCoordChannels[i].Add(new Vector3D(-UV.X, 1 - UV.Y, 0));
            }
        }

        /// <summary>
        /// Adds all the colors in a FromSoftware model vertex to the vertex color channels of an assimp mesh
        /// </summary>
        /// <param name="colors">A list of colors from a FromSoftware model vertex</param>
        /// <param name="vertexColorChannels">Vertex color channels from an assimp mesh</param>
        public static void AddVertexColors(dynamic colors, List<Color4D>[] vertexColorChannels)
        {
            for (int i = 0; i < colors.Count; ++i)
            {
                var color = colors[i];
                vertexColorChannels[i].Add(new(color.R, color.G, color.B, color.A));
            }
        }

        /// <summary>
        /// Adds the vertices from a FromSoftware mesh to an assimp mesh
        /// </summary>
        /// <param name="model">A FromSoftware model</param>
        /// <param name="mesh">A mesh from the FromSoftware model</param>
        /// <param name="newMesh">An assimp mesh</param>
        public static void AddVerticesNonDynamic(dynamic model, dynamic mesh, Mesh newMesh)
        {
            foreach (var vertex in mesh.Vertices)
            {
                if (vertex.NormalW != -1)
                {
                    var transform = Vector.ComputeTransformNonDynamic(model, mesh, vertex);
                    newMesh.Vertices.Add(Vector.Vector3TransformedToVector3D(vertex.Position, transform));
                    newMesh.Normals.Add(Vector.Vector3TransformedToVector3D(vertex.Normal, transform));
                    newMesh.BiTangents.Add(Vector.Vector4TransformedToVector3D(vertex.Bitangent, transform));
                    if (vertex.Tangents.Count > 0) newMesh.Tangents.Add(Vector.Vector4TransformedToVector3D(vertex.Tangents[0], transform));
                    AddTransformedVertexUVs(vertex.UVs, newMesh.TextureCoordinateChannels, transform);
                }
                else
                {
                    newMesh.Vertices.Add(Vector.Vector3ToVector3D(vertex.Position));
                    newMesh.Normals.Add(Vector.Vector3ToVector3D(vertex.Normal));
                    newMesh.BiTangents.Add(Vector.Vector4ToVector3D(vertex.Bitangent));
                    if (vertex.Tangents.Count > 0) newMesh.BiTangents.Add(Vector.Vector4ToVector3D(vertex.Tangents[0]));
                    AddVertexUVs(vertex.UVs, newMesh.TextureCoordinateChannels);
                }

                AddVertexColors(vertex.Colors, newMesh.VertexColorChannels);
            }
        }

        public static void AddVerticesDynamic(FLVER0 model, FLVER0.Mesh mesh, Mesh newMesh)
        {
            throw new NotImplementedException();
            foreach (FLVER.Vertex vertex in mesh.Vertices)
            {
                
            }
        }

        public static void AddVerticesCheckDynamic(FLVER0 model, FLVER0.Mesh mesh, Mesh newMesh, byte dynamic)
        {
            if (dynamic != 1) AddVerticesNonDynamic(model, mesh, newMesh);
            else AddVerticesDynamic(model, mesh, newMesh);
        }

        /// <summary>
        /// Adds faces from a FromSoftware FLVER0 model mesh to an assimp mesh
        /// </summary>
        /// <param name="mesh">A mesh from the Fromsoftware FLVER0 model</param>
        /// <param name="newMesh">An assimp mesh</param>
        /// <param name="version">The version of a FromSoftware FLVER0 model</param>
        public static void AddFaces(FLVER0.Mesh mesh, Mesh newMesh, int version)
        {
            foreach (int[] indices in mesh.GetFacesIndices(version))
            {
                newMesh.Faces.Add(new Face(indices));
            }
        }

        /// <summary>
        /// Adds faces from a FromSoftware MDL4 model mesh to an assimp mesh
        /// </summary>
        /// <param name="mesh">A mesh from the Fromsoftware MDL4 model</param>
        /// <param name="newMesh">An assimp mesh</param>
        public static void AddFaces(MDL4.Mesh mesh, Mesh newMesh)
        {
            foreach (int[] indices in mesh.GetFacesIndices())
            {
                newMesh.Faces.Add(new Face(indices));
            }
        }

        /// <summary>
        /// Adds faces from a FromSoftware FLVER2 model mesh to an assimp mesh
        /// </summary>
        /// <param name="mesh">A mesh from the Fromsoftware FLVER2 model</param>
        /// <param name="newMesh">An assimp mesh</param>
        public static void AddFaces(FLVER2.Mesh mesh, Mesh newMesh)
        {
            foreach (FLVER2.FaceSet faceset in mesh.FaceSets)
            {
                newMesh.Faces.Add(new Face(faceset.Indices.ToArray()));
            }
        }

        /// <summary>
        /// Adds meshes from an imported FromSoftware model to an assimp scene
        /// </summary>
        /// <param name="model">A FromSoftware model</param>
        /// <param name="scene">An assimp scene</param>
        public static void AddMeshes(dynamic model, Scene scene)
        {
            int meshCounter = 0;
            foreach (var mesh in model.Meshes)
            {
                Mesh newMesh = new("Mesh_M" + meshCounter, PrimitiveType.Triangle);

                // Add vertices
                if (model is MDL4) AddVerticesCheckDynamic(model, mesh, newMesh, mesh.VertexFormat);
                else AddVerticesCheckDynamic(model, mesh, newMesh, mesh.Dynamic);

                // Add faces
                if (model is FLVER0) AddFaces(mesh, newMesh, model.Header.Version);
                else AddFaces(mesh, newMesh);

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
        }

        /// <summary>
        /// Determines what FromSoftware model type imported FromSoftware model is then reads it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static dynamic ReadModel(string path)
        {
            if(FLVER0.Is(path)) return FLVER0.Read(path);
            if(FLVER2.Is(path)) return FLVER2.Read(path);
            if(MDL4.Is(path)) return MDL4.Read(path);
            return null;
        }

        /// <summary>
        /// Exports a FromSoftware model to the chosen model type using assimp
        /// </summary>
        /// <param name="path">A string containing the path to a Fromsoftware model</param>
        /// <param name="type">A string containing the type to export</param>
        /// <returns></returns>
        public static bool ExportModel(string path, string type, bool[][] flips, bool[][] swaps)
        {
            // Make save path
            string saveExtension = type switch
            {
                "fbx" => "fbx",
                "fbxa" => "fbx",
                "collada" => "dae",
                "obj" => "obj",
                _ => type
            };
            string savePath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.{saveExtension}";

            // Read model and make scene
            var model = ReadModel(path);
            if (model == null) return false;
            Scene scene = new()
            {
                RootNode = new()
            };

            // Determine flips and swaps
            SoulsFormatsFlip.DetermineFlip(model, flips[0], flips[1], flips[2], flips[3]);
            SoulsFormatsSwap.DetermineSwap(model, swaps[0], swaps[1], swaps[2], swaps[3]);

            // Export model to scene
            AddMaterials(model, scene);
            AddMeshes(model, scene);


            // Export scene to save path
            AssimpContext exporter = new();
            bool success = exporter.ExportFile(scene, savePath, type);
            return success;
        }
    }
}
