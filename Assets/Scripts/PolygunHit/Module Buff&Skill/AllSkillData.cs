using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	技能 用于转换Json的数据表格
/// </summary>
[CreateAssetMenu(menuName = "Skill", fileName = "AllSkillData")]
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
   /// <summary> 简介/描述 </summary>
   public string skillName;
   /// <summary> 简介/描述 </summary>
   public string description;
   /// <summary> 技能对应类名 </summary>
   public string skillClassName;
   /// <summary> 技能编号 </summary>
   public int skillSN;
}
