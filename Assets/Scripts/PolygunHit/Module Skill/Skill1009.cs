using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
    /// <summary>
    /// 地雷
    /// </summary>
    public class Skill1009 : SkillBase
    {
		[Header("地雷")]
		[SerializeField]
		GameObject DiLei;

        public override void UseSkill()
        {
			GameManager.inst.TeaInstantiate(DiLei, PlayerBase.inst.transform.position, 1);
        }
    }
}
