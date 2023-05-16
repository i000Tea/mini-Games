using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
    /// <summary>
    /// 强力击
    /// </summary>
    public class Skill1002 : SkillBase
    {
		[Header("震荡")]
		[Tooltip("倍率")]
		[SerializeField]
		[Range(1,10)]
		private float m_Multiplying = 1;
		public override void UseSkill()
        {
			// 使用技能时 增加一个单次撞击的
			var newBuff = new BuffOnceStrike();
			newBuff = (BuffOnceStrike)PlayerBase.I.AddBuff(newBuff);
			GC.Collect();
			newBuff.mult += m_Multiplying;
		}
    }
}
