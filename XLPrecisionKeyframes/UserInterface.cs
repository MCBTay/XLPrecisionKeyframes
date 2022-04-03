using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes
{
    public class UserInterface : MonoBehaviour
    {
        private static UserInterface __instance;
        public static UserInterface Instance => __instance ?? (__instance = new UserInterface());

        private static KeyframeInfo displayed = new KeyframeInfo();

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

            GUILayout.Label("<b>Position</b>");

            CreateFloatField("X", displayed.position.x);
            CreateFloatField("Y", displayed.position.y);
            CreateFloatField("Z", displayed.position.z);

            GUILayout.EndVertical();
        }

        private void CreateRotationControls()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("<b>Rotation</b>");

            CreateFloatField("X", displayed.rotation.x);
            CreateFloatField("Y", displayed.rotation.y);
            CreateFloatField("Z", displayed.rotation.z);
            CreateFloatField("W", displayed.rotation.w);

            GUILayout.EndVertical();
        }

        private void CreateTimeControls()
        {
            CreateFloatField("Time", displayed.time.ToString("F8"), false);
        }
        
        private void CreateFloatField(string label, string value, bool isIndented = true)
        {
            GUILayout.BeginHorizontal();

            if (isIndented)
                GUILayout.Space(20);

            GUILayout.Label($"<b>{label}:</b>");
            GUILayout.TextField(value, new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleRight
            });

            GUILayout.EndHorizontal();
        }

        private float ParseStringToFloat(string value)
        {
            if (!float.TryParse(value, out float parsedFloat))
            {
                return 0;
            }

            return parsedFloat;
        }
        
        public void UpdateTextFields(Transform cameraTransform, float? time)
        {
            if (cameraTransform == null) return;

            displayed.position = new PositionInfo(cameraTransform.position);
            displayed.rotation = new RotationInfo(cameraTransform.rotation);
            displayed.time = time ?? 0;
        }
    }
}

