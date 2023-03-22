using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tea.NewRouge
{
	public class Enemy_Control : MonoBehaviour
	{
		public float health = 10;

		#region ATK
		/// <summary>
		/// 与玩家之间的距离
		/// </summary>
		float DistanceFromPlayer;
		/// <summary>
		/// 攻击距离
		/// </summary>
		public float strikingDistance = 1;
		public int baseDamage;
		/// <summary>
		/// 攻击冷却
		/// </summary>
		bool AtkCD;
		#endregion

		int valueKeyCard = 1;

		public GameObject dieParticle;

		/// <summary>
		/// 受攻击的点位
		/// </summary>
		[SerializeField]
		Transform unHitPoint;

		/// <summary>
		/// 准备攻击
		/// </summary>
		/// <param name="length"></param>
		public void ReadyAtk(float length)
		{
			DistanceFromPlayer = length;
			if (DistanceFromPlayer < strikingDistance && !AtkCD)
			{
				StartCoroutine(Atk());
			}
		}
		/// <summary>
		/// 攻击
		/// </summary>
		/// <returns></returns>
		IEnumerator Atk()
		{
			AtkCD = true;
			yield return 1;
			if (DistanceFromPlayer < strikingDistance)
			{
				Player_Control.I.UnHit(baseDamage);
			}
			AtkCD = false;
		}
		public void Startsetting()
		{

		}
		public void UnHit(float damage = 1)
		{
			health -= damage;
			if (health <= 0)
			{
				GetComponent<EnemyMove>().speed = 0;
				GetComponent<Collider>().enabled = false;
				StartCoroutine(Die());
			}
		}
		public Transform GetUnHitPoint()
		{
			if (unHitPoint)
				return unHitPoint;
			else
				return transform;
		}

		IEnumerator Die()
		{
			Player_Control.I.Keycord += valueKeyCard;
			transform.DOScale(0, 1);
			ParticleManager.InstParticle(dieParticle, transform.position, dieTime: 5);
			yield return new WaitForSeconds(1);
			gameObject.SetActive(false);
			EnemyManager.I.EnemyOver(this);
		}
	}
}
