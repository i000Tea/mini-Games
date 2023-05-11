using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
    /// <summary>
    /// 封锁 原冰冻
    /// </summary>
    public class Skill1003 : SkillBase
    {
		[Header("封锁")]

		[Tooltip("封锁特效")]
		[SerializeField]
		private GameObject m_LatchObj;

		[Tooltip("范围封锁粒子")]
		[SerializeField]
		private GameObject DiffParticle;

		[Tooltip("封锁概率")]
		[SerializeField]
		[Range(1, 100)]
		private float m_Percent = 30;
		[Tooltip("扩散概率")]
		[SerializeField]
		[Range(1, 100)]
		private float m_DiffPercent = 15;
		[Tooltip("封锁扩散范围")]
		[SerializeField]
		[Range(1, 10)]
		private float m_DiffScale = 1;
		[Tooltip("封锁时间")]
		[SerializeField]
		[Range(1, 10)]
		private float m_LatchTime = 1;
		public override void UseStrikeSkill(EnemyBase enemy)
        {
			// 判断是否冻结
			if(m_Percent > Random.Range(0, 100))
			{
				//判断冻结是否范围生效
				if (m_DiffPercent > Random.Range(0, 100))
				{
					var a =	AddVoids.ListDistance(EnemyManager.nowEnemys, PlayerBase.Player.position, m_DiffScale);

					var c = new UnCollision();
					c.Power = 1;
					c.Target = PlayerBase.Player.position;

					for (int i = 0; i < a.Length; i++)
					{
						AddLatch(a[i].transform);
						a[i].GetComponent<EnemyBase>().UnCollision(c);
					}

					ParticleManager.InstParticle(DiffParticle, PlayerBase.Player.position);
					Debug.Log("冻结范围敌人");
				}
				else
				{
					AddLatch(enemy.transform);
					Debug.Log("冻结单个敌人");
				}
			}
			Debug.Log("判定失败");
		}

		/// <summary>
		/// 增加封锁
		/// </summary>
		/// <param name="getEnemy"></param>
		void AddLatch(Transform getEnemy)
		{
			var a = Instantiate(m_LatchObj);
			a.transform.SetParent(getEnemy);
			a.GetComponent<Skill1003_Latch>().
				StartLatch(m_LatchTime,Random.Range(1.5f,3.5f));
		}
    }
}
