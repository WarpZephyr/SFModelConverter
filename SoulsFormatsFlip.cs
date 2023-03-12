using SoulsFormats.Other;

namespace SFModelConverter
{
    /// <summary>
    /// Class for flipping axes on SoulsFormats models
    /// </summary>
    internal static class SoulsFormatsFlip
    {
        /// <summary>
        /// Determines what axes should be flipped in a FromSoftware model
        /// </summary>
        /// <param name="model">A FromSoftware model</param>
        /// <param name="vertexFlips">An array of bool, each determining whether or not vertex X, Y, or Z should be flipped</param>
        /// <param name="normalFlips">An array of bool, each determining whether or not normal X, Y, or Z should be flipped</param>
        /// <param name="tangentFlips">An array of bool, each determining whether or not tangent X, Y, Z, or W should be flipped</param>
        /// <param name="bitangentFlips">An array of bool, each determining whether or not bitangent X, Y, Z, or W should be flipped</param>
        public static void DetermineFlip(dynamic model, bool[] vertexFlips, bool[] normalFlips, bool[] tangentFlips, bool[] bitangentFlips)
        {
            for (var i = 0; i < model.Meshes.Count; ++i)
            {
                var mesh = model.Meshes[i];
                DetermineVertexFlip(mesh, vertexFlips);
                DetermineNormalFlip(mesh, normalFlips);
                DetermineTangentFlip(mesh, tangentFlips);
                DetermineBiTangentFlip(mesh, bitangentFlips);
                model.Meshes[i] = mesh;
            }
        }

        /// <summary>
        /// Determines if vertex should be flipped in a FromSoftware model
        /// </summary>
        /// <param name="mesh">A FromSoftware model mesh</param>
        /// <param name="vertexFlips">An array of bool, each determining whether or not vertex X, Y, or Z should be flipped</param>
        public static void DetermineVertexFlip(dynamic mesh, bool[] vertexFlips)
        {
            if (!vertexFlips[0] && !vertexFlips[1] && !vertexFlips[2]) return;
            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                if (vertexFlips[0] && vertexFlips[1] && vertexFlips[2]) mesh.Vertices[i].Position = -mesh.Vertices[i].Position;
                else
                {
                    if (vertexFlips[0]) mesh.Vertices[i].Position.X = -mesh.Vertices[i].Position.X;
                    if (vertexFlips[1]) mesh.Vertices[i].Position.Y = -mesh.Vertices[i].Position.Y;
                    if (vertexFlips[2]) mesh.Vertices[i].Position.Z = -mesh.Vertices[i].Position.Z;
                }
            }
        }

        /// <summary>
        /// Determines if normal should be flipped in a FromSoftware model
        /// </summary>
        /// <param name="mesh">A FromSoftware model mesh</param>
        /// <param name="normalFlips">An array of bool, each determining whether or not normal X, Y, or Z should be flipped</param>
        public static void DetermineNormalFlip(dynamic mesh, bool[] normalFlips)
        {
            if (!normalFlips[0] && !normalFlips[1] && !normalFlips[2]) return;
            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                if (normalFlips[0] && normalFlips[1] && normalFlips[2]) mesh.Vertices[i].Normal = -mesh.Vertices[i].Normal;
                else 
                {
                    if (normalFlips[0]) mesh.Vertices[i].Normal.X = -mesh.Vertices[i].Normal.X;
                    if (normalFlips[1]) mesh.Vertices[i].Normal.Y = -mesh.Vertices[i].Normal.Y;
                    if (normalFlips[2]) mesh.Vertices[i].Normal.Z = -mesh.Vertices[i].Normal.Z;
                }
            }
        }

        /// <summary>
        /// Determines if tangent should be flipped in a FromSoftware model
        /// </summary>
        /// <param name="mesh">A FromSoftware model mesh</param>
        /// <param name="tangentFlips">An array of bool, each determining whether or not tangent X, Y, Z, or W should be flipped</param>
        public static void DetermineTangentFlip(dynamic mesh, bool[] tangentFlips)
        {
            if (!tangentFlips[0] && !tangentFlips[1] && !tangentFlips[2] && !tangentFlips[3]) return;
            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                for (var j = 0; j < mesh.Vertices[i].Tangents.Count; ++j)
                {
                    if (tangentFlips[0] && tangentFlips[1] && tangentFlips[2] && tangentFlips[3]) mesh.Vertices[i].Tangents[0] = -mesh.Vertices[i].Tangents[j];
                    else
                    {
                        var tangent = mesh.Vertices[i].Tangents[j];
                        if (tangentFlips[0]) tangent.X = -tangent.X;
                        if (tangentFlips[1]) tangent.Y = -tangent.Y;
                        if (tangentFlips[2]) tangent.Z = -tangent.Z;
                        if (tangentFlips[3]) tangent.W = -tangent.W;
                        mesh.Vertices[i].Tangents[0] = tangent;
                    }
                } 
            }
        }

        /// <summary>
        /// Determines if tangent should be flipped in an MDL4 model
        /// </summary>
        /// <param name="mesh">A MDL4 model mesh</param>
        /// <param name="tangentFlips">An array of bool, each determining whether or not tangent X, Y, Z, or W should be flipped</param>
        public static void DetermineTangentFlip(MDL4.Mesh mesh, bool[] tangentFlips)
        {
            if (!tangentFlips[0] && !tangentFlips[1] && !tangentFlips[2] && !tangentFlips[3]) return;
            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                if (tangentFlips[0] && tangentFlips[1] && tangentFlips[2] && tangentFlips[3]) mesh.Vertices[i].Tangent = -mesh.Vertices[i].Tangent;
                else
                {
                    if (tangentFlips[0]) mesh.Vertices[i].Tangent.X = -mesh.Vertices[i].Tangent.X;
                    if (tangentFlips[1]) mesh.Vertices[i].Tangent.Y = -mesh.Vertices[i].Tangent.Y;
                    if (tangentFlips[2]) mesh.Vertices[i].Tangent.Z = -mesh.Vertices[i].Tangent.Z;
                    if (tangentFlips[3]) mesh.Vertices[i].Tangent.W = -mesh.Vertices[i].Tangent.W;
                }
            }
        }

        /// <summary>
        /// Determines if bitangent should be flipped in a FromSoftware model
        /// </summary>
        /// <param name="mesh">A FromSoftware model mesh</param>
        /// <param name="bitangentFlips">An array of bool, each determining whether or not bitangent X, Y, Z, or W should be flipped</param>
        public static void DetermineBiTangentFlip(dynamic mesh, bool[] bitangentFlips)
        {
            if (!bitangentFlips[0] && !bitangentFlips[1] && !bitangentFlips[2] && !bitangentFlips[3]) return;
            for (var i = 0; i < mesh.Vertices.Count; ++i)
            {
                if (bitangentFlips[0] && bitangentFlips[1] && bitangentFlips[2] && bitangentFlips[3]) mesh.Vertices[i].Bitangent = -mesh.Vertices[i].Bitangent;
                else
                {
                    if (bitangentFlips[0]) mesh.Vertices[i].Bitangent.X = -mesh.Vertices[i].Bitangent.X;
                    if (bitangentFlips[1]) mesh.Vertices[i].Bitangent.Y = -mesh.Vertices[i].Bitangent.Y;
                    if (bitangentFlips[2]) mesh.Vertices[i].Bitangent.Z = -mesh.Vertices[i].Bitangent.Z;
                    if (bitangentFlips[3]) mesh.Vertices[i].Bitangent.W = -mesh.Vertices[i].Bitangent.W;
                }
            }
        }
    }
}
