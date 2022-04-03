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
    }
}
