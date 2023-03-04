using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	保存Buff的各项数据，如最高等级、持续时间、效果值等
/// </summary>
[CreateAssetMenu(menuName = "Skill", fileName = "SkillData")]
[System.Serializable]
public class SkillData : ScriptableObject
{
	public string buffDataName;
	public List<Skill> skills;
}
[System.Serializable]

public class Skill
{
	public int id;
	public string buffName;
	public GameObject instObj;

}
