using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    public class PositionInfo
    {
        public string x;
        public string y;
        public string z;

        public PositionInfo() : this(Vector3.zero)
        {

        }

        public PositionInfo(Vector3 position)
        {
            x = position.x.ToString("F8");
            y = position.y.ToString("F8");
            z = position.z.ToString("F8");
        }

        public override string ToString()
        {
            return $"({x:F8}, {y:F8}, {z:F8})";
        }
    }
}
