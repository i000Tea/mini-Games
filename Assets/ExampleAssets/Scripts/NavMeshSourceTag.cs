using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// 导航 网格源 标签
/// 标记组件用于 本地导航网格生成器(LocalNavMeshBuilder)
/// 支持网格过滤器和地形-可以扩展到物理和/或原语
/// </summary>
// Tagging component for use with the LocalNavMeshBuilder
// Supports mesh-filter and terrain - can be extended to physics and/or primitives
[DefaultExecutionOrder(-200)]
public class NavMeshSourceTag : MonoBehaviour
{
	// Global containers for all active mesh/terrain tags
	/// <summary>
	/// 所有用于寻路的网格/地形标签的全局 变量
	/// </summary>
	public static List<MeshFilter> m_Meshes = new List<MeshFilter>();
	public static List<Terrain> m_Terrains = new List<Terrain>();

	protected virtual void OnEnable()
	{
		//var m = GetComponent<MeshFilter>();
		//if (m != null)
		//{
		//    m_Meshes.Add(m);
		//}
		if (TryGetComponent(out MeshFilter mesh))
		{
			//Debug.Log($"{transform.parent.name}{gameObject.name}{mesh}");
			m_Meshes.Add(mesh);
		}

		//var t = GetComponent<Terrain>();
		//if (t != null)
		//{
		//    m_Terrains.Add(t);
		//}
		if (TryGetComponent(out Terrain terrain))
		{
			m_Terrains.Add(terrain);
		}
	}

	protected virtual void OnDisable()
	{
		//var m = GetComponent<MeshFilter>();
		//if (m != null)
		//{
		//    m_Meshes.Remove(m);
		//}
		if (TryGetComponent(out MeshFilter mesh))
		{
			m_Meshes.Remove(mesh);
		}
		//var t = GetComponent<Terrain>();
		//if (t != null)
		//{
		//	m_Terrains.Remove(t);
		//}
		if (TryGetComponent(out Terrain terrain))
		{
			m_Terrains.Remove(terrain);
		}
	}

	// Collect all the navmesh build sources for enabled objects tagged by this component
	/// <summary>
	/// 收集所有被该组件标记的对象的 nav mesh 构建源
	/// </summary>
	/// <param name="sources"></param>
	public static void Collect(ref List<NavMeshBuildSource> sources)
	{
		sources.Clear();

		for (var i = 0; i < m_Meshes.Count; ++i)
		{
			var mf = m_Meshes[i];
			if (mf == null) continue;

			var m = mf.sharedMesh;
			if (m == null) continue;

			var s = new NavMeshBuildSource();
			s.shape = NavMeshBuildSourceShape.Mesh;
			s.sourceObject = m;
			s.transform = mf.transform.localToWorldMatrix;
			s.area = 0;
			sources.Add(s);
		}

		for (var i = 0; i < m_Terrains.Count; ++i)
		{
			var t = m_Terrains[i];
			if (t == null) continue;

			var s = new NavMeshBuildSource();
			s.shape = NavMeshBuildSourceShape.Terrain;
			s.sourceObject = t.terrainData;
			// Terrain system only supports translation - so we pass translation only to back-end
			// 地形系统只支持翻译，所以我们只将翻译传递给后端
			s.transform = Matrix4x4.TRS(t.transform.position, Quaternion.identity, Vector3.one);
			s.area = 0;
			sources.Add(s);
		}
	}
}
