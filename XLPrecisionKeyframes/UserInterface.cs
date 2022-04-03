using UnityEngine;
using UnityModManagerNet;

namespace XLPrecisionKeyframes
{
    public class UserInterface : MonoBehaviour
    {
        private static UserInterface __instance;
        public static UserInterface Instance => __instance ?? (__instance = new UserInterface());

        private void OnEnable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnDisable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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

            GUILayout.Window(823, new Rect(40, 40, 200, 50), DrawWindow, "XL Precision Keyframes", style);
        }

        private void DrawWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 20f));
            GUI.backgroundColor = Color.black;

            GUILayout.BeginVertical();

            CreatePositionControls();
            CreateRotationControls();
            CreateTimeControls();

            GUILayout.EndVertical();
        }

        private void CreatePositionControls()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Position");

            CreateFloatField("X", "1");
            CreateFloatField("Y", "2");
            CreateFloatField("Z", "3");

            GUILayout.EndVertical();
        }

        private void CreateRotationControls()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Rotation");

            CreateFloatField("X", "1");
            CreateFloatField("Y", "2");
            CreateFloatField("Z", "3");
            CreateFloatField("W", "4");

            GUILayout.EndVertical();
        }

        private void CreateTimeControls()
        {
            CreateFloatField("Time", "Who knows?", false);
        }

        private void CreateFloatField(string label, string value, bool isIndented = true)
        {
            GUILayout.BeginHorizontal();

            if (isIndented)
                GUILayout.Space(20);

            GUILayout.Label(label);
            GUILayout.TextField(value);

            GUILayout.EndHorizontal();
        }
        

        public void UpdateTextFields(Transform cameraTransform)
        {
            if (cameraTransform == null) return;

            //UnityModManager.Logger.Log("XLPK: Camera position: " + cameraTransform.position);
            //UnityModManager.Logger.Log("XLPK: Camera rotation: " + cameraTransform.rotation);
        }
    }
}

