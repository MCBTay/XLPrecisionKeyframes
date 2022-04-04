using ReplayEditor;
using System;
using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class KeyframeInfo
    {
        private PositionInfo _position;
        public PositionInfo position
        {
            get => _position;
            set => _position = value;
        }

        private RotationInfo _rotation;
        public RotationInfo rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        private TimeInfo _time;
        public TimeInfo time
        {
            get => _time;
            set => _time = value;
        }

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
