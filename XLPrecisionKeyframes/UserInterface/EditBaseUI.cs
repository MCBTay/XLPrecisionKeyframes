using System.Linq;
using UnityEngine;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditBaseUI : MonoBehaviour
    {
        private bool HasKeyframes => UserInterface.keyFrames != null && UserInterface.keyFrames.Any();
        private bool HasKeyframeName => !string.IsNullOrEmpty(UserInterface.currentKeyframeName);

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
    }
}
