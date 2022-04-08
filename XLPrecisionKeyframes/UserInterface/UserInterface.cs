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
        private static List<KeyFrame> keyFrames = new List<KeyFrame>();

        private bool hasKeyframes => keyFrames != null && !keyFrames.Any();

        private static string currentKeyframeName = FieldLabel.NoKeyframe;

        public UserInterfacePopup<PasteUI> PasteUI { get; set; }
        public UserInterfacePopup<OffsetKeyframesUI> OffsetKeyframesUI { get; set; }
        public UserInterfacePopup<EditPositionUI> EditPositionUI { get; set; }
        public UserInterfacePopup<EditRotationUI> EditRotationUI { get; set; }
        public UserInterfacePopup<EditTimeUI> EditTimeUI { get; set; }
        public UserInterfacePopup<EditFieldOfViewUI> EditFovUI { get; set; }

        private void OnEnable()
        {
            PasteUI = new UserInterfacePopup<PasteUI>();
            OffsetKeyframesUI = new UserInterfacePopup<OffsetKeyframesUI>();
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
            OffsetKeyframesUI.Destroy();
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

            var rect = new Rect(Settings.Instance.WindowXPos, Settings.Instance.WindowYPos, 250, 465);

            GUILayout.Window(854, rect, DrawWindow, WindowLabel.XLPrecisionKeyframes, style);
        }

        private void DrawWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 20f));
            GUI.backgroundColor = Color.black;

            GUILayout.BeginVertical();

            CreateKeyframeControls();
            CreatePositionControls();
            CreateRotationControls();
            CreateTimeControls();
            CreateFieldOfViewControls();

            GUILayout.EndVertical();
        }

        #region Keyframe controls
        private void CreateKeyframeControls()
        {
            GUILayout.BeginVertical();
            CreateKeyframeNameControl();
            CreateKeyframeArrowControls();
            CreateKeyframeDeleteButtons();
            CreateCopyPasteControls();
            CreateOffsetControls();
            GUILayout.EndVertical();
        }

        private void CreateKeyframeNameControl()
        {
            GUILayout.Label(currentKeyframeName, new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            });
        }

        private void CreateKeyframeArrowControls()
        {
            GUI.enabled = !hasKeyframes;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(ButtonLabel.DoubleLeft))
            {
                var keyframe = keyFrames.FirstOrDefault();
                ReplayEditorController.Instance.SetPlaybackTime(keyframe?.time ?? 0);
            }

            if (GUILayout.Button(ButtonLabel.Left))
            {
                ReplayEditorController.Instance.JumpByTime(-ReplaySettings.Instance.PlaybackTimeJumpDelta, true);
            }

            if (GUILayout.Button(ButtonLabel.Right))
            {
                ReplayEditorController.Instance.JumpByTime(ReplaySettings.Instance.PlaybackTimeJumpDelta, true);
            }

            if (GUILayout.Button(ButtonLabel.DoubleRight))
            {
                var keyframe = keyFrames.LastOrDefault();
                ReplayEditorController.Instance.SetPlaybackTime(keyframe?.time ?? ReplayEditorController.Instance.playbackController.ClipEndTime);
            }
            GUILayout.EndHorizontal();

            GUI.enabled = true;
        }

        #region Delete buttons
        private void CreateKeyframeDeleteButtons()
        {
            GUI.enabled = !hasKeyframes;

            GUILayout.BeginHorizontal();
            CreateKeyframeDeleteButton();
            CreateKeyframeDeleteAllButton();
            GUILayout.EndHorizontal();

            GUI.enabled = true;
        }

        private void CreateKeyframeDeleteButton()
        {
            GUI.enabled = currentKeyframeName != FieldLabel.NoKeyframe;

            if (!GUILayout.Button(ButtonLabel.Delete))
            {
                GUI.enabled = true;
                return;
            }

            var index = currentKeyframeName.Split(' ').LastOrDefault();
            if (string.IsNullOrEmpty(index))
            {
                GUI.enabled = true;
                return;
            }

            if (!int.TryParse(index, out var keyframeIndex))
            {
                GUI.enabled = true;
                return;
            }

            var camController = ReplayEditorController.Instance.cameraController;

            UISounds.Instance.PlayOneShotSelectMinor();

            // subtract 1 here because we add 1 to the text we're parsing this from for user display reasons
            // keyframe index should always be >= 1
            Traverse.Create(camController).Method("DeleteKeyFrame", keyframeIndex - 1, true).GetValue();
            camController.keyframeUI.UpdateKeyframes(camController.keyFrames);

            GUI.enabled = true;
        }

        private void CreateKeyframeDeleteAllButton()
        {
            GUI.enabled = !hasKeyframes;

            if (!GUILayout.Button(ButtonLabel.DeleteAll))
            {
                GUI.enabled = true;
                return;
            }

            UISounds.Instance.PlayOneShotSelectMinor();

            var camController = ReplayEditorController.Instance.cameraController;

            camController.DeleteAllKeyFrames();
            camController.keyframeUI.UpdateKeyframes(camController.keyFrames);

            GUI.enabled = true;
        }
        #endregion

        private void CreateCopyPasteControls()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(ButtonLabel.Copy))
            {
                AddToClipboard(displayed);
            }

            GUI.enabled = !hasKeyframes;
            if (GUILayout.Button(ButtonLabel.CopyAll))
            {
                var frames = keyFrames.Select(frame => new KeyframeInfo(frame)).ToList();
                AddToClipboard(frames);
            }
            GUI.enabled = true;

            if (GUILayout.Button(ButtonLabel.Paste))
            {
                PasteUI.Show();
            }

            GUILayout.EndHorizontal();
        }

        private void CreateOffsetControls()
        {
            GUI.enabled = !hasKeyframes;

            if (GUILayout.Button(ButtonLabel.OffsetKeyframes))
            {
                OffsetKeyframesUI.Show();
            }

            GUI.enabled = true;
        }

        private void AddToClipboard(object o)
        {
            var json = JsonConvert.SerializeObject(o, Formatting.Indented);
            json = $"```json\n{json}\n```";
            GUIUtility.systemCopyBuffer = json;
        }
        #endregion

        /// <summary>
        /// Creates the position edit section, which contains a Position label and X, Y, and Z fields.
        /// </summary>
        private void CreatePositionControls()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"<b>{FieldLabel.Position}</b>");

            if (GUILayout.Button(ButtonLabel.Edit, GUILayout.Width(50)))
            {
                EditPositionUI.Show(displayed.position);
            }
            GUILayout.EndHorizontal();

            CreateFloatField(FieldLabel.X, displayed.position.x);
            CreateFloatField(FieldLabel.Y, displayed.position.y);
            CreateFloatField(FieldLabel.Z, displayed.position.z);

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Creates the rotation edit section, which contains a Rotation label and X, Y, Z, and W fields.
        /// </summary>
        private void CreateRotationControls()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"<b>{FieldLabel.Rotation}</b>");

            if (GUILayout.Button(ButtonLabel.Edit, GUILayout.Width(50)))
            {
                EditRotationUI.Show(displayed.rotation);
            }

            GUILayout.EndHorizontal();

            CreateFloatField(FieldLabel.X, displayed.rotation.x);
            CreateFloatField(FieldLabel.Y, displayed.rotation.y);
            CreateFloatField(FieldLabel.Z, displayed.rotation.z);
            CreateFloatField(FieldLabel.W, displayed.rotation.w);

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Creates the time edit section, which contains a Time label and time field showing CurrentTime.
        /// </summary>
        private void CreateTimeControls()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"<b>{FieldLabel.Time}</b>");

            if (GUILayout.Button(ButtonLabel.Edit, GUILayout.Width(50)))
            {
                EditTimeUI.Show(displayed.time);
            }
            GUILayout.EndHorizontal();

            CreateFloatField(FieldLabel.Time, displayed.time.time.ToString("F8"));
            CreateFloatField(FieldLabel.FromEnd, displayed.time.timeFromEnd.ToString("F8"));

            GUI.enabled = !hasKeyframes;
            CreateFloatField(FieldLabel.ToPrevKeyframe, displayed.time.timeFromPrevKeyframe.ToString("F8"));
            CreateFloatField(FieldLabel.ToNextKeyframe, displayed.time.timeFromNextKeyframe.ToString("F8"));
            GUI.enabled = true;

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Creates the field of view edit section, which contains a FOV label and FOV field showing the current field of view.
        /// </summary>
        private void CreateFieldOfViewControls()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"<b>{FieldLabel.FieldOfView}</b>");

            if (GUILayout.Button(ButtonLabel.Edit, GUILayout.Width(50)))
            {
                EditFovUI.Show(displayed.fov);
            }
            GUILayout.EndHorizontal();

            CreateFloatField(FieldLabel.FieldOfView, displayed.fov.fov.ToString("F5"));

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
            currentKeyframeName = match != null ? 
                string.Format(FieldLabel.KeyframeNumber, keyFrames.IndexOf(match) + 1) : 
                FieldLabel.NoKeyframe;
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

