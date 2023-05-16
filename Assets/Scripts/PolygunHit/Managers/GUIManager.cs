using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

namespace Tea.PolygonHit
{
   public class GUIManager : Singleton<GUIManager>
   {
      #region 变量
      [SerializeField]
      private GameObject _Awake;
      [SerializeField]
      GameObject[] showCanvas;

      #region cache Date
      float cacheMaxExp;
      #endregion

      #region Drag setting 绘制设置
      [Header("PlayingSetting")]

      [SerializeField]
      private Image exaImage;

      [SerializeField]
      private Text levelText;

      [SerializeField]
      private Text scoreText;

      [SerializeField]
      private Text HealthText;


      [Header("PlayingSetting")]
      [SerializeField]
      private Transform SkillParent;
      #endregion

      #endregion

      #region 广播
      protected override void AddDelegate()
      {
         EventControl.OnAddButtonList(ButtonType.Menu_StartGame, GameStart);

         EventControl.OnAddGameStateList(CanvasSwitch);
      }
      protected override void Removedelegate()
      {
         EventControl.OnRemoveButtonList(ButtonType.Menu_StartGame, GameStart);

         EventControl.OnRemoveGameStateList(CanvasSwitch);
      }
      #endregion
      protected override void Awake()
      {
         base.Awake();      
         for (int i = 1; i < showCanvas.Length; i++)
         {
            showCanvas[i].SetActive(false);
         }
      }
      private void GameStart()
      {
         Debug.Log("游戏开始");
         _Awake.SetActive(false);
         CanvasSwitch(GameState.Gameing);
      }

      #region Calculation ui更新
      public void LevelUpdate(int level)
      {
         GUIUpdate(level);
      }
      public void ExpUpdate(float nowExp, float maxExp)
      {
         GUIUpdate(nowExp: nowExp, maxExp: maxExp);
      }
      public void HealthUpdate(float health, float healthMax = default)
      {
         GUIUpdate(health: (int)health);
      }
      public void GUIUpdate(int level = -1, float nowExp = -1, float maxExp = -1, int health = -1)
      {
         if (nowExp != -1 && maxExp != -1)
         {
            exaImage.fillAmount = nowExp / maxExp;
         }
         if (level != -1)
         {
            levelText.text = level.ToString();
         }
         if (health != -1)
         {
            HealthText.text = health.ToString();
         }
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
      private void CanvasSwitch(GameState state)
      {
         for (int i = 1; i < showCanvas.Length; i++)
         {
            showCanvas[i].SetActive(false);
         }

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
