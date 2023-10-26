using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using SoulsFormats;

namespace SFModelConverter
{
    public static class Extensions
    {
        #region Vector

        public static Vector2 ToNumericsVector2(this Assimp.Vector2D vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector2 ToNumericsVector2(this Assimp.Vector2D vector, Assimp.Matrix4x4 transform)
        {
            var newVector = new Vector2(vector.X, vector.Y);
            return Vector2.Transform(newVector, transform.ToNumericsMatrix4x4());
        }

        public static Assimp.Vector2D ToAssimpVector2D(this Vector2 vector)
        {
            return new Assimp.Vector2D(vector.X, vector.Y);
        }

        public static Assimp.Vector2D ToAssimpVector2D(this Vector2 vector, Matrix4x4 transform)
        {
            vector = Vector2.Transform(vector, transform);
            return new Assimp.Vector2D(vector.X, vector.Y);
        }

        public static Assimp.Vector3D ToAssimpVector3D(this Vector3 vector)
        {
            return new Assimp.Vector3D(vector.X, vector.Y, vector.Z);
        }

        public static Assimp.Vector3D ToAssimpVector3D(this Vector3 vector, Matrix4x4 transform)
        {
            vector = Vector3.Transform(vector, transform);
            return new Assimp.Vector3D(vector.X, vector.Y, vector.Z);
        }

        public static Assimp.Vector3D ToAssimpVector3DNormal(this Vector3 vector, Matrix4x4 transform)
        {
            vector = Vector3.TransformNormal(vector, transform);
            return new Assimp.Vector3D(vector.X, vector.Y, vector.Z);
        }

        public static Assimp.Vector3D ToAssimpVector3D(this Vector4 vector)
        {
            return new Assimp.Vector3D(vector.X, vector.Y, vector.Z);
        }

        public static Assimp.Vector3D ToAssimpVector3D(this Vector4 vector, Matrix4x4 transform)
        {
            vector = Vector4.Transform(vector, transform);
            return new Assimp.Vector3D(vector.X, vector.Y, vector.Z);
        }

        public static Assimp.Vector3D ToAssimpVector3DNormal(this Vector4 vector, Matrix4x4 transform)
        {
            vector = Vector4.Transform(vector, transform);
            return new Assimp.Vector3D(vector.X, vector.Y, vector.Z);
        }

        #endregion

        #region Axis Vector

        public static Assimp.Vector3D FlipXY(this Assimp.Vector3D vector)
        {
            float x = vector.X;
            float y = vector.Y;
            vector.X = y;
            vector.Y = x;
            return vector;
        }

        public static Assimp.Vector3D FlipXZ(this Assimp.Vector3D vector)
        {
            float x = vector.X;
            float z = vector.Z;
            vector.X = z;
            vector.Z = x;
            return vector;
        }

        public static Assimp.Vector3D FlipYZ(this Assimp.Vector3D vector)
        {
            float y = vector.Y;
            float z = vector.Z;
            vector.Y = z;
            vector.Z = y;
            return vector;
        }

        public static Vector3 FlipXY(this Vector3 vector)
        {
            float x = vector.X;
            float y = vector.Y;
            vector.X = y;
            vector.Y = x;
            return vector;
        }

        public static Vector3 FlipXZ(this Vector3 vector)
        {
            float x = vector.X;
            float z = vector.Z;
            vector.X = z;
            vector.Z = x;
            return vector;
        }

        public static Vector3 FlipYZ(this Vector3 vector)
        {
            float y = vector.Y;
            float z = vector.Z;
            vector.Y = z;
            vector.Z = y;
            return vector;
        }

        #endregion

        #region Matrix4x4

        /// <summary>
        /// Convert a System.Numerics.Matrix4x4 into an Assimp.Matrix4x4.
        /// </summary>
        /// <param name="mat4">A System.Numerics.Matrix4x4.</param>
        /// <returns>An Assimp.Matrix4x4.</returns>
        public static Assimp.Matrix4x4 ToAssimpMatrix4x4(this Matrix4x4 mat4)
        {
            return new Assimp.Matrix4x4(mat4.M11, mat4.M21, mat4.M31, mat4.M41,
                                        mat4.M12, mat4.M22, mat4.M32, mat4.M42,
                                        mat4.M13, mat4.M23, mat4.M33, mat4.M43,
                                        mat4.M14, mat4.M24, mat4.M34, mat4.M44);
        }

        /// <summary>
        /// Convert an Assimp.Matrix4x4 into a System.Numerics.Matrix4x4.
        /// </summary>
        /// <param name="mat4">An Assimp.Matrix4x4.</param>
        /// <returns>A System.Numerics.Matrix4x4.</returns>
        public static Matrix4x4 ToNumericsMatrix4x4(this Assimp.Matrix4x4 mat4)
        {
            return new Matrix4x4(mat4.A1, mat4.B1, mat4.C1, mat4.D1,
                                 mat4.A2, mat4.B2, mat4.C2, mat4.D2,
                                 mat4.A3, mat4.B3, mat4.C3, mat4.D3,
                                 mat4.A4, mat4.B4, mat4.C4, mat4.D4);
        }

        #endregion

        #region Transform

        /// <summary>
        /// Compute a transform starting from a given bone to the root bone.
        /// </summary>
        /// <param name="bone">The bone to start from.</param>
        /// <param name="bones">A list of all bones.</param>
        /// <returns>A transform.</returns>
        /// <exception cref="InvalidDataException">The parent index of a bone was outside of the provided bone array.</exception>
        public static Matrix4x4 ComputeTransform(this FLVER.Bone bone, IList<FLVER.Bone> bones)
        {
            var transform = bone.ComputeLocalTransform();
            while (bone.ParentIndex != -1)
            {
                if (!(bone.ParentIndex < -1) && !(bone.ParentIndex > bones.Count))
                {
                    bone = bones[bone.ParentIndex];
                    transform *= bone.ComputeLocalTransform();
                }
                else
                {
                    throw new InvalidDataException("Bone has a parent index outside of the provided bone array.");
                }
            }

            return transform;
        }

        public static Matrix4x4 ComputeTransform(this MDL4.Bone bone, IList<MDL4.Bone> bones)
        {
            var transform = bone.ComputeLocalTransform();
            while (bone.ParentIndex != -1)
            {
                if (!(bone.ParentIndex < -1) && !(bone.ParentIndex > bones.Count))
                {
                    bone = bones[bone.ParentIndex];
                    transform *= bone.ComputeLocalTransform();
                }
                else
                {
                    throw new InvalidDataException("Bone has a parent index outside of the provided bone array.");
                }
            }

            return transform;
        }

        public static Assimp.Matrix4x4 ComputeTransform(this Assimp.Node node)
        {
            var transform = node.Transform;
            while (node.Parent != null)
            {
                transform *= node.Transform;
                node = node.Parent;
            }

            return transform;
        }

        #endregion

        /// <summary>
        /// Convert a VertexBoneIndices struct into an int array.
        /// </summary>
        /// <param name="indices">A VertexBoneIndices struct.</param>
        /// <returns>An int array.</returns>
        public static int[] ToIntArray(this FLVER.VertexBoneIndices indices)
        {
            return new int[4] { indices[0], indices[1], indices[2], indices[3] };
        }

        /// <summary>
        /// Convert a VertexBoneWeights struct into a float array.
        /// </summary>
        /// <param name="weights">A VertexBoneWeights struct.</param>
        /// <returns>An float array.</returns>
        public static float[] ToFloatArray(this FLVER.VertexBoneWeights weights)
        {
            return new float[4] { weights[0], weights[1], weights[2], weights[3] };
        }
    }
}
