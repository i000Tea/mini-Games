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
			int targetMask = (1 << 0) | (1 << 6);
			// 生成一条从起始点o, 以方向d为延展的射线
			Ray ray = new Ray(transform.position, transform.forward);
			// hit用于从光线投射中, 获取信息的结构
			RaycastHit hit;
			// 射线, 抛出的碰撞信息, 射线长度(注意, 碰撞体越小, 速度越快, 越容易帧缺失)
			if (Physics.Raycast(ray, out hit, 6, targetMask))
			{
				// 从起点到碰撞点画一条线
				Debug.DrawLine(ray.origin, hit.point, Color.red);
				//Debug.Log(hit.transform.name);

				ParticleManager.InstParticle(hitParticle, hit.point
					, null, dieTime: 1f);
			}
			else
			{
				ParticleManager.InstParticle(hitParticle, collision.collider.ClosestPoint(transform.position)
					, null, dieTime: 1f);
			}

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
