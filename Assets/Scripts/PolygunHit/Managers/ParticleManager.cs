using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
	public static List<GameObject> strikes;
	[SerializeField]
	private List<GameObject> m_Strikes;
	private void Awake()
	{
		strikes = m_Strikes;
	}

	/// <summary>
	/// 新建撞击粒子效果
	/// </summary>
	/// <param name="num"></param>
	/// <param name="target"></param>
	/// <param name="parent"></param>
	public static void InstStrikeParticle(int num, Vector3 target, Transform parent = null)
	{
		InstParticle(strikes[num], target, parent, 1, 3);
	}

	/// <summary>
	/// 新建粒子效果
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="target"></param>
	/// <param name="parent"></param>
	public static void InstParticle(GameObject obj, Vector3 target,
		Transform parent = null, float Scale = 0,float dieTime = 10)
	{
		if (!obj)
		{
			Debug.Log("空粒子");
			return;
		}
		var newParticle = Instantiate(obj);
		newParticle.transform.position = target;
		if (parent)
			newParticle.transform.SetParent(parent);
		if (Scale != 0)
			newParticle.transform.localScale = Vector3.one * Scale;
		Destroy(newParticle, 10);
	}

}
