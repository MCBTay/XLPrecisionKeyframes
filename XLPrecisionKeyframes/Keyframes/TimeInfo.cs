using ReplayEditor;
using System;

namespace XLPrecisionKeyframes.Keyframes
{
    [Serializable]
    public class TimeInfo
    {
        private float _time;
        public float time
        {
            get => _time;
            set => _time = value;
        }

        private float _timeFromEnd;
        public float timeFromEnd
        {
            get => _timeFromEnd;
            set => _timeFromEnd = value;
        }

        private float _timeFromPrevKeyframe;
        public float timeFromPrevKeyframe
        {
            get => _timeFromPrevKeyframe;
            set => _timeFromPrevKeyframe = value;
        }

        private float _timeFromNextKeyframe;
        public float timeFromNextKeyframe
        {
            get => _timeFromNextKeyframe;
            set => _timeFromNextKeyframe = value;
        }

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
