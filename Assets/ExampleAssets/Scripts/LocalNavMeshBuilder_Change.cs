using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

/// <summary>
/// 从 NavMeshSourceTag 标记的数据源中构建并更新本地化的 navmesh
/// </summary>
[DefaultExecutionOrder(-102)]
public class LocalNavMeshBuilder_Change : LocalNavMeshBuilder
{
	public static LocalNavMeshBuilder_Change inst;
	float lastTime;
	protected override IEnumerator Start()
	{
		return null;
	}


	[SerializeField]
	int setWait = 15;
	int needWait;
	int nowWait;
	private void Awake()
	{
		if (!inst)
		{
			inst = this;
		}
		if (setWait < 5)
		{
			setWait = 15;
		}
	}
	private void FixedUpdate()
	{
		if (lastTime > Time.time)
		{
			needWait = setWait;
		}
		else
		{
			needWait = 120;
		}

		nowWait++;
		if (nowWait >= needWait)
		{
			nowWait = 0;
			UpdateNavMesh();
		}
	}
	public void UpdateTime(float AddTime)
	{
		lastTime = Time.time + AddTime;
	}

}
