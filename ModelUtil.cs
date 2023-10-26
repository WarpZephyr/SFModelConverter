using Assimp;
using SoulsFormats;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace SFModelConverter
{
    /// <summary>
    /// Class for converting vectors in different ways
    /// </summary>
    internal static class ModelUtil
    {
        /// <summary>
        /// Computes the transform a vertex should have from its bone and that bone's parent bones
        /// </summary>
        /// <param name="model">A non-dynamic FromSoftware model</param>
        /// <param name="mesh">A mesh from the non-dynamic FromSoftware model</param>
        /// <param name="vertex">A vertex from the mesh of the FromSoftware model</param>
        /// <returns>A transform for vertex from its bone and that bone's parent bones</returns>
        public static System.Numerics.Matrix4x4 ComputeTransformNonDynamic(dynamic model, dynamic mesh, dynamic vertex)
        {
            int boneIndiceIndex;
            if (model is MDL4)
                boneIndiceIndex = (int)vertex.Normal.W;
            else
                boneIndiceIndex = vertex.NormalW;

            var bone = model.Bones[mesh.BoneIndices[boneIndiceIndex]];
            System.Numerics.Matrix4x4 transform = bone.ComputeLocalTransform();
            while (bone.ParentIndex != -1)
            {
                bone = model.Bones[bone.ParentIndex];
                transform *= bone.ComputeLocalTransform();
            }

            return transform;
        }
    }
}
