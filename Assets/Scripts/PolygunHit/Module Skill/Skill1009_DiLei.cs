using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tea;

namespace Tea.PolygonHit
{
    /// <summary>
    /// 震荡
    /// </summary>
    public class Skill1009_DiLei : MonoBehaviour
    {
		[SerializeField]
		private GameObject m_Particle;

		[Tooltip("触发器大小倍率")]
		[Range(0.5f,10)]
		public float Scale;
		[Tooltip("爆炸生效范围")]
		[Range(1, 10)]
		public float BoomScale;
		[Tooltip("爆炸生效范围")]
		[Range(1, 20)]
		public float ShockScale;
		[Tooltip("伤害")]
		public float m_Dmg;
		[Tooltip("击退力度")]
		public float m_Power;

		private void OnValidate()
		{
			GetComponent<CircleCollider2D>().radius = 50 * Scale * 2;
		}
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.tag == "Enemy")
			{
				Debug.Log( Vector3.Distance(transform.position, collision.transform.position));
				// 获取攻击范围内的敌人
				var enemy = AddVoids.ListDistance(EnemyManager.enemys, transform.position, BoomScale);

				// 爆炸伤害和击退
				enemy.AtkEnemys(m_Dmg, AddVoids.SetUnColl(m_Power, PlayerBase.Player.position));

				//// 设置震荡范围
				//enemy = AddVoid.ListDistance(EnemyManager.enemys, transform.position, ShockScale);
				//// 震荡范围内额外受一次冲击
				//enemy.AtkEnemys(AddVoid.SetUnColl(1, PlayerBase.Player.position));

				ParticleManager.InstParticle(m_Particle, transform.position);
				Destroy(gameObject);
			}
		}
	}
}
