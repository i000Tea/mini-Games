using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	技能 用于转换Json的数据表格
/// </summary>
[CreateAssetMenu(menuName = "Skill", fileName = "SkillData")]
[System.Serializable]
public class AllSkillData : ScriptableObject
{
   public List<SkillData> skillList;
   // 创建一个新的ScriptableObject实例
   public static AllSkillData CreateNewSkillData()
   {
      return CreateInstance<AllSkillData>();
   }
}
[System.Serializable]

public class SkillData
{
   public string skillName;
   /// <summary> 简介/描述 </summary>
   public string description;
   /// <summary> 简介/描述 </summary>
   public string skillClassName;
   public int id;
}
