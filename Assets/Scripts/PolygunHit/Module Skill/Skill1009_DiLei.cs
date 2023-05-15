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
	}
}
