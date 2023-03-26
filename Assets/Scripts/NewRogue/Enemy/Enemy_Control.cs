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
		public int baseDamage = 1;
		/// <summary>
		/// 攻击冷却
		/// </summary>
		bool AtkCD;
		float CDTime = 3;
		#endregion

		int valueKeyCard
		{
			get
			{
				return Random.Range(5, 7);
			}
		}

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
				Player_Control.I.BeHit(baseDamage);
				Debug.Log("打出一次攻击");
			}
			yield return new WaitForSeconds(CDTime);
			AtkCD = false;
		}
		public void StartSetting()
		{

		}
		public void UnHit(float damage = 1)
		{
			health -= damage;
			if (health <= 0)
			{
				GetComponent<EnemyMove>().speed = 0;
				GetComponent<Collider>().enabled = false;
				StartCoroutine(Death());
			}
		}
		public Transform GetUnHitPoint()
		{
			if (unHitPoint)
				return unHitPoint;
			else
				return transform;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="beAssault">被攻击而死</param>
		/// <returns></returns>
		public IEnumerator Death(bool beAssault = true)
		{
			enabled = false;
			GetComponent<EnemyMove>().enabled = false;
			if (beAssault)
			{
				Player_Control.I.Keycord += valueKeyCard;

			}
			else
			{

			}
			float dieTime = Random.Range(0.9f, 1.1f);
			transform.DOScale(0, dieTime);
			//ParticleManager.InstParticle(dieParticle, transform.position, dieTime: 5);
			yield return new WaitForSeconds(dieTime + 0.1f);
			gameObject.SetActive(false);
			EnemyManager.I.EnemyOver(this);
		}
	}
}
