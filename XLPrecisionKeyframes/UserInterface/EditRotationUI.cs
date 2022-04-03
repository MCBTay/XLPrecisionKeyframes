using ReplayEditor;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditRotationUI : EditBaseUI
    {
        public RotationInfo originalRotation;
        public RotationInfo rotation;

        public void SetRotation(RotationInfo rotation)
        {
            this.rotation = new RotationInfo(rotation);
            this.originalRotation = new RotationInfo(rotation);
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

            GUILayout.Window(825, new Rect(245, 170, 200, 50), DrawWindow, "Edit Rotation", style);
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
                UpdateCameraRotation();
                gameObject.SetActive(false);
            }

            if (GUILayout.Button("Cancel"))
            {
                gameObject.SetActive(false);
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void UpdateCameraRotation()
        {
            ReplayEditorController.Instance.cameraController.ReplayCamera.transform.rotation = rotation.ConvertToQuaternion();
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
