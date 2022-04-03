using ReplayEditor;
using UnityEngine;
using UnityModManagerNet;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditPositionUI : EditBaseUI
    {
        public PositionInfo position;
        public PositionInfo originalPosition;

        public void SetPosition(PositionInfo position)
        {
            this.position = position;
            this.originalPosition = position;
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

            GUILayout.Window(824, new Rect(245, 80, 200, 50), DrawWindow, "Edit Position", style);
        }

        private void DrawWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 20f));
            GUI.backgroundColor = Color.black;

            GUILayout.BeginVertical();

            CreatePositionControls();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
            {
                UpdateCameraPosition();
                gameObject.SetActive(false);
            }

            if (GUILayout.Button("Cancel"))
            {
                gameObject.SetActive(false);
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void UpdateCameraPosition()
        {
            var newPosition = position.ConvertToVector3();
            UnityModManager.Logger.Log("XLPK: Updating cam position to " + newPosition + " from " + ReplayEditorController.Instance.cameraController.ReplayCamera.transform.position);
            ReplayEditorController.Instance.cameraController.ReplayCamera.transform.position = newPosition;
            UnityModManager.Logger.Log("XLPK: New cam position: " + ReplayEditorController.Instance.cameraController.ReplayCamera.transform.position);
        }

        private void CreatePositionControls()
        {
            GUILayout.BeginVertical();

            position.x = CreateFloatField("X", position.x);
            position.y = CreateFloatField("Y", position.y);
            position.z = CreateFloatField("Z", position.z);

            GUILayout.EndVertical();
        }
    }
}
