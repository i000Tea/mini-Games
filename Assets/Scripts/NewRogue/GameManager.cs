using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.NewRouge
{
	public class GameManager : Singleton<GameManager>
	{
		enum GameState
		{
			menu,
			playing,
			pause,
		}
		GameState gState = GameState.playing;
		float gameSpeed;
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				PauseButton();
		}
		public void PauseButton()
		{
			switch (gState)
			{
				// 游戏状态下 功能为暂停
				case GameState.playing:
					gState = GameState.pause;
					GUIManager.I.SetPause(true);
					gameSpeed = Time.timeScale;
					Time.timeScale = 0;
					break;

				// 暂停状态下 功能为恢复
				case GameState.pause:
					gState = GameState.playing;
					GUIManager.I.SetPause(false);
					Time.timeScale = gameSpeed;
					break;
				default:
					break;
			}
		}
		private void OnApplicationPause(bool pause)
		{

		}
	}
}
