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
			CreateWholeFloor();
		}
		/// <summary>
		/// 创建整层楼
		/// </summary>
		void CreateWholeFloor()
		{
			// 房间列表重置
			rEntityList = new List<Room_Control>();
			// 初始房间作为第一个
			rEntityList.Add(startRoom);

			startRoom.RoomState = true;

			var mainRoom = startRoom;
			for (int i = 0; i < mainRoadLength; i++)
			{

				mainRoom = CreateRoom(mainRoom.SomeDoor());
				rEntityList.Add(mainRoom);
			}
		}
		/// <summary>
		/// 创造房间
		/// </summary>
		Room_Control CreateRoom(Room_DoorPoint rPoint = null)
		{
			// 查询可用门
			if (!rPoint)
			{

			}

			// 选择随机房间进行创建
			var selectRoom = rItem.RoomPrefabs[0];
			var room = Instantiate(selectRoom).GetComponent<Room_Control>();
			room.RoomLink(rPoint);

			// 返回数据
			return null;
		}
	}
}
