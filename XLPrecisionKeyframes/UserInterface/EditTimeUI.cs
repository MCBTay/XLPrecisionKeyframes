using ReplayEditor;
using UnityEngine;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditTimeUI : EditBaseUI
    {
        public float time;
        public float originalTime;

        private string timeString;

        public void SetTime(float time)
        {
            this.time = time;
            this.timeString = time.ToString("F8");
            this.originalTime = time;
        }

        private void OnGUI()
        {
            StartingYPos = 280;
            WindowLabel = "Edit Time";

            base.OnGUI();
        }

        protected override void DrawWindow(int windowID)
        {
            base.DrawWindow(windowID);

            GUILayout.BeginVertical();
            CreateTimeControls();
            CreateSaveAndCancelButtons();
            GUILayout.EndVertical();
        }

        protected override void Save()
        {
            if (!float.TryParse(timeString, out var newTime)) return;

            ReplayEditorController.Instance.SetPlaybackTime(newTime);

            base.Save();
        }

        private void CreateTimeControls()
        {
            GUILayout.BeginVertical();

            timeString = CreateFloatField("Time", timeString);

            GUILayout.EndVertical();
        }
    }
}
