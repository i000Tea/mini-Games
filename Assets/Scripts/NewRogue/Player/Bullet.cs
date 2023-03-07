using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class Bullet : MonoBehaviour
	{
		public GameObject muzzleParticle;
		public GameObject hitParticle;
		private void Start()
		{
			ParticleManager.InstParticle(muzzleParticle,
				transform.position, null, dieTime: 1f);
		}
		private void OnCollisionEnter(Collision collision)
		{
			//Debug.Log("aaa");
			ParticleManager.InstParticle(hitParticle, collision.collider.ClosestPoint(transform.position)
				, null, dieTime: 1f);

			if (collision.gameObject.tag == "Enemy")
			{
				collision.gameObject.GetComponent<Enemy_Control>().UnHit();
			}
			//collision.gameObject.GetComponent<EnemyBase>().UnHit(4);
			GetComponent<Collider>().enabled = false;
			Destroy(gameObject, 0.05f);
		}
	}
}
