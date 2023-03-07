using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tea.NewRouge
{
	public class Enemy_Control : MonoBehaviour
	{
		public int health = 3;
		public void Startsetting()
		{

		}
		public void UnHit(int damage = 1)
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
			transform.DOScale(0,1);
			yield return new WaitForSeconds(1);
			gameObject.SetActive(false);
			EnemyManager.inst.EnemyOver(this);
		}
	}
}
