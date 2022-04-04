using ReplayEditor;
using System;
using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class KeyframeInfo
    {
        public PositionInfo position;
        public RotationInfo rotation;
        public TimeInfo time;

        public KeyframeInfo()
        {
            position = new PositionInfo();
            rotation = new RotationInfo();
            time = new TimeInfo();
        }

        public KeyframeInfo(Vector3 pos, Quaternion rot)
        {
            position = new PositionInfo(pos);
            rotation = new RotationInfo(rot);
            time = new TimeInfo();
        }

        public KeyframeInfo(Transform transform) : this(transform.position, transform.rotation)
        {
            
        }

        public KeyframeInfo(KeyFrame keyframe) : this(keyframe.position, keyframe.rotation)
        {
            time = new TimeInfo(keyframe.time);
        }
    }
}
