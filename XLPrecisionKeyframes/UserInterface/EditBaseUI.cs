using UnityEngine;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditBaseUI : MonoBehaviour
    {
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
