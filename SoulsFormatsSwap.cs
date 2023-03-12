using SoulsFormats.Other;

namespace SFModelConverter
{
    /// <summary>
    /// Class for swapping axes on SoulsFormatsModels
    /// </summary>
    internal static class SoulsFormatsSwap
    {
        /// <summary>
        /// Determines what axes should be swapped in a FromSoftware model
        /// </summary>
        /// <param name="model">A FromSoftware model</param>
        /// <param name="vertexSwaps">An array of bool, each determining whether or not vertex XZ, XY, or YZ should be swapped</param>
        /// <param name="normalSwaps">An array of bool, each determining whether or not normal XZ, XY, or YZ should be swapped</param>
        /// <param name="tangentSwaps">An array of bool, each determining whether or not tangent XZ, XY, XW, YZ, YW, or ZW should be swapped</param>
        /// <param name="bitangentSwaps">An array of bool, each determining whether or not bitangent XZ, XY, XW, YZ, YW, or ZW should be swapped</param>
        public static void DetermineSwap(dynamic model, bool[] vertexSwaps, bool[] normalSwaps, bool[] tangentSwaps, bool[] bitangentSwaps)
        {
            for (var i = 0; i < model.Meshes.Count; ++i)
            {
                var mesh = model.Meshes[i];
                DetermineVertexSwap(mesh, vertexSwaps);
                DetermineNormalSwap(mesh, normalSwaps);
                DetermineTangentSwap(mesh, tangentSwaps);
                DetermineBiTangentSwap(mesh, bitangentSwaps);
                model.Meshes[i] = mesh;
            }
        }

        /// <summary>
        /// Determines what vertex axes should be swapped in a FromSoftware model
        /// </summary>
        /// <param name="mesh">A FromSoftware model mesh</param>
        /// <param name="vertexSwaps">An array of bool, each determining whether or not vertex XZ, XY, or YZ should be swapped</param>
        public static void DetermineVertexSwap(dynamic mesh, bool[] vertexSwaps)
        {
            if (!vertexSwaps[0] && !vertexSwaps[1] && !vertexSwaps[2]) return;
            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                if (vertexSwaps[0]) // If swap XZ
                {
                    var positionX = mesh.Vertices[i].Position.X;
                    var positionZ = mesh.Vertices[i].Position.Z;
                    mesh.Vertices[i].Position.X = positionZ;
                    mesh.Vertices[i].Position.Z = positionX;
                }
                if (vertexSwaps[1]) // If swap XY
                {
                    var positionX = mesh.Vertices[i].Position.X;
                    var positionY = mesh.Vertices[i].Position.Y;
                    mesh.Vertices[i].Position.X = positionY;
                    mesh.Vertices[i].Position.Y = positionX;
                }
                if (vertexSwaps[2]) // If swap YZ
                {
                    var positionY = mesh.Vertices[i].Position.Y;
                    var positionZ = mesh.Vertices[i].Position.Z;
                    mesh.Vertices[i].Position.Y = positionZ;
                    mesh.Vertices[i].Position.Z = positionY;
                }
            }
        }

