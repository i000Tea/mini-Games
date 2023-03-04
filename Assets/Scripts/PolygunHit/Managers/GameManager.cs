using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace Tea.PolygonHit
{
	/// <summary>
	/// 游戏管理器
	/// </summary>
    public class GameManager : MonoBehaviour
    {
        #region State 游戏状态
        /// <summary>
        /// 静态自身
        /// </summary>
        public static GameManager inst;
        
        /// <summary>
        /// 暂停状态
        /// </summary>
        public static bool Pause;

        /// <summary>
        /// 游戏速度
        /// </summary>
        private float gameSpeed;
        #endregion

        #region Data 数据

        private int score;

		[SerializeField]
		private Transform GameCanvas;

		#endregion

		#region unity void

		/// <summary>
		/// 
		/// </summary>
		private void Awake()
        {
            inst = this;
            Time.timeScale = 1;
        }
        private void Start()
        {
            SetState(GameState.Gameing);
            GUIManager.inst.CalculationScore(score);
        }
		#endregion

		#region 预计要做成广播的方法

		/// <summary>
		/// 加分
		/// </summary>
		/// <param name="add"></param>
		public void ScoreAdd(int add = 1)
        {
            score += add;
            GUIManager.inst.CalculationScore(score);
        }

        /// <summary>
        /// 设置游戏状态
        /// </summary>
        public void SetState(GameState State)
        {
            switch (State)
            {
                case GameState.Gameing:
                    if (gameSpeed != 0)
                        Time.timeScale = gameSpeed;
                    GUIManager.inst.CanvasSwitch(GameState.Gameing);
                    break;

                case GameState.LevelUp:

                    if (Time.timeScale != 0)
                        gameSpeed = Time.timeScale;
                    Time.timeScale = 0;

                    GUIManager.inst.CanvasSwitch(GameState.LevelUp);
                    break;

                case GameState.Pause:
                    if (Time.timeScale != 0)
                        gameSpeed = Time.timeScale;
                    Time.timeScale = 0;

                    GUIManager.inst.CanvasSwitch(GameState.Pause);
                    break;

                case GameState.Over:
                    GUIManager.inst.CanvasSwitch(GameState.Pause);
                    break;

                default:
                    break;
            }
        }

		#endregion

		#region Button 各种按钮
		/// <summary>
		/// 暂停状态
		/// </summary>
		/// <param name="_pause"></param>
		public void ButtonPause(bool _pause)
        {
            if (_pause)
                SetState(GameState.Pause);
            else
                SetState(GameState.Gameing);
        }
        /// <summary>
        /// 重新开始
        /// </summary>
        public void ButtonReturn()
        {
            SceneManager.LoadScene(1);
        }
        public void ButtonQuit()
        {
            SceneManager.LoadScene(0);
        }
		#endregion
		
		public GameObject TeaInstantiate(GameObject instObj, Vector3 position, float Scale = 1, Transform Parent = null)
		{
			return TeaInstantiate(instObj, position, new Quaternion(0, 0, 0, 0), Scale, Parent);
		}
		public GameObject TeaInstantiate(GameObject instObj, Transform trans, float Scale = 1, Transform Parent = null)
		{
			return TeaInstantiate(instObj, trans.position, trans.rotation, Scale, Parent);
		}
		/// <summary>
		/// 重写的方法 生成物体 生成父集 生成位置 初始大小
		/// </summary>
		/// <param name="instObj"></param>
		/// <param name="Parent"></param>
		/// <param name="setPosition"></param>
		/// <param name="Scale"></param>
		/// <returns></returns>
		public GameObject TeaInstantiate(GameObject instObj, Vector3 setPosition, Quaternion setRotation, float Scale = 1, Transform Parent = null)
		{
			GameObject obj = Instantiate(instObj);

			if (Parent)
				obj.transform.SetParent(Parent);
			else
				obj.transform.SetParent(GameCanvas);

			obj.transform.localScale = Vector3.one * Scale;
			obj.transform.position = setPosition;
			obj.transform.rotation = setRotation;

			return obj; 
		}
	}
}
