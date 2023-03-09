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
			// 第一次 创造主路线
			for (int i = 0; i < mainRoadLength - 1; i++)
			{
				createRoom = CreateRoom(listRoom, Floor, DoorType._2Door);
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
		/// <param name="dType"> 门的类型 </param>
		/// <param name="beforeRoom"> 上一个房间 </param>
		/// <param name="newRoomParent"> 父集节点 </param>
		/// <returns></returns>
		Room_Control CreateRoom(Room_Control beforeRoom, Transform newRoomParent = null, DoorType? dType = null)
		{
			// 若输入类型为空 则随机一个
			if (dType == null)
			{
				dType = AddVoids.RandomEnum<DoorType>();
				Debug.Log(dType);
			}
			if (!beforeRoom)
			{
				Debug.Log("前一房间为空");
				return null;
			}

			// 创建随机房间(确保此房间有可用的门) 并获取脚本 定义为 新的房间
			Room_Control newRoom = null;
			try
			{
				newRoom = Instantiate(rItem.RandomRoom((DoorType)dType)).GetComponent<Room_Control>();
				// 如果输入了父集 则设置父对象
				if (newRoomParent)
					newRoom.transform.SetParent(newRoomParent);
			}
			catch (Exception)
			{
				Debug.LogError($"新的房间为{newRoom}");
			}


			Room_DoorPoint beforerDoor = null, newDoor = null;
			try
			{
				// 获取 新的房间 的一扇门适合的门 定义为新的门
				newDoor = newRoom.GetDoor((DoorType)dType);
				// 获取之前房间中的一扇门 定义为 前置门
				beforerDoor = beforeRoom.GetDoor((DoorType)dType);
			}
			catch (Exception)
			{
				Debug.Log($"原房间的门 {beforerDoor} \n 新房间的门 {newDoor}");
			}

			// 设置旋转
			float rotateY = beforerDoor.transform.eulerAngles.y - newDoor.transform.localEulerAngles.y - 180;
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
