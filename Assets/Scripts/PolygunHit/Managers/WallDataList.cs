using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
[System.Serializable]
public class HinderWallData
{
   public string Name;

   /// <summary> 耐久度 </summary>
   public float durability = 1;
   [Range(1, 10)] public float durMulit = 1;

   /// <summary> 护甲 </summary>
   public float armor = 0;
   [Range(1, 10)] public float armMulit = 1;
}
[System.Serializable]
public class AwardWallData
{
   public string Name;


}