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
            GUI.backgroundColor = Color.black;

            var style = new GUIStyle(GUI.skin.window)
            {
                contentOffset = new Vector2(0, -20),
                stretchHeight = false,
                stretchWidth = false
            };

            GUILayout.Window(824, new Rect(245, 80, 200, 50), DrawWindow, "Edit Time", style);
        }

        private void DrawWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 20f));
            GUI.backgroundColor = Color.black;

            GUILayout.BeginVertical();

            CreateTimeControls();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
            {
                UpdateTimelinePosition();
                gameObject.SetActive(false);
            }

            if (GUILayout.Button("Cancel"))
            {
                gameObject.SetActive(false);
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void UpdateTimelinePosition()
        {
            ReplayEditorController.Instance.SetPlaybackTime(time);
        }

        private void CreateTimeControls()
        {
            GUILayout.BeginVertical();

            timeString = CreateFloatField("Time", timeString);

            GUILayout.EndVertical();
        }
    }
}
