using System;
using System.Numerics;

namespace SFModelConverter
{
    public static class MathUtil
    {
        public static Quaternion Euler(float yaw, float pitch, float roll)
        {
            yaw = ToRadians(yaw);
            pitch = ToRadians(pitch);
            roll = ToRadians(roll);

            double yawOver2 = yaw * 0.5f;
            float cosYawOver2 = (float)Math.Cos(yawOver2);
            float sinYawOver2 = (float)Math.Sin(yawOver2);
            double pitchOver2 = pitch * 0.5f;
            float cosPitchOver2 = (float)Math.Cos(pitchOver2);
            float sinPitchOver2 = (float)Math.Sin(pitchOver2);
            double rollOver2 = roll * 0.5f;
            float cosRollOver2 = (float)Math.Cos(rollOver2);
            float sinRollOver2 = (float)Math.Sin(rollOver2);
            Quaternion result;
            result.W = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.X = sinYawOver2 * cosPitchOver2 * cosRollOver2 + cosYawOver2 * sinPitchOver2 * sinRollOver2;
            result.Y = cosYawOver2 * sinPitchOver2 * cosRollOver2 - sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.Z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

            return result;
        }

        public static float ToRadians(float degrees)
        {
            return (float)((double)degrees * (Math.PI / 180.0));
        }
    }
}
