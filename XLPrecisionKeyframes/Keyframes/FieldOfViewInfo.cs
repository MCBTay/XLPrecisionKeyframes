using System;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class FieldOfViewInfo
    {
        public float fov { get; set; }

        public FieldOfViewInfo() : this(0)
        {
            
        }

        public FieldOfViewInfo(float fov)
        {
            this.fov = fov;
        }

        public void Update(float fov)
        {
            this.fov = fov;
        }
    }
}
