using Newtonsoft.Json;
using ReplayEditor;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;
using XLPrecisionKeyframes.UserInterface.Popups;

namespace XLPrecisionKeyframes.UserInterface
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
        public static List<KeyFrame> keyFrames = new List<KeyFrame>();

        public static string currentKeyframeName = "";

        public UserInterfacePopup<PasteUI> PasteUI { get; set; }
        public UserInterfacePopup<EditPositionUI> EditPositionUI { get; set; }
        public UserInterfacePopup<EditRotationUI> EditRotationUI { get; set; }
        public UserInterfacePopup<EditTimeUI> EditTimeUI { get; set; }
        public UserInterfacePopup<EditFieldOfViewUI> EditFovUI { get; set; }

        private void OnEnable()
        {
            PasteUI = new UserInterfacePopup<PasteUI>();
            EditPositionUI = new UserInterfacePopup<EditPositionUI>();
            EditRotationUI = new UserInterfacePopup<EditRotationUI>();
            EditTimeUI = new UserInterfacePopup<EditTimeUI>();
            EditFovUI = new UserInterfacePopup<EditFieldOfViewUI>();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnDisable()
        {
            PasteUI.Destroy();
            EditPositionUI.Destroy();
            EditRotationUI.Destroy();
            EditTimeUI.Destroy();
            EditFovUI.Destroy();

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

            GUILayout.Window(823, new Rect(Settings.Instance.WindowXPos, Settings.Instance.WindowYPos, 250, 50), DrawWindow, "XL Precision Keyframes", style);
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
            CreateFieldOfViewControls();

            GUILayout.EndVertical();
        }

        private void CreateKeyframeControls()
        {
            if (keyFrames == null) return;
            if (!keyFrames.Any()) return;

            GUILayout.BeginVertical();

            CreateKeyframeNameControl();
            CreateKeyframeArrowControls();
            CreateKeyframeDeleteButtons();

            GUILayout.EndVertical();
        }

        private void CreateKeyframeNameControl()
        {
            if (string.IsNullOrEmpty(currentKeyframeName)) return;

            GUILayout.Label(currentKeyframeName, new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            });
        }

        private void CreateKeyframeArrowControls()
        {
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
        }

        private void CreateKeyframeDeleteButtons()
        {
            if (keyFrames == null) return;
            if (!keyFrames.Any()) return;

            GUILayout.BeginHorizontal();
            CreateKeyframeDeleteButton();
            CreateKeyframeDeleteAllButton();
            GUILayout.EndHorizontal();
        }

        private void CreateKeyframeDeleteButton()
        {
            if (string.IsNullOrEmpty(currentKeyframeName)) return;

            if (!GUILayout.Button("Delete")) return;

            var index = currentKeyframeName.Split(' ').LastOrDefault();
            if (string.IsNullOrEmpty(index)) return;

            if (!int.TryParse(index, out var keyframeIndex)) return;

            var camController = ReplayEditorController.Instance.cameraController;

            UISounds.Instance.PlayOneShotSelectMinor();

            // subtract 1 here because we add 1 to the text we're parsing this from for user display reasons
            // keyframe index should always be >= 1
            Traverse.Create(camController).Method("DeleteKeyFrame", keyframeIndex - 1, true).GetValue();
            camController.keyframeUI.UpdateKeyframes(camController.keyFrames);
        }

        private void CreateKeyframeDeleteAllButton()
        {
            if (!GUILayout.Button("Delete All")) return;

            UISounds.Instance.PlayOneShotSelectMinor();

            var camController = ReplayEditorController.Instance.cameraController;

            camController.DeleteAllKeyFrames();
            camController.keyframeUI.UpdateKeyframes(camController.keyFrames);
        }

        private void CreateCopyPasteControls()
        {
            GUILayout.BeginVertical();

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

            if (GUILayout.Button("Paste JSON"))
            {
                PasteUI.Show();
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
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

            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>Position</b>");

            if (GUILayout.Button("Edit"))
            {
                EditPositionUI.Show(displayed.position);
            }
            GUILayout.EndHorizontal();

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

            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>Rotation</b>");

            if (GUILayout.Button("Edit"))
            {
                EditRotationUI.Show(displayed.rotation);
            }

            GUILayout.EndHorizontal();

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
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>Time</b>");

            if (GUILayout.Button("Edit"))
            {
                EditTimeUI.Show(displayed.time);
            }
            GUILayout.EndHorizontal();

            CreateFloatField("Time", displayed.time.time.ToString("F8"));
            CreateFloatField("From End", displayed.time.timeFromEnd.ToString("F8"));

            if (keyFrames != null && keyFrames.Any())
            {
                CreateFloatField("To Prev Keyframe", displayed.time.timeFromPrevKeyframe.ToString("F8"));
                CreateFloatField("To Next Keyframe", displayed.time.timeFromNextKeyframe.ToString("F8"));
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Creates the field of view edit section, which contains a FOV label and FOV field showing the current field of view.
        /// </summary>
        private void CreateFieldOfViewControls()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>Field of View</b>");

            if (GUILayout.Button("Edit"))
            {
                EditFovUI.Show(displayed.fov);
            }
            GUILayout.EndHorizontal();

            CreateFloatField("FOV", displayed.fov.fov.ToString("F5"));

            GUILayout.EndVertical();
        }

        private void CreateFloatField(string label, string value)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(20);

            GUILayout.Label($"<b>{label}:</b>");
            GUILayout.Label(value, new GUIStyle(GUI.skin.label)
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
        
        public void UpdateTextFields(Transform cameraTransform, float? time, float? fov)
        {
            if (cameraTransform == null) return;

            displayed.position.Update(cameraTransform.position);
            displayed.rotation.Update(cameraTransform.rotation);
            displayed.time.Update(time ?? 0);
            displayed.fov.Update(fov ?? 0);
        }
    }
}

