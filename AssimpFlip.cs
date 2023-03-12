using Assimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFModelConverter
{
    /// <summary>
    /// Class for flipping axes on Assimp scenes
    /// </summary>
    internal static class AssimpFlip
    {
        /// <summary>
        /// Determines what axes should be flipped in an Assimp scene
        /// </summary>
        /// <param name="scene">An assimp scene</param>
        /// <param name="vertexFlips">An array of bool, each determining whether or not vertex X, Y, or Z should be flipped</param>
        /// <param name="normalFlips">An array of bool, each determining whether or not normal X, Y, or Z should be flipped</param>
        /// <param name="tangentFlips">An array of bool, each determining whether or not tangent X, Y, Z, or W should be flipped</param>
        /// <param name="bitangentFlips">An array of bool, each determining whether or not bitangent X, Y, Z, or W should be flipped</param>
        public static void DetermineFlip(Scene scene, bool[] vertexFlips, bool[] normalFlips, bool[] tangentFlips, bool[] bitangentFlips)
        {
            throw new NotImplementedException();
        }
    }
}
