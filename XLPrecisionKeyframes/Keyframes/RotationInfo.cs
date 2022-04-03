using UnityEngine;

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
            x = rotation.x.ToString();
            y = rotation.y.ToString();
            z = rotation.z.ToString();
            w = rotation.w.ToString();
        }
    }
}
