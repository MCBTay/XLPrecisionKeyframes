using ReplayEditor;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class EditTimeUI : EditBaseUI
    {
        public float time { get; set; }
        public float originalTime { get; set; }

        private string timeString;

        public override void SetValue(TimeInfo time)
        {
            this.time = time.time;
            this.timeString = time.time.ToString("F8");
            this.originalTime = time.time;
        }

        protected override void OnGUI()
        {
            StartingYPos = 280;
            WindowLabel = "Edit Time";

            base.OnGUI();
        }

        public override void Save()
        {
            if (!float.TryParse(timeString, out var newTime)) return;

            ReplayEditorController.Instance.SetPlaybackTime(newTime);

            base.Save();
        }

        protected override void CreateControls()
        {
            GUILayout.BeginVertical();

            timeString = CreateFloatField("Time", timeString);

            GUILayout.EndVertical();
        }
    }
}
