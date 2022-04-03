using UnityEngine;
using UnityModManagerNet;

namespace XLPrecisionKeyframes
{
    public class UserInterface
    {
        private static UserInterface __instance;
        public static UserInterface Instance => __instance ?? (__instance = new UserInterface());

        public void Create()
        {

        }

        public void Destroy()
        {

        }

        public void Update(Transform cameraTransform)
        {
            UnityModManager.Logger.Log("XLPK: Camera position: " + cameraTransform.position);
            UnityModManager.Logger.Log("XLPK: Camera rotation: " + cameraTransform.rotation);
        }
    }
}
