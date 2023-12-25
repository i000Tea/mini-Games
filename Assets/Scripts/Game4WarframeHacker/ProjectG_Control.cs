using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Tea.Warframe
{
   public class ProjectG_Control : Singleton<ProjectG_Control>
   {
      #region v
      private Difficulty _difficulty;
      [SerializeField] private Transform RimParent;
      [SerializeField] List<Transform> touthObj;
      List<bool> touthState;

      [SerializeField] private Transform pointer;
      private float rotate;
      private float TargetNum
      {
         get => targetNum;
         set
         {
            Debug.Log(value);
            targetNum = (int)value;
            for (int i = 0; i < touthObj.Count; i++)
            {
               if (i == targetNum)
               {
                  BG(touthObj[i]).enabled = true;
               }
               else
               {
                  BG(touthObj[i]).enabled = false;
               }
            }
         }
      }
      private int targetNum;
      [SerializeField] private float rotateSpeed = 0.1f;
      #endregion
      #region unity

      private void OnValidate()
      {
         Initialize();
      }
      private void Start()
      {
         Initialize();
      }
      private void Update()
      {
         if (Input.GetKeyDown(KeyCode.Space)) { PointerTouth(); }
      }
      #endregion
      private void Initialize(Difficulty difficulty = Difficulty.none)
      {
         if (difficulty != Difficulty.none)
         {
            _difficulty = difficulty;
         }
         touthObj = new List<Transform>();
         touthState = new List<bool>();

         for (int i = 0; i < RimParent.childCount; i++)
         {
            var obj = RimParent.GetChild(i);
            obj.transform.localEulerAngles = new Vector3(0, 0, 45 * i);
            touthObj.Add(obj);
            touthState.Add(false);
            BG(obj).enabled = false;
         }
      }
      private void FixedUpdate()
      {
         PointerRotate(rotateSpeed);
      }
      void PointerRotate(float angle)
      {
         var cache = rotate + angle;

         cache += 22.5f;
         if (cache > 360) { cache %= 360; }
         while (cache < 0) { cache += 360; }
         TargetNum = (cache / 45);
         rotate = cache - 22.5f;
         pointer.localEulerAngles = new Vector3(0, 0, rotate);
      }
      void PointerTouth()
      {
         var rect = touthObj[targetNum].GetChild(0).GetChild(1).transform as RectTransform;
         var v2pts = rect.anchoredPosition;

         touthState[targetNum] = !touthState[targetNum];
         if (touthState[targetNum])
         {
            v2pts.x = 80f; 
            touthObj[targetNum].GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);
         }
         else
         {
            v2pts.x = 12.5f;
            touthObj[targetNum].GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);
         }
         rect.DOAnchorPos(v2pts, 0.2f).SetEase(Ease.OutBack);
      }
      private Image BG(Transform baseTrans)
      {
         var target = baseTrans.GetChild(0).GetChild(0).GetChild(0);
         if (target.TryGetComponent(out Image image)) { return image; }
         else { return null; }
      }
   }
}
