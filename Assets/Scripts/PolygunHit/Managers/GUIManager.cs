using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.PolygonHit
{
   public class GUIManager : Singleton<GUIManager>
   {
      #region 变量
      [SerializeField]
      private GameObject _Awake;
      [SerializeField]
      GameObject[] showCanvas;

      #region Drag setting
      [Header("PlayingSetting")]

      [SerializeField]
      private Image exaImage;

      [SerializeField]
      private Text levelText;

      [SerializeField]
      private Text scoreText;

      [SerializeField]
      private Text HealthText;
      #endregion

      #endregion

      #region 广播

      protected override void AddDelegate()
      {
         EventControl.OnAddButtonList(ButtonType.Menu_StartGame, GameStart);
      }
      protected override void Removedelegate()
      {
         EventControl.OnRemoveButtonList(ButtonType.Menu_StartGame, GameStart);
      }
      #endregion

      private void GameStart()
      {
         Debug.Log("游戏开始");
         _Awake.SetActive(false);
         CanvasSwitch(GameState.Gameing);
      }
      #region Calculation ui更新

      /// <summary>
      /// 玩家界面UI更新
      /// </summary>
      /// <param name="level"></param>
      /// <param name="now"></param>
      /// <param name="max"></param>
      /// <param name="health"></param>
      public void PlayerMessageUpdate(int level, int now, int max, int health)
      {
         exaImage.fillAmount = (float)now / (float)max;
         levelText.text = level.ToString();
         HealthText.text = health.ToString();
      }

      public void GUIUpdate(int level=default, int now = default, int max = default, int health = default)
      {

      }

      /// <summary>
      /// 显示更新后的分数
      /// </summary>
      /// <param name="score"></param>
      public void CalculationScore(int score)
      {
         scoreText.text = score.ToString();
      }

      /// <summary>
      /// 切换Canvas显示
      /// </summary>
      /// <param name="state"></param>
      public void CanvasSwitch(GameState state)
      {
         for (int i = 1; i < showCanvas.Length; i++)
            showCanvas[i].SetActive(false);

         showCanvas[(int)state].SetActive(true);
      }

      #endregion

      #region SpecialSystems 特殊系统

      #endregion

      #region Buttons
      /// <summary>
      /// 上一个角色
      /// </summary>
      public void MenuButton_BeforeCharacter()
      {

      }
      /// <summary>
      /// 下一个角色
      /// </summary>
      public void MenuButton_AfterCharacter()
      {

      }
      #endregion
   }
}
