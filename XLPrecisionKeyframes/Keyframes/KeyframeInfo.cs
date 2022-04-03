using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    public class KeyframeInfo
    {
        public PositionInfo position;
        public RotationInfo rotation;
        public float time;

        public KeyframeInfo()
        {
            position = new PositionInfo();
            rotation = new RotationInfo();
            time = 0;
        }

        public KeyframeInfo(Vector3 pos, Quaternion rot)
        {
            position = new PositionInfo(pos);
            rotation = new RotationInfo(rot);
            time = 0;
        }

        public KeyframeInfo(Transform transform) : this(transform.position, transform.rotation)
        {
            
        }
    }
}
