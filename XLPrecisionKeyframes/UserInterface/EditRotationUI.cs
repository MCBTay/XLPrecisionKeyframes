using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditRotationUI : EditBaseUI
    {
        public RotationInfo rotation;

        private void OnGUI()
        {
            GUI.backgroundColor = Color.black;

            var style = new GUIStyle(GUI.skin.window)
            {
                contentOffset = new Vector2(0, -20),
                stretchHeight = false,
                stretchWidth = false
            };

            GUILayout.Window(823, new Rect(40, 40, 200, 50), DrawWindow, "Edit Rotation", style);
        }

        private void DrawWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 20f));
            GUI.backgroundColor = Color.black;

            GUILayout.BeginVertical();

            CreateRotationControls();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
            {

            }

            if (GUILayout.Button("Cancel"))
            {

            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void CreateRotationControls()
        {
            GUILayout.BeginVertical();

            rotation.x = CreateFloatField("X", rotation.x);
            rotation.y = CreateFloatField("Y", rotation.y);
            rotation.z = CreateFloatField("Z", rotation.z);
            rotation.w = CreateFloatField("W", rotation.w);

            GUILayout.EndVertical();
        }
    }
}
