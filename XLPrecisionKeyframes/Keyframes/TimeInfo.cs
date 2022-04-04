using ReplayEditor;
using System;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class TimeInfo
    {
        public float time;
        public float timeFromEnd;
        public float timeFromPrevKeyframe;
        public float timeFromNextKeyframe;

        public TimeInfo() : this(0)
        {
            
        }

        public TimeInfo(float time)
        {
            this.time = time;
        }

        public void Update(float newTime)
        {
            time = newTime;

            var clipEndTime = ReplayEditorController.Instance.playbackController.ClipEndTime;

            timeFromEnd = clipEndTime - time;

            var prevKeyFrame = ReplayEditorController.Instance.cameraController.FindNextKeyFrame(time, true);
            timeFromPrevKeyframe = time - (prevKeyFrame?.time ?? 0);

            var nextKeyFrame = ReplayEditorController.Instance.cameraController.FindNextKeyFrame(time, false);
            timeFromNextKeyframe = (nextKeyFrame?.time ?? clipEndTime) - time;
        }
    }
}
