using UnityEngine;

namespace Game.Utils.Core
{
    public class ApplicationQuit : MonoBehaviour
    {
        public void QuitApplication()
        {
            Debug.Log("Quitting Application");
            Application.Quit();
        }
    }
}
