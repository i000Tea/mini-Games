using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tea;

namespace Tea.PolygonHit
{
	/// <summary>
	/// 震荡
	/// </summary>
	public class Skill1003_Latch : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_Particle;

		private float m_LatchTime;
		[Tooltip("旋转速度最小限制")]
		[Range(0, 10)]
		public float m_RotateSpeed;
		float min;
		[Tooltip("旋转速度最大限制")]
		[Range(0, 10)]
		public float m_RotateMaxSpeed;
		float max;
		float speed;
		private void OnValidate()
		{
			if (m_RotateSpeed > max)
			{
				m_RotateMaxSpeed = m_RotateSpeed;
			}
			min = m_RotateSpeed;

			if (m_RotateMaxSpeed < m_RotateSpeed)
			{
				m_RotateSpeed = m_RotateMaxSpeed;
			}
			max = m_RotateMaxSpeed;
		}
		private void OnDisable()
		{
			StopAllCoroutines();
			Destroy(gameObject);
		}
		public void StartLatch(float latchTime, float rotateSpeed)
		{
			m_LatchTime += latchTime;
			if (speed == 0)
			{
				speed = rotateSpeed;
				StartCoroutine(StartRotate());
			}
		}

		IEnumerator StartRotate()
		{
			var enemy = transform.parent.GetComponent<EnemyBase>();
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
			yield return 0;
			float rotate = 0;
			float nowTime = 0;
			while (nowTime < m_LatchTime)
			{
				yield return new WaitForFixedUpdate();

				rotate += speed;
				transform.eulerAngles = new Vector3(0, 0, rotate);

				nowTime += Time.deltaTime;
			}
			Destroy(gameObject);
		}
	}
}
