using ReplayEditor;
using UnityEngine;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class OffsetKeyframesUI : EditBaseUI
    {
        public float offset { get; set; }

        private string offsetString = "0";

        protected override void OnGUI()
        {
            StartingYPos = 110;
            Label = WindowLabel.EditTime;

            base.OnGUI();
        }

        public override void Save()
        {
            if (!float.TryParse(offsetString, out var newOffset)) return;

            var camController = ReplayEditorController.Instance.cameraController;
            foreach (var keyframe in camController.keyFrames)
            {
                keyframe.time += newOffset;
            }

            camController.keyframeUI.UpdateKeyframes(camController.keyFrames);

            base.Save();
        }

        protected override void CreateControls()
        {
            GUILayout.BeginVertical();

            offsetString = CreateFloatField(FieldLabel.Offset, offsetString);

            GUILayout.EndVertical();
        }
    }
}
