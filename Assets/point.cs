using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{
	public float _long = 0.1f;
	public Transform myPoint;
	public Transform targetTran;
	public Transform targetPoint;
	float rotateY;
	void Update()
	{
		rotateY = myPoint.eulerAngles.y - targetPoint.localEulerAngles.y + 180;
		targetTran.eulerAngles = new Vector3(0, rotateY, 0);
		targetTran.position = myPoint.position - JiaoDu(myPoint.eulerAngles.y, targetPoint.localPosition);
	}
	Vector3 JiaoDu(float jiaoDu, Vector3 point)
	{
		Vector3 a = new Vector3(
			point.x * Mathf.Cos(jiaoDu * Mathf.Deg2Rad) +
			point.z * Mathf.Sin(jiaoDu * Mathf.Deg2Rad),
			0,
			point.x * -Mathf.Sin(jiaoDu * Mathf.Deg2Rad) +
			point.z * Mathf.Cos(jiaoDu * Mathf.Deg2Rad)
			);

		return a;
	}
}
