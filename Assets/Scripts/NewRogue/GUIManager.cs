using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tea.NewRouge
{
	public class GUIManager : Singleton<GUIManager>
	{
		#region 变量
		[SerializeField]
		private GameObject canvas_Start;
		[SerializeField]
		private GameObject canvas_Playing;
		[SerializeField]
		private GameObject canvas_Pause;
		[SerializeField]
		private GameObject canvas_Over;

		#region StartSelect
		[Header("开始前配置")]
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

		[Header("游戏中")]
		[SerializeField]
		private Text KeycardText;

		[Header("Health")]
		[SerializeField]
		private Transform playerLifebar;
		private float lifebarMaxX;
		[SerializeField]
		private Text playerHealthText;
		#endregion


		protected override void Awake()
		{
			if (playerLifebar)
			{
				lifebarMaxX = (playerLifebar as RectTransform).sizeDelta.x;
			}
			base.Awake();
		}
		private void Start()
		{
			Button_ChangeSelect_Player();
			Game2Manager.I.OnGameStart += GameStart;
			Game2Manager.I.OnPlayerDead += GameOver;
		}

		#endregion

		#region OpenSelect 开始时 选择页面

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
		#endregion

		#region Gameing 游戏中
		void GameStart()
		{
			canvas_Start.SetActive(false);
			canvas_Playing.SetActive(true);
		}
		public void SetKeycord(int num)
		{
			if (KeycardText)
				KeycardText.text = num.ToString();
		}
		public void SetHealth(int num, int? maxNum = null)
		{
			string nText = num.ToString();
			if (maxNum != null)
			{
				nText += "/" + maxNum;
				(playerLifebar as RectTransform).sizeDelta = new Vector2((float)num / (float)maxNum * lifebarMaxX,
					(playerLifebar as RectTransform).sizeDelta.y);
			}
			playerHealthText.text = nText;
		}
		/// <summary>
		/// 设置暂停状态
		/// </summary>
		/// <param name="isPause"></param>
		public void SetPause(bool isPause)
		{
			Debug.LogWarning(gameObject);
			Debug.Log(canvas_Pause);
			canvas_Pause.SetActive(isPause);
		}

		#endregion

		#region Over Setting 结束后设置
		void GameOver()
		{
			canvas_Playing.SetActive(false);
			canvas_Over.SetActive(true);
		}
		public void Button_Setting_()
		{

		}
		public void Button_Setting_Return()
		{
			SceneManager.LoadScene(2);
		}
		public void Button_Setting_Exit()
		{
			SceneManager.LoadScene(0);
		}
		#endregion
	}
}
