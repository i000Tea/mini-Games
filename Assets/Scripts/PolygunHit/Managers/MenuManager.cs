using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tea.CyberCard
{
    public class MenuManager : MonoBehaviour
    {
		private void Start()
		{
			Time.timeScale = 1;
		}
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
