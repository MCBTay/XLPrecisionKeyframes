using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface
{
    public class UserInterfacePopup<T> where T : EditBaseUI
    {
        private readonly GameObject gameObject;
        private readonly T ui;

        public UserInterfacePopup()
        {
            gameObject = new GameObject();
            gameObject.SetActive(false);
            ui = gameObject.AddComponent<T>();
            Object.DontDestroyOnLoad(gameObject);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(gameObject);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        public void Show(PositionInfo position)
        {
            Show();
            ui.SetValue(position);
        }

        public void Show(RotationInfo rotation)
        {
            Show();
            ui.SetValue(rotation);
        }

        public void Show(TimeInfo time)
        {
            Show();
            ui.SetValue(time);
        }

        public void Show(FieldOfViewInfo fov)
        {
            Show();
            ui.SetValue(fov);
        }
    }
}
