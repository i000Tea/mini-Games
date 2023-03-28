using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class Bullet : MonoBehaviour
	{
		enum BulletMode
		{
			_base,
			onlyOneRay,
			sustainedRay,
		}

		[SerializeField]
		BulletMode mode;
		/// <summary>
		/// 模组1的基础射速
		/// </summary>
		const float mode1BaseVelocity = 16;
		public GameObject muzzleParticle;
		public GameObject hitParticle;
		private float damage = 1;

		/// <summary>
		/// 创建子弹 
		/// </summary>
		/// <param name="item"></param>
		public void Create(HoldWeaponItem item)
		{
			// 设置位置及缩放
			transform.SetPositionAndRotation(item.muzzle.position, item.muzzle.rotation);
			transform.localScale = Vector3.one * item.scale;

			// 设置伤害
			damage = item.damage;

			// 设置枪口粒子
			ParticleManager.InstParticle(muzzleParticle,
				transform.position, null, Scale: item.scale, dieTime: 1f);
			// 当存在音频时 播放
			if (TryGetComponent(out AudioSource audio))
			{
				audio.pitch = Random.Range(audio.pitch - 0.15f, audio.pitch + 0.15f);
				audio.Play();
			}


			var Offset = Random.Range(-item.Offset, item.Offset);
			//var	HorizOffset = Random.Range(-item.Offset, item.Offset);
			//var	vertiOffset = Random.Range(-item.Offset, item.Offset);
			// 计算经过偏移后的射击方向
			var _forward = (transform.forward + new Vector3(Offset, 0, Offset));

			switch (mode)
			{
				case BulletMode._base:
					ShootMod1(mode1BaseVelocity  * item.velocity * _forward);
					break;
				case BulletMode.onlyOneRay:
					ShootMode2(_forward);
					break;
				case BulletMode.sustainedRay:
					break;
				default:
					break;
			}
		}
		/// <summary>
		/// 子弹模组1射击
		/// </summary>
		/// <param name="velocity"></param>
		/// <param name="scale"></param>
		void ShootMod1(Vector3 velocity)
		{
			if (TryGetComponent(out Rigidbody rig))
			{
				rig.velocity = velocity;
			}
			else
			{
				Debug.LogWarning("子弹模组1未找到物理属性");
			}
		}
		private void OnCollisionEnter(Collision collision)
		{
			//Debug.Log(collision.gameObject.name);
			int targetMask = (1 << 0) | (1 << 6);
			// 循环生成一条从起始点o, 以方向d为延展的射线
			Ray ray = new Ray(transform.position, transform.forward);
			// hit用于从光线投射中, 获取信息的结构
			RaycastHit hit;

			Vector3 point = collision.collider.ClosestPoint(transform.position);
			// 射线, 抛出的碰撞信息, 射线长度(注意, 碰撞体越小, 速度越快, 越容易帧缺失)
			if (Physics.Raycast(ray, out hit, 6, targetMask))
			{
				// 从起点到碰撞点画一条线
				Debug.DrawLine(ray.origin, hit.point, Color.red);
				//Debug.Log(hit.transform.name);
				if (Vector3.Distance(point, hit.point) < 3)
					point = hit.point;
			}

			var newParticle = ParticleManager.InstParticle(hitParticle, point, null, Scale: transform.lossyScale.z, dieTime: 1f);


			if (newParticle.TryGetComponent(out AudioSource audio))
			{
				audio.pitch = Random.Range(audio.pitch - 0.15f, audio.pitch + 0.15f);
				audio.Play();
			}

			if (collision.gameObject.tag == "Enemy")
			{
				collision.gameObject.GetComponent<Enemy_Control>().UnHit(damage);
			}

			//collision.gameObject.GetComponent<EnemyBase>().UnHit(4);
			GetComponent<Collider>().enabled = false;
			Destroy(gameObject, 0.05f);
		}
		/// <summary>
		/// 子弹模组2(一次性射线)射击
		/// </summary>
		void ShootMode2(Vector3 velocity)
		{
			if (TryGetComponent(out LineRenderer line))
			{
				//设置射线检测层
				int targetMask = (1 << 0) | (1 << 6) | (1 << 8);
				// 生成一条从起始点o, 以方向d为延展的射线
				Ray ray = new Ray(transform.position, velocity);
				// hit用于从光线投射中, 获取信息的结构
				RaycastHit hit;
				// 射线, 抛出的碰撞信息, 射线长度(注意, 碰撞体越小, 速度越快, 越容易帧缺失)
				if (Physics.Raycast(ray, out hit, 100, targetMask))
				{
					// 从起点到碰撞点画一条线
					Debug.DrawLine(ray.origin, hit.point, Color.red);
					if (hit.collider.gameObject.TryGetComponent(out Enemy_Control enemy))
					{
						enemy.UnHit(damage);
					}
					//Debug.Log(hit.collider.gameObject.name);
					line.SetPosition(0, transform.position);
					line.SetPosition(1, hit.point);
					var newParticle = ParticleManager.InstParticle(hitParticle, hit.point, null, Scale: transform.lossyScale.z, dieTime: 1f);

					StartCoroutine(Darken(line.material));
				}
			}
		}
		IEnumerator Darken(Material material)
		{
			float power = 1;
			while (power > 0)
			{
				power -= Time.deltaTime;
				material.SetFloat("_Alpha", power);
				yield return new WaitForFixedUpdate();
			}
			Destroy(gameObject);
		}
	}
}
