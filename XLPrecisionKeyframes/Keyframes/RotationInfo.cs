using System;
using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class RotationInfo
    {
        private string _x;
        public string x
        {
            get => _x;
            set => _x = value;
        }

        private string _y;
        public string y
        {
            get => _y;
            set => _y = value;
        }

        private string _z;
        public string z
        {
            get => _z;
            set => _z = value;
        }

        private string _w;
        public string w
        {
            get => _w;
            set => _w = value;
        }

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

        public RotationInfo(RotationInfo rotationInfo)
        {
            x = rotationInfo.x;
            y = rotationInfo.y;
            z = rotationInfo.z;
            w = rotationInfo.w;
        }

        public override string ToString()
        {
            return $"({x:F8}, {y:F8}, {z:F8}, {w:F8})";
        }

        public void Update(Quaternion rotation)
        {
            x = rotation.x.ToString("F8");
            y = rotation.y.ToString("F8");
            z = rotation.z.ToString("F8");
            w = rotation.w.ToString("F8");
        }

        public Quaternion ConvertToQuaternion()
        {
            var xSuccess = float.TryParse(x, out float newX);
            var ySuccess = float.TryParse(y, out float newY);
            var zSuccess = float.TryParse(z, out float newZ);
            var wSuccess = float.TryParse(w, out float newW);

            return new Quaternion(xSuccess ? newX : 0, ySuccess ? newY : 0, zSuccess ? newZ : 0, wSuccess ? newW : 0);
        }
    }
}
