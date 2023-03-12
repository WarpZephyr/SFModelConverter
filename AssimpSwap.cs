using Assimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFModelConverter
{
    /// <summary>
    /// Class for swapping axes on Assimp scenes
    /// </summary>
    internal static class AssimpSwap
    {
        /// <summary>
        /// Determines what axes should be swapped in an Assimp scene
        /// </summary>
        /// <param name="scene">An assimp scene to swap</param>
        /// <param name="vertexSwaps">An array of bool, each determining whether or not vertex XZ, XY, or YZ should be swapped</param>
        /// <param name="normalSwaps">An array of bool, each determining whether or not normal XZ, XY, or YZ should be swapped</param>
        /// <param name="tangentSwaps">An array of bool, each determining whether or not tangent XZ, XY, XW, YZ, YW, or ZW should be swapped</param>
        /// <param name="bitangentSwaps">An array of bool, each determining whether or not bitangent XZ, XY, XW, YZ, YW, or ZW should be swapped</param>
        public static void DetermineSwap(Scene scene, bool[] vertexSwaps, bool[] normalSwaps, bool[] tangentSwaps, bool[] bitangentSwaps)
        {
            throw new NotImplementedException();
        }
    }
}
