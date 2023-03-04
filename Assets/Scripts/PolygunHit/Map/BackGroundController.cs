using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tea.PolygonHit;
namespace Tea.PolygonHit
{
	/// <summary>
	/// 无缝背景父集
	/// </summary>
	public class BackGroundController : MonoBehaviour
	{
		public static BackGroundController inst;
		/// <summary>
		/// 子物体的方位
		/// </summary>
		List<Transform> bGPoints;
		/// <summary>
		/// 子物体的脚本
		/// </summary>
		List<BackGround> backGrounsds;

		/// <summary>
		/// 最大长宽数量
		/// </summary>
		public int mapNums
		{
			get { return bGPoints.Count; }
		}

		float longLeft, longRight;

		public Sprite m_bg;
		public Color BGColor;

		public Color meshColor;
		public Material meshMat;

		private void OnValidate()
		{
			Camera.main.backgroundColor = BGColor;
			meshMat.color = meshColor;
			SetBackGrounds();
			for (int i = 0; i < backGrounsds.Count; i++)
			{
				backGrounsds[i].SetBgImage(m_bg);
			}
		}
		private void Awake()
		{
			inst = this;
			SetBackGrounds();
			GetLandR();
		}
		private void Start()
		{
		}
		private void Update()
		{
			InfiniteMap();
		}
		/// <summary>
		/// 设置地图
		/// </summary>
		void SetBackGrounds()
		{
			// 重新设置内容
			bGPoints = new List<Transform>();
			for (int i = 0; i < transform.childCount; i++)
				bGPoints.Add(transform.GetChild(i));
			if (backGrounsds == null)
				backGrounsds = new List<BackGround>();
			if (backGrounsds.Count < bGPoints.Count)
			{
				backGrounsds = new List<BackGround>();
				for (int i = 0; i < bGPoints.Count; i++)
					backGrounsds.Add(bGPoints[i].GetComponent<BackGround>());
			}
		}
		void GetLandR()
		{
			backGrounsds[0].MaxLR(ref longLeft, ref longRight);
		}
		/// <summary>
		/// 无限地图
		/// </summary>
		void InfiniteMap()
		{
			if (CameraController.inst.MaxUp() > bGPoints[0].position.y)
			{
				bGPoints[mapNums - 1].position = bGPoints[0].position + new Vector3(0, 4, 0);
				bGPoints[mapNums - 1].SetAsFirstSibling();
				SetBackGrounds();
			}
			if (CameraController.inst.MaxDown() < bGPoints[mapNums - 1].position.y)
			{
				bGPoints[0].position = bGPoints[mapNums - 1].position - new Vector3(0, 4, 0);
				bGPoints[0].SetAsLastSibling();
				SetBackGrounds();
			}

			if (CameraController.inst.MaxLeft() < longLeft)
			{
				for (int i = 0; i < backGrounsds.Count; i++)
					backGrounsds[i].ForTheLeft();
				GetLandR();
			}

			if (CameraController.inst.MaxRight() > longRight)
			{
				for (int i = 0; i < backGrounsds.Count; i++)
					backGrounsds[i].ForTheRight();
				GetLandR();

			}
		}
	}
}
