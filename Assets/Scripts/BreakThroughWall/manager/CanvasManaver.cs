using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.BreakThroughWall
{
   public class CanvasManaver : Singleton<CanvasManaver>
   {
      #region MyRegion

      [Header("墙简介")]
      [SerializeField] private RectTransform HinderState;

      [SerializeField] private Text HinderName;
      [SerializeField] private Text HinderHP;
      [SerializeField] private Text HinderArom;
      [SerializeField] private Text HinderIntroduction;

      private Vector2 hinderBasePoint;
      [SerializeField] private Vector2 hinderShowPoint;
      [SerializeField] private float hinderShowTime = 0.5f;
      [SerializeField] private Ease hinderShowEase;
      [SerializeField] private float hinderBackTime = 0.5f;
      [SerializeField] private Ease hinderBackEase;
      #endregion

      #region MyRegion

      public Text Atk;
      public Text AtkNum;
      public Text Cost;
      #endregion
      private void Start()
      {
         hinderBasePoint = HinderState.anchoredPosition;
      }
      public void ShowHinderState(string name, string HP, string Arom, string Introduction = "暂无")
      {
         HinderName.text = name;
         HinderHP.text = HP;
         HinderArom.text = Arom;
         HinderIntroduction.text = Introduction;
         HinderState.anchoredPosition = hinderBasePoint;
         HinderState.DOAnchorPos(hinderShowPoint, hinderShowTime).SetEase(hinderShowEase);
      }
      public void BackHinder()
      {
         HinderState.DOAnchorPos(hinderBasePoint, hinderBackTime).SetEase(hinderBackEase);
      }
   }
}
