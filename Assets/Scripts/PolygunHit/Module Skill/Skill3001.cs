using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
    /// <summary>
    /// 震荡
    /// </summary>
    public class Skill3001 : SkillBase
    {
		[Header("装填")]
		public int num;
		public override void UseStrikeSkill(EnemyBase enemy)
		{
			//Debug.Log("装填");
			PlayerBase.inst.AddAmmo(num);
        }
    }
}
