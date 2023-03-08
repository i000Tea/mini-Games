using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.NewRouge
{
	/// <summary>
	/// 楼层管理器
	/// </summary>
	public class Room_FloorManager : MonoBehaviour
	{
		/// <summary>
		/// 楼层
		/// </summary>
		[SerializeField]
		Transform Floor;
		/// <summary>
		/// 初始房间
		/// </summary>
		[SerializeField]
		Room_Control startRoom;
		/// <summary>
		/// 已经生成的房间的列表
		/// </summary>
		List<Room_Control> rEntityList;
		/// <summary>
		/// 数据
		/// </summary>
		[SerializeField]
		RoomItem rItem;
		/// <summary>
		/// 主干道的长度
		/// </summary>
		public int mainRoadLength = 4;
		private void Start()
		{
			StartCoroutine(iCreateWholeFloor());
			//CreateWholeFloor();
		}
		/// <summary>
		/// 生成楼层
		/// </summary>
		/// <returns></returns>
		IEnumerator iCreateWholeFloor()
		{
			// 房间列表重置
			rEntityList = new List<Room_Control>();
			// 初始的房间作为第一个
			rEntityList.Add(startRoom);

			var mainRoom = startRoom;
			var createRoom = mainRoom;
			for (int i = 0; i < mainRoadLength - 1; i++)
			{
				createRoom = CreateRoom(createRoom, Floor);
				yield return new WaitForFixedUpdate();
			}
			for (int i = 1; i < rEntityList.Count; i++)
			{
				rEntityList[i].AwakeRoomSet();
			}
		}
		/// <summary>
		/// 创造房间
		/// </summary>
		/// <param name="beforeRoom"> 上一个房间 </param>
		/// <param name="parent"> 父集节点 </param>
		/// <returns></returns>
		Room_Control CreateRoom(Room_Control beforeRoom = null,Transform parent = null)
		{
			var rPoint = beforeRoom.SomeDoor();
			// 随机房间
			var selectRoom = rItem.RoomPrefabs[0];
			// 创建房间
			var CreateRoom = Instantiate(selectRoom).GetComponent<Room_Control>();
			if (parent)
				CreateRoom.transform.SetParent(parent);
			//CreateRoom.name = "a" + Time.time;

			// 获取创建的房间的任意一扇门
			var newPoint = CreateRoom.SomeDoor();

			//Debug.Log(rPoint + " " + newPoint);
			// 设置旋转
			float rotateY = rPoint.transform.eulerAngles.y - newPoint.transform.eulerAngles.y - 180;
			if (rotateY > 360)
				rotateY %= 360;
			CreateRoom.transform.rotation = Quaternion.Euler(0, rotateY, 0);

			//设置位置
			CreateRoom.transform.position = rPoint.transform.position - newPoint.transform.position;

			rEntityList.Add(CreateRoom);
			rPoint.SetRoomLink(CreateRoom);
			newPoint.SetRoomLink();
			// 返回数据
			return CreateRoom;
		}
	}
}
