using System;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class FieldOfViewInfo
    {
        public string fov { get; set; }

        public FieldOfViewInfo() : this(0)
        {
            
        }

        public FieldOfViewInfo(float fov)
        {
            this.fov = fov.ToString("F8");
        }

        public void Update(float fov)
        {
            this.fov = fov.ToString("F8");
        }
    }
}
