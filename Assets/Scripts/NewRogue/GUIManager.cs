using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.NewRouge
{
	public class GUIManager : Singleton<GUIManager>
	{
		[SerializeField]
		private GameObject gameingCanvas;

		#region StartSelect
		[SerializeField]
		GameObject startCanvas;

		[SerializeField]
		Text selectNum;

		[SerializeField]
		GameObject bPlayer;
		[SerializeField]
		GameObject PObj;

		[Space(10)]
		[SerializeField]
		GameObject bWeapon;
		[SerializeField]
		GameObject WObj;
		#endregion

		#region gameing

		[SerializeField]
		private Text KeycardText;
		[SerializeField]
		private Text playerHealthText;
		#endregion

		[SerializeField]
		private GameObject pauseCanvas;

		private void Start()
		{
			Button_ChangeSelect_Player();
		}

		#region OpenSelect
		bool selectWeapon;
		public void Button_ChangeSelect_Player()
		{
			bPlayer.SetActive(false);
			PObj.SetActive(true);

			bWeapon.SetActive(true);
			WObj.SetActive(false);

			selectWeapon = false;
			selectNum.text = OpenSelectManager.I.ChangeSelect(0, selectWeapon).ToString();
		}
		public void Button_ChangeSelect_Weapon()
		{
			bPlayer.SetActive(true);
			PObj.SetActive(false);

			bWeapon.SetActive(false);
			WObj.SetActive(true);

			selectWeapon = true;
			selectNum.text = OpenSelectManager.I.ChangeSelect(0, selectWeapon).ToString();
		}

		public void Button_SelectNext()
		{
			selectNum.text = OpenSelectManager.I.ChangeSelect(1, selectWeapon).ToString();
		}
		public void Button_SelectBefore()
		{
			selectNum.text = OpenSelectManager.I.ChangeSelect(-1, selectWeapon).ToString();
		}
		public void Button_StartGame()
		{
			startCanvas.SetActive(false);
			gameingCanvas.SetActive(true);

			GameManager.I.StartGame();
			OpenSelectManager.I.StartGame();
		}
		#endregion

		#region Gameing
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

		#endregion
	}
}
