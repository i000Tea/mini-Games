using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
    /// <summary>
    /// 手枪
    /// </summary>
    public class Skill3003 : SkillBase
    {
		[Header("装填")]
		[SerializeField]
		private GameObject CombatDrone;
		private Skill3003_CombatDrone m_CombatDrone;

		private float fireDetection;
		/// <summary>
		/// 开火冷却
		/// </summary>
		[SerializeField]
		private float expect = 2;
		private void Start()
		{
			var a = GameManager.inst.TeaInstantiate
				(CombatDrone,PlayerBase.Player.position);
			a.transform.position += new Vector3(
				Random.Range(-0.3f, 0.3f),
				Random.Range(-0.3f, 0.3f),
				0);
			m_CombatDrone = a.GetComponent<Skill3003_CombatDrone>();
		}

		private void FixedUpdate()
		{
			// 开火冷却已就绪
			if (fireDetection >= expect)
			{
				//
				if (PlayerBase.inst.m_Ammo >= 3 && m_CombatDrone.IsShoot())
				{
					PlayerBase.inst.UseAmmo(2);
					fireDetection = 0;
				}
			}
			else
				fireDetection += Time.deltaTime;
		}
		public override void UseSkill()
        {
			m_CombatDrone.SetTarget();
		}
    }
}
