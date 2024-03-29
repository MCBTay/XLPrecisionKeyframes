﻿using System.Linq;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class EditBaseUI : MonoBehaviour
    {
        protected float StartingYPos;
        protected string Label;
        protected int Height = 50;

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

            var xPos = Settings.Instance.WindowXPos + 255;
            var yPos = Settings.Instance.WindowYPos + StartingYPos;
            var rect = new Rect(xPos, yPos, 200, Height);

            GUILayout.Window(824, rect, DrawWindow, Label, style);
        }

        protected virtual void DrawWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 20f));
            GUI.backgroundColor = Color.black;

            GUILayout.BeginVertical();
            CreateControls();
            CreateSaveAndCancelButtons();
            GUILayout.EndVertical();
        }

        protected virtual void CreateControls()
        {
            // to be overriden
        }

        protected virtual void CreateSaveAndCancelButtons()
        {
            GUILayout.BeginHorizontal();
            CreateSaveButton();
            CreateCancelButton();
            GUILayout.EndHorizontal();
        }

        protected virtual void CreateSaveButton()
        {
            if (!GUILayout.Button(ButtonLabel.Save)) return;

            Save();
        }

        protected virtual void CreateCancelButton()
        {
            if (!GUILayout.Button(ButtonLabel.Cancel)) return;
                
            Cancel();
        }

        public virtual void Save()
        {
            CloseWindow();
        }

        protected virtual void Cancel()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            gameObject?.SetActive(false);
        }

        public virtual void SetValue(PositionInfo position)
        {
            // to be overriden
        }

        public virtual void SetValue(RotationInfo rotation)
        {
            // to be overriden
        }

        public virtual void SetValue(TimeInfo time)
        {
            // to be overriden
        }

        public virtual void SetValue(FieldOfViewInfo fov)
        {
            // to be overriden
        }
    }
}
