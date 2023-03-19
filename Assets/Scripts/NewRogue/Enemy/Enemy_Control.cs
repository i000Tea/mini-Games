using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tea.NewRouge
{
	public class Enemy_Control : MonoBehaviour
	{
		public float health = 10;

		int valueKeyCard = 1;

		public GameObject dieParticle;
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
		IEnumerator Die()
		{
			Player_Control.I.Keycord += valueKeyCard;
			transform.DOScale(0, 1);
			ParticleManager.InstParticle(dieParticle, transform.position,dieTime:5);
			yield return new WaitForSeconds(1);
			gameObject.SetActive(false);
			EnemyManager.inst.EnemyOver(this);
		}
	}
}
