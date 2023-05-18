using O3DWB;
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

      #region Canvas
      [SerializeField]
      private GameObject _Awake;
      [SerializeField]
      GameObject[] showCanvas;
      #endregion

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
      private TeaSkillButton[] skillButtons;
      #endregion

      #endregion

      #region 广播
      protected override void AddDelegate()
      {
         EventControl.OnAddButtonList(ButtonType.Menu_StartGame, GameStart);

         EventControl.OnAddGameStateList(CanvasSwitch);
      }
      protected override void RemoveDelegate()
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
         //Debug.Log("游戏开始");
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
         //Debug.Log($"{state}?");
         for (int i = 1; i < showCanvas.Length; i++)
         {
            showCanvas[i].SetActive(false);
         }

         showCanvas[(int)state].SetActive(true);
      }

      #endregion

      #region Skill
      /// <summary>
      /// 将输入的技能列表给到按键上
      /// </summary>
      /// <param name="datas"></param>
      public void InputSkillData(SkillData[] datas)
      {
         var btns = GetSkillButtonList(datas.Length);

         for (int i = 0; i < datas.Length; i++)
         {
            btns[i].InputSkillData(datas[i]);
         }
      }
      /// <summary>
      /// 传入需要数量 获取技能列表
      /// </summary>
      /// <param name="maxNum"></param>
      public TeaSkillButton[] GetSkillButtonList(int maxNum = 0)
      {
         // 若数组为空 创建数组
         if (skillButtons == null)
         {
            CreateButtonList(maxNum);
         }
         // 若数组大小小于需求 创建数组
         else if (skillButtons.Length < maxNum)
         {
            CreateButtonList(maxNum);
         }
         for (int i = maxNum + 1; i < skillButtons.Length; i++)
         {
            skillButtons[i].gameObject.SetActive(false);
         }
         for (int i = 0; i < maxNum; i++)
         {
            skillButtons[i].gameObject.SetActive(true);
         }

         return skillButtons;
      }

      /// <summary>
      /// 创建按钮列表
      /// </summary>
      /// <param name="maxNum"></param>
      public void CreateButtonList(int maxNum)
      {
         // 获取需要增加的数量
         var AddNum = maxNum - SkillParent.childCount;
         // 若需要增加 则开始创建新对象
         if (AddNum > 0)
         {
            // 设置基准对象为0号元素
            var createBase = SkillParent.GetChild(0);
            for (int i = 0; i < AddNum; i++)
            {
               Instantiate(createBase.gameObject, createBase.parent);
            }
         }
         // 初始化数组
         skillButtons = new TeaSkillButton[SkillParent.childCount];
         // 将所有对象添加到数组中
         for (int i = 0; i < SkillParent.childCount; i++)
         {
            skillButtons[i] = SkillParent.GetChild(i).GetComponent<TeaSkillButton>();
         }
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
