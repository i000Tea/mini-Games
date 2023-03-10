using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tea;

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
		
		targetTran.position = myPoint.position - AddVoids.AngleTransfor(targetPoint.localPosition, rotateY);
		targetTran.eulerAngles = new Vector3(0, rotateY, 0);
	}
}
