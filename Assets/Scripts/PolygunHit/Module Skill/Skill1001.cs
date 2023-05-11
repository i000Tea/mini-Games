using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
    /// <summary>
    /// 震荡
    /// </summary>
    public class Skill1001 : SkillBase
    {
		[Header("震荡")]
        [SerializeField]
		GameObject m_Particle;
		[Tooltip("生效距离")]
        [SerializeField]
        float m_long = 3;
        [Tooltip("击退强度")]
        [SerializeField]
        float m_power = 3;
		public override void UseStrikeSkill(EnemyBase _)
        {
			// 获取攻击范围内的敌人
			var enemy = AddVoids.ListDistance(EnemyManager.nowEnemys, PlayerBase.Player.position, m_long);
			// 对获取到的敌人 攻击1 击退
			enemy.AtkEnemys(1,AddVoids.SetUnColl(m_power, PlayerBase.Player.position));
			// 击退粒子
			ParticleManager.InstParticle(m_Particle, PlayerBase.Player.position);
		}
    }
}
