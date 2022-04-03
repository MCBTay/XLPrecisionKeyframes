using Newtonsoft.Json;
using ReplayEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes
{
    public class UserInterface : MonoBehaviour
    {
        private static UserInterface __instance;
        public static UserInterface Instance => __instance ?? (__instance = new UserInterface());

        /// <summary>
        /// The currently displayed keyframe information.
        /// </summary>
        private static KeyframeInfo displayed = new KeyframeInfo();
        
        /// <summary>
        /// A list of keyframes that are currently in editor.  Currently used for the keyframe controls, knowing whether to hide them or how to cycle through them.
        /// </summary>
        private static List<KeyFrame> keyFrames = new List<KeyFrame>();

        private static string currentKeyframeName = "";

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

            CreateKeyframeControls();
            CreateCopyPasteControls();
            CreatePositionControls();
            CreateRotationControls();
            CreateTimeControls();

            GUILayout.EndVertical();
        }

        private void CreateKeyframeControls()
        {
            if (keyFrames == null) return;
            if (!keyFrames.Any()) return;

            GUILayout.BeginVertical();

            if (!string.IsNullOrEmpty(currentKeyframeName))
            {
                GUILayout.Label(currentKeyframeName, new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                });
            }
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("<<"))
            {
                var keyframe = keyFrames.FirstOrDefault();
                ReplayEditorController.Instance.SetPlaybackTime(keyframe?.time ?? 0);
            }

            if (GUILayout.Button("<"))
            {
                ReplayEditorController.Instance.JumpByTime(-ReplaySettings.Instance.PlaybackTimeJumpDelta, true);
            }

            if (GUILayout.Button(">"))
            {
                ReplayEditorController.Instance.JumpByTime(ReplaySettings.Instance.PlaybackTimeJumpDelta, true);
            }

            if (GUILayout.Button(">>"))
            {
                var keyframe = keyFrames.LastOrDefault();
                ReplayEditorController.Instance.SetPlaybackTime(keyframe?.time ?? ReplayEditorController.Instance.playbackController.ClipEndTime);
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void CreateCopyPasteControls()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Copy"))
            {
                AddToClipboard(displayed);
            }

            if (GUILayout.Button("Copy All"))
            {
                var frames = keyFrames.Select(frame => new KeyframeInfo(frame)).ToList();

                AddToClipboard(frames);
            }

            GUILayout.EndHorizontal();
        }

        private void AddToClipboard(object o)
        {
            var json = JsonConvert.SerializeObject(o, Formatting.Indented);
            json = $"```json\n{json}\n```";
            GUIUtility.systemCopyBuffer = json;
        }

        /// <summary>
        /// Creates the position edit section, which contains a Position label and X, Y, and Z fields.
        /// </summary>
        private void CreatePositionControls()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("<b>Position</b>");

            CreateFloatField("X", displayed.position.x);
            CreateFloatField("Y", displayed.position.y);
            CreateFloatField("Z", displayed.position.z);

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Creates the rotation edit section, which contains a Rotation label and X, Y, Z, and W fields.
        /// </summary>
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

        /// <summary>
        /// Creates the time edit section, which contains a Time label and time field showing CurrentTime.
        /// </summary>
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

        public void UpdateKeyFrameControls(List<KeyFrame> frames, float? time)
        {
            keyFrames = frames;

            var match = keyFrames.FirstOrDefault(x => Mathf.Approximately(x.time, time ?? 0));
            currentKeyframeName = match != null ? $"Keyframe {keyFrames.IndexOf(match) + 1}" : string.Empty;
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

