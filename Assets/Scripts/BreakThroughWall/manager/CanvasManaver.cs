using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.BreakThroughWall
{
   public class CanvasManaver : Singleton<CanvasManaver>
   {
      #region MyRegion
      bool aIsOpen, hIsOpen;
      Tween aTwing;
      Tween hTwing;

      [Header("墙简介")]
      [SerializeField] private RectTransform HinderState;
      [SerializeField] private Text HinderName;
      [SerializeField] private Text HinderProperty;
      [SerializeField] private Text HinderIntroduction;

      [SerializeField] private ShowAndBackTweenDataGroup hTweenState;
      private Vector2 hinderBasePoint;

      [Header("墙简介")]
      [SerializeField] private RectTransform AwardState;

      [SerializeField] private Text AwardName;
      [SerializeField] private Text AwardProperty;
      [SerializeField] private Text AwardIntroduction;

      [SerializeField] private ShowAndBackTweenDataGroup aTweenState;
      private Vector2 AwardBasePoint;
      #endregion

      #region MyRegion

      public Text Atk;
      public Text AtkNum;
      public Text Cost;

      [SerializeField] RectTransform GameOverCanvas;
      [SerializeField] Text EndWallKillText;
      #endregion
      private void Start()
      {
         hinderBasePoint = HinderState.anchoredPosition;
         AwardBasePoint = AwardState.anchoredPosition;
      }
      private void Update()
      {
         if (Input.GetMouseButtonUp(0))
         {
            _ = StartCoroutine(WaitDele());
         }
      }
      IEnumerator WaitDele()
      {
         yield return new WaitForFixedUpdate();
         if (aIsOpen) { BackAward(); }
         if (hIsOpen) { BackHinder(); }
      }
      public void ShowHinderState(string name, string HP = "", string Arom = "", string Introduction = "暂无")
      {
         KillTween(hTwing);
         HinderName.text = name;
         HinderProperty.text = $"生命:{HP} 护甲:{Arom}";
         HinderIntroduction.text = Introduction;

         HinderState.anchoredPosition = hinderBasePoint;
         hTwing = HinderState.DOAnchorPos(new Vector2(0, hTweenState.ShowAnim.AnimLength),
            hTweenState.ShowAnim.AnimTime).SetEase(hTweenState.ShowAnim.AnimEase);
         hIsOpen = true;
      }
      public void BackHinder()
      {
         KillTween(hTwing);
         hTwing = HinderState.DOAnchorPos(hinderBasePoint,
            hTweenState.BackAnim.AnimTime).SetEase(hTweenState.BackAnim.AnimEase);
         hIsOpen = false;
      }
      public void ShowAwardState(string name, string Property = "", string Introduction = "暂无")
      {
         KillTween(aTwing);
         AwardName.text = name;
         AwardProperty.text = Property;
         AwardIntroduction.text = Introduction;

         AwardState.anchoredPosition = AwardBasePoint;
         aTwing = AwardState.DOAnchorPos(new Vector2(0, aTweenState.ShowAnim.AnimLength),
            aTweenState.ShowAnim.AnimTime).SetEase(aTweenState.ShowAnim.AnimEase);
         aIsOpen = true;
      }
      public void BackAward()
      {
         KillTween(aTwing);
         aTwing = AwardState.DOAnchorPos(hinderBasePoint,
            hTweenState.BackAnim.AnimTime).SetEase(hTweenState.BackAnim.AnimEase);
         aIsOpen = false;
      }

      void KillTween(Tween tween)
      {
         if (tween != null)
         {
            if (tween.active) { DOTween.Kill(tween); }
         }
      }

      public void GameOver()
      {
         GameOverCanvas.gameObject.SetActive(true);
         EndWallKillText.text = InterControl.I.KillWallNum.ToString();
      }
   }
}
