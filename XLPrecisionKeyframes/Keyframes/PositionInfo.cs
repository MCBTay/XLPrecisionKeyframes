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
            x = position.x.ToString();
            y = position.y.ToString();
            z = position.z.ToString();
        }
    }
}
