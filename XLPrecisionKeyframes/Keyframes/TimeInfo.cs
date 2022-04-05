using ReplayEditor;
using System;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class TimeInfo
    {
        public float time { get; set; }
        public float timeFromEnd { get; set; }
        public float timeFromPrevKeyframe { get; set; }
        public float timeFromNextKeyframe { get; set; }

        public TimeInfo() : this(0)
        {
            
        }

        public TimeInfo(float time)
        {
            Update(time);
        }

        public void Update(float newTime)
        {
            time = newTime;

            var clipEndTime = ReplayEditorController.Instance.playbackController.ClipEndTime;

            timeFromEnd = clipEndTime - time;

            var camController = ReplayEditorController.Instance.cameraController;

            var prevKeyFrame = camController.FindNextKeyFrame(time, true);
            timeFromPrevKeyframe = time - (prevKeyFrame?.time ?? 0);

            var nextKeyFrame = camController.FindNextKeyFrame(time, false);
            timeFromNextKeyframe = (nextKeyFrame?.time ?? clipEndTime) - time;
        }
    }
}
