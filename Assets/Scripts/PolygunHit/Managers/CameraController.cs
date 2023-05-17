using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;
using Cinemachine;

namespace Tea.PolygonHit
{
	/// <summary>
	/// 相机控制器
	/// </summary>
	public class CameraController : Singleton<CameraController>
	{
		public CinemachineVirtualCamera baseCamera;
		public Camera myCamera;

		[SerializeField]
		Animator CameraAnim;

		#region Edge
		/// <summary>
		/// 最大边缘距离
		/// </summary>
		public float MaxEdgeDistance;
		public float MaxUp()
		{
			return GetCorners(10)[0].y;
		}
		public float MaxDown()
		{
			return GetCorners(10)[3].y;
		}
		public float MaxLeft()
		{
			return GetCorners(10)[0].x;
		}
		public float MaxRight()
		{
			return GetCorners(10)[3].x;
		}

		#endregion
		private void Start()
		{
			baseCamera.m_Follow = PlayerBase.I.transform;

			myCamera = Camera.main;

			MaxEdgeDistance = Camera.main.ViewportToWorldPoint(new Vector2(-0.3f, -0.3f)).magnitude;

		}
		private void OnDestroy()
		{
		}

		#region Event 广播/事件

		protected override void AddDelegate()
		{
			EventControl.OnAddAntherList(ActionType.UnStrike, PlayerUnHit);
			EventControl.OnAddAntherList(ActionType.PlayerDestory, PlayerDestory);
		}
		protected override void Removedelegate()
		{
			EventControl.OnRemoveAhtnerList(ActionType.UnStrike, PlayerUnHit);
			EventControl.OnRemoveAhtnerList(ActionType.PlayerDestory, PlayerDestory);
		}

		/// <summary>
		/// 玩家受伤时 播放受击
		/// </summary>
		private void PlayerUnHit()
		{
			string _name = "Hit ";
			_name += UnityEngine.Random.Range(1, 4).ToString();
			CameraAnim.Play(_name);
			//Debug.Log(_name);
		}
		/// <summary>
		/// 玩家死亡时 播放Timeline
		/// </summary>
		private void PlayerDestory()
		{
			CameraAnim.Play("Destory");
			//Debug.Log("destory");
		}
		#endregion

		/// <summary>
		/// 0  1
		/// 
		/// 2  3
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		private Vector3[] GetCorners(float distance)
		{
			Vector3[] corners = new Vector3[4];

			float halfFOV = (myCamera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
			float aspect = myCamera.aspect;

			float height = distance * Mathf.Tan(halfFOV);
			float width = height * aspect;

			// UpperLeft
			corners[0] = transform.position - (transform.right * width);
			corners[0] += transform.up * height;
			corners[0] += transform.forward * distance;

			// UpperRight
			corners[1] = transform.position + (transform.right * width);
			corners[1] += transform.up * height;
			corners[1] += transform.forward * distance;

			// LowerLeft
			corners[2] = transform.position - (transform.right * width);
			corners[2] -= transform.up * height;
			corners[2] += transform.forward * distance;

			// LowerRight
			corners[3] = transform.position + (transform.right * width);
			corners[3] -= transform.up * height;
			corners[3] += transform.forward * distance;

			return corners;
		}
	}
}
