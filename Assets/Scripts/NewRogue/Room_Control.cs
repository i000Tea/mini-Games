using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Tea.NewRouge
{
	/// <summary>
	/// 房间的控制器
	/// </summary>
	public class Room_Control : MonoBehaviour
	{
		/// <summary>
		/// 门已耗尽
		/// </summary>
		public bool doorUsedUp
		{
			get
			{
				// 有任意一个点未被使用 则标记为否
				for (int i = 0; i < rPoints.Count; i++)
					if (!rPoints[i].UnUse)
						return false;
				// 所有点都标记为使用 则更新为是
				return true;
			}
		}
		[SerializeField]
		public List<Room_DoorPoint> rPoints;
		[SerializeField]
		private Transform roomParent;

		private void OnValidate()
		{
			if (rPoints == null || rPoints.Count != transform.GetChild(0).childCount)
			{
				rPoints = new List<Room_DoorPoint>();
				for (int i = 0; i < transform.GetChild(0).childCount; i++)
				{
					rPoints.Add(transform.GetChild(0).GetChild(i).GetComponent<Room_DoorPoint>());
				}
			}
			if (!roomParent)
				roomParent = transform.GetChild(1);
			if (roomParent)
			{
				for (int i = 0; i < roomParent.childCount; i++)
				{
					if (!roomParent.GetChild(i).GetComponent<NavMeshSourceTag>())
						roomParent.GetChild(i).gameObject.AddComponent<NavMeshSourceTag>();
				}
			}
		}
		private void Awake()
		{

		}
	
		/// <summary>
		/// 初始化设置
		/// </summary>
		public void AwakeRoomSet()
		{
			gameObject.SetActive(false);
			transform.localScale = Vector3.zero;
			gameObject.SetActive(true);
		}
		public void ShowRoom()
		{
			gameObject.SetActive(true);
			transform.localScale = Vector3.zero;
			transform.DOScale(1, 0.7f).SetEase(Ease.OutCirc);
		}
		/// <summary>
		/// 房间链接
		/// </summary>
		/// <param name="rPoint">目标门</param>
		public void RoomLink(Room_DoorPoint rPoint)
		{
			var myPoint = SomeDoor();
			myPoint.name = "aa";

			// 旋转
			float rotateY = 180 + myPoint.transform.localEulerAngles.y + rPoint.transform.eulerAngles.y;
			if (rotateY > 360)
				rotateY %= 360;
			Debug.Log(rotateY);
			transform.eulerAngles = new Vector3(0, rotateY, 0);

			// 位移
			transform.position = rPoint.transform.position - myPoint.transform.position;

			// 目标点标记为已使用 目标点的下一房间设为自身
			rPoint.UnUse = true;
			rPoint.nextRoom = this;
			// 自身对应门标记为已使用 并将其关闭
			myPoint.UnUse = true;
			myPoint.gameObject.SetActive(false);
		}
		/// <summary>
		/// 返回此房间的某一个未使用过的门
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public Room_DoorPoint SomeDoor(int num = -1)
		{
			//Debug.Log(num);
			// 当传入-1 视为随机
			if (num < 0)
			{
				num = Random.Range(0, rPoints.Count - 1);
			}
			//Debug.Log(num);
			// 查找门点
			for (int i = 0; i < rPoints.Count; i++)
			{
				// 若门点已被使用 +1 若超出数组 返回 0
				if (rPoints[num].UnUse)
				{
					num++;
					if (num >= rPoints.Count)
					{
						num = 0;
					}
				}
				// 若没有被使用 返回该门点
				else
				{
					return rPoints[num];
				}
			}
			return null;
		}
	}
}
