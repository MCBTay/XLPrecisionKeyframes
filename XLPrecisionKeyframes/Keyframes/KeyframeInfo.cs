using System;
using ReplayEditor;
using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
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

        public KeyframeInfo(KeyFrame keyframe) : this(keyframe.position, keyframe.rotation)
        {
            time = keyframe.time;
        }
    }
}
