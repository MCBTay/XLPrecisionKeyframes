using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityModManagerNet;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class PasteUI : EditBaseUI
    {
        private string pastedJson;

        private bool parseFailed;

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

            KeyframeInfo keyFrameInfo = null;

            try
            {
                keyFrameInfo = JsonConvert.DeserializeObject<KeyframeInfo>(pastedJson);

                if (keyFrameInfo == null)
                {
                    parseFailed = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                parseFailed = true;
                UnityModManager.Logger.Log("XLPK: Exception caught deserializing JSON: " + pastedJson + ex);
                return;
            }

            SetValue(UserInterface.Instance.EditPositionUI, keyFrameInfo);
            SetValue(UserInterface.Instance.EditRotationUI, keyFrameInfo);
            SetValue(UserInterface.Instance.EditFovUI, keyFrameInfo);

            pastedJson = string.Empty;
            parseFailed = false;

            base.Save();
        }

        protected override void Cancel()
        {
            pastedJson = string.Empty;
            base.Cancel();
        }

        protected override void CreateSaveButton()
        {
            if (string.IsNullOrEmpty(pastedJson)) return;

            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = buttonStyle.hover.textColor = parseFailed ? Color.red : Color.white;
            
            if (!GUILayout.Button(ButtonLabel.Save, buttonStyle)) return;

            Save();
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
    }
}
