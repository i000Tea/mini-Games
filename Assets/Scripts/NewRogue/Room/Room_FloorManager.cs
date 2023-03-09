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

			var listRoom = startRoom;
			var createRoom = listRoom;

			yield return new WaitForFixedUpdate();
			for (int i = 0; i < mainRoadLength - 1; i++)
			{
				createRoom = CreateRoom(listRoom, Floor);
				if (createRoom)
					listRoom = createRoom;
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
		/// <param name="newRoomParent"> 父集节点 </param>
		/// <returns></returns>
		Room_Control CreateRoom(Room_Control beforeRoom = null, Transform newRoomParent = null)
		{
			// 获取之前房间中的一扇门 定义为 前置门
			var beforerDoor = beforeRoom.GetDoor();

			//Debug.LogWarning(beforerDoor);
			// 随机门的预制件
			var roomObj = rItem.RandomRoom(beforerDoor.dType);
			//Debug.LogWarning(roomObj);
			// 若随即不到 则直接返回空
			if (!roomObj)
				return null;
			// 创建随机房间 并获取脚本 定义为 新的房间
			var newRoom = Instantiate(roomObj).GetComponent<Room_Control>();
			// 如果输入了父集 则设置父对象
			if (newRoomParent)
				newRoom.transform.SetParent(newRoomParent);

			// 获取 新的房间 的一扇门适合的门 定义为新的门
			var newDoor = newRoom.GetDoor(beforerDoor.dType);

			// 设置旋转
			float rotateY = beforerDoor.transform.eulerAngles.y - newDoor.transform.eulerAngles.y - 180;
			if (rotateY > 360)
				rotateY %= 360;
			newRoom.transform.rotation = Quaternion.Euler(0, rotateY, 0);

			//设置位置
			newRoom.transform.position = beforerDoor.transform.position - newDoor.transform.position;

			//Debug.Log("ssaa");

			// 将生成好的房间加入列表
			rEntityList.Add(newRoom);
			beforerDoor.SetRoomLink(newRoom);
			newDoor.CloseMe();

			// 返回数据
			return newRoom;
		}
	}
}