        /// <summary>
        /// Determines what normal axes should be swapped in a FromSoftware model
        /// </summary>
        /// <param name="mesh">A FromSoftware model mesh</param>
        /// <param name="normalSwaps">An array of bool, each determining whether or not normal XZ, XY, or YZ should be swapped</param>
        public static void DetermineNormalSwap(dynamic mesh, bool[] normalSwaps)
        {
            if (!normalSwaps[0] && !normalSwaps[1] && !normalSwaps[2]) return;
            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                if (normalSwaps[0]) // If swap XZ
                {
                    var normalX = mesh.Vertices[i].Normal.X;
                    var normalZ = mesh.Vertices[i].Normal.Z;
                    mesh.Vertices[i].Normal.X = normalZ;
                    mesh.Vertices[i].Normal.Z = normalX;
                }
                if (normalSwaps[1]) // If swap XY
                {
                    var normalX = mesh.Vertices[i].Normal.X;
                    var normalY = mesh.Vertices[i].Normal.Y;
                    mesh.Vertices[i].Normal.X = normalY;
                    mesh.Vertices[i].Normal.Y = normalX;
                }
                if (normalSwaps[2]) // If swap YZ
                {
                    var normalY = mesh.Vertices[i].Normal.Y;
                    var normalZ = mesh.Vertices[i].Normal.Z;
                    mesh.Vertices[i].Normal.Y = normalZ;
                    mesh.Vertices[i].Normal.Z = normalY;
                }
            }
        }

        /// <summary>
        /// Determines what tangent axes should be swapped in a FromSoftware model
        /// </summary>
        /// <param name="mesh">A FromSoftware model mesh</param>
        /// <param name="tangentSwaps">An array of bool, each determining whether or not tangent XZ, XY, XW, YZ, YW, or ZW should be swapped</param>
        public static void DetermineTangentSwap(dynamic mesh, bool[] tangentSwaps)
        {
            if (!tangentSwaps[0] && !tangentSwaps[1] && !tangentSwaps[2] && !tangentSwaps[3] && !tangentSwaps[4] && !tangentSwaps[5]) return;
            
            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                for (var j = 0; j < mesh.Vertices[i].Tangents.Count; ++j)
                {
                    var tangent = mesh.Vertices[i].Tangents[j];
                    if (tangentSwaps[0]) // If swap XZ
                    {
                        var tangentX = mesh.Vertices[i].Tangents[j].X;
                        var tangentZ = mesh.Vertices[i].Tangents[j].Z;
                        tangent.X = tangentZ;
                        tangent.Z = tangentX;
                        mesh.Vertices[i].Tangents[j] = tangent;
                    }
                    if (tangentSwaps[1]) // If swap XY
                    {
                        var tangentX = mesh.Vertices[i].Tangents[j].X;
                        var tangentY = mesh.Vertices[i].Tangents[j].Y;
                        tangent.X = tangentY;
                        tangent.Y = tangentX;
                        mesh.Vertices[i].Tangents[j] = tangent;
                    }
                    if (tangentSwaps[2]) // If swap XW
                    {
                        var tangentX = mesh.Vertices[i].Tangents[j].X;
                        var tangentW = mesh.Vertices[i].Tangents[j].W;
                        tangent.X = tangentW;
                        tangent.W = tangentX;
                        mesh.Vertices[i].Tangents[j] = tangent;
                    }
                    if (tangentSwaps[3]) // If swap YZ
                    {
                        var tangentY = mesh.Vertices[i].Tangents[j].Y;
                        var tangentZ = mesh.Vertices[i].Tangents[j].Z;
                        tangent.Y = tangentZ;
                        tangent.Z = tangentY;
                        mesh.Vertices[i].Tangents[j] = tangent;
                    }
                    if (tangentSwaps[4]) // If swap YW
                    {
                        var tangentY = mesh.Vertices[i].Tangents[j].Y;
                        var tangentW = mesh.Vertices[i].Tangents[j].W;
                        tangent.Y = tangentW;
                        tangent.W = tangentY;
                        mesh.Vertices[i].Tangents[j] = tangent;
                    }
                    if (tangentSwaps[5]) // If swap ZW
                    {
                        var tangentZ = mesh.Vertices[i].Tangents[j].Z;
                        var tangentW = mesh.Vertices[i].Tangents[j].W;
                        tangent.Z = tangentW;
                        tangent.W = tangentZ;
                        mesh.Vertices[i].Tangents[j] = tangent;
                    }
                }
            }
        }

        /// <summary>
        /// Determines what tangent axes should be swapped in a MDL4 model
        /// </summary>
        /// <param name="mesh">A MDL4 model mesh</param>
        /// <param name="tangentSwaps">An array of bool, each determining whether or not tangent XZ, XY, XW, YZ, YW, or ZW should be swapped</param>
        public static void DetermineTangentSwap(MDL4.Mesh mesh, bool[] tangentSwaps)
        {
            if (!tangentSwaps[0] && !tangentSwaps[1] && !tangentSwaps[2] && !tangentSwaps[3] && !tangentSwaps[4] && !tangentSwaps[5]) return;

            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                if (tangentSwaps[0]) // If swap XZ
                {
                    var tangentX = mesh.Vertices[i].Tangent.X;
                    var tangentZ = mesh.Vertices[i].Tangent.Z;
                    mesh.Vertices[i].Tangent.X = tangentZ;
                    mesh.Vertices[i].Tangent.Z = tangentX;
                }
                if (tangentSwaps[1]) // If swap XY
                {
                    var tangentX = mesh.Vertices[i].Tangent.X;
                    var tangentY = mesh.Vertices[i].Tangent.Y;
                    mesh.Vertices[i].Tangent.X = tangentY;
                    mesh.Vertices[i].Tangent.Y = tangentX;
                }
                if (tangentSwaps[2]) // If swap XW
                {
                    var tangentX = mesh.Vertices[i].Tangent.X;
                    var tangentW = mesh.Vertices[i].Tangent.W;
                    mesh.Vertices[i].Tangent.X = tangentW;
                    mesh.Vertices[i].Tangent.W = tangentX;
                }
                if (tangentSwaps[3]) // If swap YZ
                {
                    var tangentY = mesh.Vertices[i].Tangent.Y;
                    var tangentZ = mesh.Vertices[i].Tangent.Z;
                    mesh.Vertices[i].Tangent.Y = tangentZ;
                    mesh.Vertices[i].Tangent.Z = tangentY;
                }
                if (tangentSwaps[4]) // If swap YW
                {
                    var tangentY = mesh.Vertices[i].Tangent.Y;
                    var tangentW = mesh.Vertices[i].Tangent.W;
                    mesh.Vertices[i].Tangent.Y = tangentW;
                    mesh.Vertices[i].Tangent.W = tangentY;
                }
                if (tangentSwaps[5]) // If swap ZW
                {
                    var tangentZ = mesh.Vertices[i].Tangent.Z;
                    var tangentW = mesh.Vertices[i].Tangent.W;
                    mesh.Vertices[i].Tangent.Z = tangentW;
                    mesh.Vertices[i].Tangent.W = tangentZ;
                }
            }
        }

        /// <summary>
        /// Determines what bitangent axes should be swapped in a FromSoftware model
        /// </summary>
        /// <param name="mesh">A FromSoftware model mesh</param>
        /// <param name="bitangentSwaps">An array of bool, each determining whether or not bitangent XZ, XY, XW, YZ, YW, or ZW should be swapped</param>
        public static void DetermineBiTangentSwap(dynamic mesh, bool[] bitangentSwaps)
        {
            if (!bitangentSwaps[0] && !bitangentSwaps[1] && !bitangentSwaps[2] && !bitangentSwaps[3] && !bitangentSwaps[4] && !bitangentSwaps[5]) return;

            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                if (bitangentSwaps[0]) // If swap XZ
                {
                    var tangentX = mesh.Vertices[i].Bitangent.X;
                    var tangentZ = mesh.Vertices[i].Bitangent.Z;
                    mesh.Vertices[i].Bitangent.X = tangentZ;
                    mesh.Vertices[i].Bitangent.Z = tangentX;
                }
                if (bitangentSwaps[1]) // If swap XY
                {
                    var tangentX = mesh.Vertices[i].Bitangent.X;
                    var tangentY = mesh.Vertices[i].Bitangent.Y;
                    mesh.Vertices[i].Bitangent.X = tangentY;
                    mesh.Vertices[i].Bitangent.Y = tangentX;
                }
                if (bitangentSwaps[2]) // If swap XW
                {
                    var tangentX = mesh.Vertices[i].Bitangent.X;
                    var tangentW = mesh.Vertices[i].Bitangent.W;
                    mesh.Vertices[i].Bitangent.X = tangentW;
                    mesh.Vertices[i].Bitangent.W = tangentX;
                }
                if (bitangentSwaps[3]) // If swap YZ
                {
                    var tangentY = mesh.Vertices[i].Bitangent.Y;
                    var tangentZ = mesh.Vertices[i].Bitangent.Z;
                    mesh.Vertices[i].Bitangent.Y = tangentZ;
                    mesh.Vertices[i].Bitangent.Z = tangentY;
                }
                if (bitangentSwaps[4]) // If swap YW
                {
                    var tangentY = mesh.Vertices[i].Bitangent.Y;
                    var tangentW = mesh.Vertices[i].Bitangent.W;
                    mesh.Vertices[i].Bitangent.Y = tangentW;
                    mesh.Vertices[i].Bitangent.W = tangentY;
                }
                if (bitangentSwaps[5]) // If swap ZW
                {
                    var tangentZ = mesh.Vertices[i].Bitangent.Z;
                    var tangentW = mesh.Vertices[i].Bitangent.W;
                    mesh.Vertices[i].Bitangent.Z = tangentW;
                    mesh.Vertices[i].Bitangent.W = tangentZ;
                }
            }
        }
    }
}
