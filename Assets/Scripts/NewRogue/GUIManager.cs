using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.NewRouge
{
	public class GUIManager : Singleton<GUIManager>
	{
		[SerializeField]
		private GameObject pauseCanvas;
		[SerializeField]
		private Text GetKeycard;
		public void UpdateKeycord(int num)
		{
			GetKeycard.text = num.ToString();
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
