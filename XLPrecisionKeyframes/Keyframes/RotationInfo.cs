﻿using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    public class RotationInfo
    {
        public string x;
        public string y;
        public string z;
        public string w;

        public RotationInfo() : this(Quaternion.identity)
        {

        }

        public RotationInfo(Quaternion rotation)
        {
            x = rotation.x.ToString("F8");
            y = rotation.y.ToString("F8");
            z = rotation.z.ToString("F8");
            w = rotation.w.ToString("F8");
        }

        public override string ToString()
        {
            return $"({x:F8}, {y:F8}, {z:F8}, {w:F8})";
        }
    }
}