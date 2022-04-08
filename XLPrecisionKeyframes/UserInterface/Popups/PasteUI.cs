using HarmonyLib;
using Newtonsoft.Json;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityModManagerNet;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class PasteUI : EditBaseUI
    {
        private string pastedJson;

        private bool parseFailed;

        private bool createKeyframeOnSave = true;

        protected override void OnGUI()
        {
            StartingYPos = 0;
            Label = "Paste Keyframe JSON";
            Height = 465;

            base.OnGUI();
        }

        public override void Save()
        {
            if (string.IsNullOrEmpty(pastedJson)) return;

            SanitizeDiscordTags();

            var keyframes = TrySingleDeserialize();
            if (!keyframes.Any())
            {
                keyframes = TryListDeserialize();
            }

            if (!keyframes.Any())
            {
                parseFailed = true;
                UnityModManager.Logger.Log("XLPK: Unable to deserialize JSON:" + Environment.NewLine + pastedJson);
            }

            var camController = ReplayEditorController.Instance.cameraController;

            foreach (var keyframe in keyframes)
            {
                SetValue(UserInterface.Instance.EditPositionUI, keyframe);
                SetValue(UserInterface.Instance.EditRotationUI, keyframe);
                SetValue(UserInterface.Instance.EditFovUI, keyframe);
                if (!createKeyframeOnSave) continue;

                SetValue(UserInterface.Instance.EditTimeUI, keyframe);
                Traverse.Create(camController).Method("AddKeyFrame", keyframe.time.time).GetValue();
            }

            if (createKeyframeOnSave)
            {
                camController.keyframeUI.UpdateKeyframes(camController.keyFrames);
                UISounds.Instance.PlayOneShotSelectMinor();
            }

            pastedJson = string.Empty;
            parseFailed = false;

            base.Save();
        }

        private List<KeyframeInfo> TrySingleDeserialize()
        {
            KeyframeInfo keyFrameInfo = null;

            try
            {
                keyFrameInfo = JsonConvert.DeserializeObject<KeyframeInfo>(pastedJson);

                if (keyFrameInfo == null)
                {
                    return new List<KeyframeInfo>();
                }
            }
            catch (Exception ex)
            {
                return new List<KeyframeInfo>();
            }

            return new List<KeyframeInfo> { keyFrameInfo };
        }

        private List<KeyframeInfo> TryListDeserialize()
        {
            List<KeyframeInfo> keyFrameInfo = null;

            try
            {
                keyFrameInfo = JsonConvert.DeserializeObject<List<KeyframeInfo>>(pastedJson);

                if (keyFrameInfo == null)
                {
                    return new List<KeyframeInfo>();
                }
            }
            catch (Exception ex)
            {
                return new List<KeyframeInfo>();
            }

            return keyFrameInfo;
        }

        protected override void Cancel()
        {
            pastedJson = string.Empty;
            base.Cancel();
        }

        protected override void CreateSaveButton()
        {
            GUI.enabled = !string.IsNullOrEmpty(pastedJson);

            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = buttonStyle.hover.textColor = parseFailed ? Color.red : Color.white;

            if (!GUILayout.Button(ButtonLabel.Save, buttonStyle))
            {
                GUI.enabled = true;
                return;
            }

            Save();
            GUI.enabled = true;
        }

        private void SetValue<T>(UserInterfacePopup<T> ui, KeyframeInfo keyframe) where T : EditBaseUI
        {
            if (ui == null)
            {
                ui = new UserInterfacePopup<T>();
            }

            switch (ui)
            {
                case UserInterfacePopup<EditPositionUI> pos:
                    ui.SetValue(keyframe.position);
                    break;
                case UserInterfacePopup<EditRotationUI> rot:
                    ui.SetValue(keyframe.rotation);
                    break;
                case UserInterfacePopup<EditFieldOfViewUI> fov:
                    ui.SetValue(keyframe.fov);
                    break;
                case UserInterfacePopup<EditTimeUI> time:
                    ui.SetValue(keyframe.time);
                    break;
                default: break;
            }

            ui.Save();
            ui.Destroy();
        }

        private void SanitizeDiscordTags()
        {
            if (pastedJson.StartsWith("```json"))
            {
                pastedJson = pastedJson.Remove(0, 7);
            }
            else if (pastedJson.StartsWith("```"))
            {
                pastedJson = pastedJson.Remove(0, 3);
            }

            if (pastedJson.EndsWith("```"))
            {
                pastedJson = pastedJson.Remove(pastedJson.Length - 3, 3);
            }

            pastedJson = pastedJson.Trim();
        }

        protected override void CreateControls()
        {
            GUILayout.BeginVertical();
            pastedJson = GUILayout.TextArea(pastedJson, GUILayout.ExpandHeight(true), GUILayout.MinHeight(200), GUILayout.MinHeight(400));
            GUILayout.EndVertical();
        }

        protected override void CreateSaveAndCancelButtons()
        {
            base.CreateSaveAndCancelButtons();

            GUI.backgroundColor = Color.white;
            createKeyframeOnSave = GUILayout.Toggle(createKeyframeOnSave, FieldLabel.CreateOnSave);
            GUI.backgroundColor = Color.black;
        }
    }
}
