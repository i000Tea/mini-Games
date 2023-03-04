using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Tea.PolygonHit
{
	/// <summary>
	/// 无缝无限背景
	/// </summary>
	public class BackGround : MonoBehaviour
	{
		public List<Transform> griddings;
		public Vector3 me;

		private void OnValidate()
		{
			me = transform.position;
			GetGridding();
		}
		private void Start()
		{
			GetGridding();
		}
		public void SetBgImage(Sprite bg)
		{
			for (int i = 0; i < griddings.Count; i++)
			{
				griddings[i].GetComponent<Image>().sprite = bg;
			}
		}
		/// <summary>
		/// 设置地图格数量
		/// </summary>
		void GetGridding()
		{
			griddings = new List<Transform>();
			for (int i = 0; i < transform.childCount; i++)
			{
				griddings.Add(transform.GetChild(i));
			}
		}
		public void SetColor(Color set)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).GetComponent<Image>().color = set;
			}
		}

		/// <summary>
		/// 向左平移
		/// </summary>
		public void ForTheLeft()
		{
			griddings[BackGroundController.inst.mapNums - 1].position = griddings[0].position - new Vector3(4, 0, 0);
			griddings[BackGroundController.inst.mapNums - 1].SetAsFirstSibling();
			GetGridding();
		}
		/// <summary>
		/// 向右平移
		/// </summary>
		public void ForTheRight()
		{
			griddings[0].position = griddings[BackGroundController.inst.mapNums - 1].position + new Vector3(4, 0, 0);
			griddings[0].SetAsLastSibling();
			GetGridding();
		}

		public void MaxLR(ref float L, ref float R)
		{
			L = griddings[0].position.x;
			R = griddings[BackGroundController.inst.mapNums - 1].position.x;
		}
	}
}
