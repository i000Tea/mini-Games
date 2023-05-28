using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	=
/// </summary>
[CreateAssetMenu(menuName = "WallData", fileName = "WallData")]
[System.Serializable]
public class WallDataList : ScriptableObject
{
   public List<WallData> WallDatas;
   public WallData RandomData()
   {
      var value = Random.Range(0, WallDatas.Count);
      return WallDatas[value];
   }
}
[System.Serializable]
public class WallData
{
   public string Name;

   /// <summary> 耐久度 </summary>
   public float durability = 1;
   [Range(1, 10)] public float durMulit = 1;

   /// <summary> 护甲 </summary>
   public float armor = 0;
   [Range(1, 10)] public float armMulit = 1;
}