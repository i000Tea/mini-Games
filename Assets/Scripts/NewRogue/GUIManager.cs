using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.NewRouge
{
	public class GUIManager : Singleton<GUIManager>
	{
		[SerializeField]
		private Text KeycardText;
		[SerializeField]
		private Text playerHealthText;
		[SerializeField]
		private GameObject pauseCanvas;
		public void SetKeycord(int num)
		{
			if (KeycardText)
				KeycardText.text = num.ToString();
		}
		public void SetHealth(int num, int? maxNum = null)
		{
			string nText = num.ToString();
			if (maxNum != null)
				nText += "/" + maxNum;
			playerHealthText.text = nText;
		}
		/// <summary>
		/// 设置暂停状态
		/// </summary>
		/// <param name="isPause"></param>
		public void SetPause(bool isPause)
		{
			Debug.LogWarning(gameObject);
			Debug.Log(pauseCanvas);
			pauseCanvas.SetActive(isPause);
		}
	}
}
