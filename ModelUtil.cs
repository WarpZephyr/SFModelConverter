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
        /// Converts a Vector3 to an assimp Vector3D
        /// </summary>
        /// <param name="vector">A Vector3</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector3ToVector3D(Vector3 vector)
        {
            return new Vector3D(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Normalizes then converts a Vector3 to an assimp Vector3D
        /// </summary>
        /// <param name="vector">A Vector3</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector3NormalizedToVector3D(Vector3 vector)
        {
            Vector3 normalizedVector = Vector3.Normalize(vector);
            return new Vector3D(normalizedVector.X, normalizedVector.Y, normalizedVector.Z);
        }

        /// <summary>
        /// Transforms then converts a Vector3 to an assimp Vector3D
        /// </summary>
        /// <param name="vector">A Vector3</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector3TransformedToVector3D(Vector3 vector, System.Numerics.Matrix4x4 transform)
        {
            Vector3 transformedVector = Vector3.Transform(vector, transform);
            return new Vector3D(transformedVector.X, transformedVector.Y, transformedVector.Z);
        }

        /// <summary>
        /// Transforms then converts a Vector3 Normal to an assimp Vector3D
        /// </summary>
        /// <param name="vector">A Vector3</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector3NormalTransformedToVector3D(Vector3 vector, System.Numerics.Matrix4x4 transform)
        {
            Vector3 transformedVector = Vector3.TransformNormal(vector, transform);
            return new Vector3D(transformedVector.X, transformedVector.Y, transformedVector.Z);
        }

        /// <summary>
        /// Normalizes, then transforms, and finally converts a Vector3 to an assimp Vector3D
        /// </summary>
        /// <param name="vector">A Vector3</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector3NormalizedTransformedToVector3D(Vector3 vector, System.Numerics.Matrix4x4 transform)
        {
            Vector3 normalizedVector = Vector3.Normalize(vector);
            Vector3 transformedVector = Vector3.TransformNormal(normalizedVector, transform);
            return new Vector3D(transformedVector.X, transformedVector.Y, transformedVector.Z);
        }

        /// <summary>
        /// Converts a Vector4 to assimp Vector3D, W is discarded
        /// </summary>
        /// <param name="vector">A Vector4</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector4ToVector3D(Vector4 vector)
        {
            return new Vector3D(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Normalizes then converts a Vector4 to assimp Vector3D, W is discarded
        /// </summary>
        /// <param name="vector">A Vector4</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector4NormalizedToVector3D(Vector4 vector)
        {
            Vector4 normalizedVector = Vector4.Normalize(vector);
            return new Vector3D(normalizedVector.X, normalizedVector.Y, normalizedVector.Z);
        }

        /// <summary>
        /// Transforms then converts a Vector4 to assimp Vector3D, W is discarded
        /// </summary>
        /// <param name="vector">A Vector4</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector4TransformedToVector3D(Vector4 vector, System.Numerics.Matrix4x4 transform)
        {
            Vector4 transformedVector = Vector4.Transform(vector, transform);
            return new Vector3D(transformedVector.X, transformedVector.Y, transformedVector.Z);
        }

        /// <summary>
        /// Normalizes, then transforms, and finally converts a Vector4 to assimp Vector3D, W is discarded
        /// </summary>
        /// <param name="vector">A Vector4</param>
        /// <returns>An assimp Vector3D</returns>
        public static Vector3D Vector4NormalizedTransformedToVector3D(Vector4 vector, System.Numerics.Matrix4x4 transform)
        {
            Vector4 normalizedVector = Vector4.Normalize(vector);
            Vector4 transformedVector = Vector4.Transform(normalizedVector, transform);
            return new Vector3D(transformedVector.X, transformedVector.Y, transformedVector.Z);
        }

        /// <summary>
        /// Adds Vector3's from a Vector3 list to the first element in each Vector3D list of a new array of Vector3D lists
        /// Can't be used to assign to assimp texture coordinate channels directly
        /// Iterate over array to add to assimp texture coordinate channels
        /// </summary>
        /// <param name="vector3s">A list of Vector3</param>
        /// <returns>An array of Vector3D lists</returns>
        public static List<Vector3D>[] Vector3ListToVector3DListArray(List<Vector3> vector3s)
        {
            List<Vector3D>[] vector3DLists = new List<Vector3D>[vector3s.Count];
            for (int i = 0; i < vector3s.Count; ++i)
            {
                Vector3 vector3 = vector3s[i];
                vector3DLists[i].Add(new Vector3D(vector3.X, 1 - vector3.Y, 0));
            }

            return vector3DLists;
        }

        /// <summary>
        /// Adds Vector3's from a Vector3 list to a new Vector3D list
        /// Can't be used to make assimp texture coordinate channel
        /// </summary>
        /// <param name="vector3s"></param>
        /// <returns>A list of Vector3D</returns>
        public static List<Vector3D> Vector3ListToVector3DList(List<Vector3> vector3s)
        {
            List<Vector3D> vector3DList = new();
            for (int i = 0; i < vector3s.Count; ++i)
            {
                Vector3 vector3 = vector3s[i];
                vector3DList.Add(new Vector3D(vector3.X, 1 - vector3.Y, 0));
            }

            return vector3DList;
        }

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

        public static System.Numerics.Matrix4x4 ComputeTransform(this FLVER.Bone bone, List<FLVER.Bone> bones)
        {
            var transform = bone.ComputeLocalTransform();
            while (bone.ParentIndex != -1)
            {
                if (!(bone.ParentIndex < 0 || bone.ParentIndex > bones.Count))
                {
                    bone = bones[bone.ParentIndex];
                    transform *= bone.ComputeLocalTransform();
                }
                else
                    throw new InvalidDataException("Bone has a parent index outside of the provided bone array.");
            }

            return transform;
        }

        /// <summary>
        /// Converts a System.Numerics.Matrix4x4 into an Assimp.Matrix4x4.
        /// </summary>
        /// <param name="matrix">A System.Numerics.Matrix4x4.</param>
        /// <returns>An Assimp.Matrix4x4.</returns>
        public static Assimp.Matrix4x4 ToAssimpMatrix4x4(this System.Numerics.Matrix4x4 matrix)
        {
            return new Assimp.Matrix4x4
            {
                A1 = matrix.M11,
                A2 = matrix.M12,
                A3 = matrix.M13,
                A4 = matrix.M14,
                B1 = matrix.M21,
                B2 = matrix.M22,
                B3 = matrix.M23,
                B4 = matrix.M24,
                C1 = matrix.M31,
                C2 = matrix.M32,
                C3 = matrix.M33,
                C4 = matrix.M34,
                D1 = matrix.M41,
                D2 = matrix.M42,
                D3 = matrix.M43,
                D4 = matrix.M44,
            };
        }
    }
}
