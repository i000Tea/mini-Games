using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tea.CyberCard
{
    public class MenuManager : MonoBehaviour
    {
        #region buttons
        public void ButtonStart(int SceneNum)
        {
            SceneManager.LoadScene(SceneNum);
        }
        public void ButtonExit()
        {
            Application.Quit();
        }

        #endregion
    }
}
