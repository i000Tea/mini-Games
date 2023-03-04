using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tea.CyberCard
{
    public class MenuManager : MonoBehaviour
    {
        #region buttons
        public void ButtonStart()
        {
            SceneManager.LoadScene(1);
        }
        public void ButtonExit()
        {
            Application.Quit();
        }

        #endregion
    }
}
