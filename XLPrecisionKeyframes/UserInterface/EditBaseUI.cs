using System.Linq;
using UnityEngine;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditBaseUI : MonoBehaviour
    {
        private bool HasKeyframes => UserInterface.keyFrames != null && UserInterface.keyFrames.Any();
        private bool HasKeyframeName => !string.IsNullOrEmpty(UserInterface.currentKeyframeName);

        protected float StartingYPos;
        protected string WindowLabel;

        protected float GetYPos(float originalYPos)
        {
            var yPos = originalYPos;

            switch (HasKeyframes)
            {
                case true when HasKeyframeName:
                    yPos += 45;
                    break;
                case true:
                    yPos += 25;
                    break;
            }

            return yPos;
        }

        protected string CreateFloatField(string label, string value)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label($"<b>{label}:</b>");

            value = GUILayout.TextField(value, new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleRight
            });

            GUILayout.EndHorizontal();

            return value;
        }

        protected virtual void OnGUI()
        {
            GUI.backgroundColor = Color.black;

            var style = new GUIStyle(GUI.skin.window)
            {
                contentOffset = new Vector2(0, -20),
                stretchHeight = false,
                stretchWidth = false
            };

            GUILayout.Window(824, new Rect(295, GetYPos(StartingYPos), 200, 50), DrawWindow, WindowLabel, style);
        }

        protected virtual void DrawWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 20f));
            GUI.backgroundColor = Color.black;
        }

        protected virtual void CreateSaveAndCancelButtons()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
            {
                Save();
            }

            if (GUILayout.Button("Cancel"))
            {
                Cancel();
            }

            GUILayout.EndHorizontal();
        }

        protected virtual void Save()
        {
            CloseWindow();
        }

        protected virtual void Cancel()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
