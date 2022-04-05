using ReplayEditor;
using System;
using UnityEngine;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class KeyframeInfo
    {
        public PositionInfo position { get; set; }
        public RotationInfo rotation { get; set; }
        public TimeInfo time { get; set; }
        public FieldOfViewInfo fov { get; set; }

        public KeyframeInfo()
        {
            position = new PositionInfo();
            rotation = new RotationInfo();
            time = new TimeInfo();
            fov = new FieldOfViewInfo();
        }

        public KeyframeInfo(Vector3 pos, Quaternion rot)
        {
            position = new PositionInfo(pos);
            rotation = new RotationInfo(rot);
            time = new TimeInfo();
            fov = new FieldOfViewInfo();
        }

        public KeyframeInfo(Transform transform) : this(transform.position, transform.rotation)
        {
            
        }

        public KeyframeInfo(KeyFrame keyframe) : this(keyframe.position, keyframe.rotation)
        {
            time = new TimeInfo(keyframe.time);
            fov = new FieldOfViewInfo(keyframe.fov);
        }
    }
}
