using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.NewRouge
{
	public delegate void BaseEvent();
	public class Game2Manager : Singleton<Game2Manager>
	{
		public event BaseEvent OnGameStart;
		public event BaseEvent OnPlayerDead;
		enum GameState
		{
			menu,
			playing,
			pause,
		}
		GameState gState = GameState.playing;
		float gameSpeed;

		private void Start()
		{
			Debug.Log("start");
			Room_FloorManager.I.StartCreate();
		}
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Stage_Pause();
		}

		public void Stage_GameStart()
		{
			OnGameStart();
		}
		public void Stage_Pause()
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

		public void Stage_GameOver()
		{
			OnPlayerDead();
		}
	}
}
