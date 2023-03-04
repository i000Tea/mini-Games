using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
	public class Skill3003_Bullet : MonoBehaviour
	{
		[SerializeField]
		GameObject ShootParticle;
		private void OnCollisionEnter2D(Collision2D collision)
		{
			//Debug.Log("aaa");
			ParticleManager.InstParticle(ShootParticle,
				(Vector3)
				collision.collider.ClosestPoint(transform.position) + new Vector3(0, 0, 1)
				, null, 0.5f);
			collision.gameObject.GetComponent<EnemyBase>().UnHit(4);
			Destroy(gameObject,0.01f);
		}
	}
}
