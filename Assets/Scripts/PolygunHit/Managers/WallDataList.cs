using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Tea.BreakThroughWall;
using Tea.PolygonHit;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

/// <summary>
///	=
/// </summary>
[CreateAssetMenu(menuName = "WallData", fileName = "WallData")]
[System.Serializable]
public class WallDataList : ScriptableObject
{
   [FormerlySerializedAs("WallDatas")]
   public List<HinderWallData> HinderWallDatas;
   public List<AwardWallData> AwardWallDatas;
   public HinderWallData RandomHData()
   {
      var value = Random.Range(0, HinderWallDatas.Count);
      return HinderWallDatas[value];
   }
   public AwardWallData RandomAData()
   {
      var value = Random.Range(0, AwardWallDatas.Count);
      return AwardWallDatas[value];
   }
}
public abstract class BaseWallData
{
   public string Name;
   public string introduction;
}
[System.Serializable]
public class HinderWallData : BaseWallData
{
   public float durability => baseDurability + baseDurability * durMulit * InterControl.Difficulty;
   /// <summary> 耐久度 </summary>
   [SerializeField] private float baseDurability = 1;
   [SerializeField][Range(1, 10)] public float durMulit = 1;

   /// <summary> 护甲 </summary>
   public float armor = 0;
   [Range(1, 10)] public float armMulit = 1;
}
[System.Serializable]
public class AwardWallData : BaseWallData
{
   public float atk;
   public bool atkMulit;
   public float atkNum;
   public float cost;
}

[System.Serializable]
public class ShowAndBackTweenDataGroup
{
   public TweenDataGroup ShowAnim;
   public TweenDataGroup BackAnim;
}
[System.Serializable]
public class TweenDataGroup
{
   public float AnimLength = -1;
   public float WaitTime = 0.5f;
   public float AnimTime = 0.5f;
   public Ease AnimEase = Ease.OutExpo;
}