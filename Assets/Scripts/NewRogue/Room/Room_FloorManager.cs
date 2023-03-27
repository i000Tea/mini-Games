using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.NewRouge
{
	/// <summary>
	/// 楼层管理器
	/// </summary>
	public class Room_FloorManager : Singleton<Room_FloorManager>
	{
		#region 变量
		/// <summary>
		/// 数据
		/// </summary>
		[SerializeField]
		RoomItem rItem;
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
		List<Room_Control_EnemyCreate> rEnemyList;

		[SerializeField]
		bool createCollider;
		/// <summary>
		/// 主干道的长度
		/// </summary>
		public int mainRoadLength = 5;
		/// <summary>
		/// 分支路线最大数量
		/// </summary>
		public int branchRoadMaxNum = 4;
		/// <summary>
		/// 分支路线最长长度
		/// </summary>
		public int branchRoadMaxLength = 4;

		/// <summary>
		/// 刷新武器数量
		/// </summary>
		public int WeaponNum = 3;
		public int propNum = 6;
		/// <summary>
		/// 开门需要的花费
		/// </summary>
		public int OpenDoorCost = 10;
		#endregion
		public void StartCreate()
		{
			StartCoroutine(ICreateWholeFloor());
		}
		/// <summary>
		/// 生成楼层
		/// </summary>
		/// <returns></returns>
		IEnumerator ICreateWholeFloor()
		{
			yield return new WaitForFixedUpdate();
			// 房间列表重置
			rEnemyList = new List<Room_Control_EnemyCreate>();
			rEntityList = new List<Room_Control>();

			// 生成基准房间设置为初始房间
			Room_Control BaseRoom = startRoom;
			Room_Control createRoom = null;

			// 创建敌人房间
			var a = (Room_Control_EnemyCreate)TryCreateRoom(RoomPrefabs.EnemyCreate, BaseRoom, Floor, DoorType._2Door);
			if (a)
				rEnemyList.Add(a);
			//Debug.Log(a);
			yield return new WaitForFixedUpdate();
			//Debug.Log(mainRoadLength);
			// 第一循环 创造主路线
			for (int i = 0; i < mainRoadLength - 1; i++)
			{
				// 当基准房间存在时 使用基准房间 尝试生成新房间
				if (BaseRoom)
					createRoom = TryCreateRoom(RoomPrefabs.BaseRoom, BaseRoom, Floor, DoorType._2Door);
				// 生成房间成功后 更新基准房间
				if (createRoom)
				{
					// 将生成好的房间加入列表
					rEntityList.Add(createRoom);
					BaseRoom = createRoom;
				}
				yield return new WaitForFixedUpdate();
			}
			// 获取主路线的最后一个房间
			var baseLastRoom = rEntityList[rEntityList.Count - 1];

			//Debug.Log(baseLastRoom);
			// 分支岔道1 从初始房间开始(剩余两间)
			for (int i = 0; i < 2; i++)
			{
				if (!startRoom.doorUsedUp)
				{
					BaseRoom = startRoom;
					var branchLength = UnityEngine.Random.Range(3, branchRoadMaxLength);
					for (int n = 0; n < branchLength; n++)
					{
						// 当基准房间存在时 使用基准房间 尝试生成新房间
						if (BaseRoom)
							createRoom = TryCreateRoom(RoomPrefabs.BaseRoom, BaseRoom, Floor, DoorType._2Door);
						// 生成房间成功后 更新基准房间
						if (createRoom)
						{
							// 将生成好的房间加入列表
							rEntityList.Add(createRoom);
							BaseRoom = createRoom;
						}
						yield return new WaitForFixedUpdate();
					}
				}
			}
			var branchNum = UnityEngine.Random.Range(1, branchRoadMaxNum);
			// 分支岔道2 从随机房间开始
			for (int i = 0; i < branchNum; i++)
			{
				var rNum = UnityEngine.Random.Range(0, rEntityList.Count);
				for (int set = 0; set < rEntityList.Count; set++)
				{
					if (!rEntityList[rNum].doorUsedUp)
					{
						BaseRoom = rEntityList[rNum];
						break;
					}
					else
					{
						rNum++;
						if (rNum >= rEntityList.Count)
							rNum = 0;
					}
				}
				var branchLength = UnityEngine.Random.Range(3, branchRoadMaxLength);
				for (int n = 0; n < branchLength; n++)
				{
					// 当基准房间存在时 使用基准房间 尝试生成新房间
					if (BaseRoom)
						createRoom = TryCreateRoom(RoomPrefabs.BaseRoom, BaseRoom, Floor, DoorType._2Door);
					// 生成房间成功后 更新基准房间
					if (createRoom)
					{
						// 将生成好的房间加入列表
						rEntityList.Add(createRoom);
						BaseRoom = createRoom;
					}
					yield return new WaitForFixedUpdate();
				}

			}

			// 生成道具
			for (int i = 0; i < WeaponNum; i++)
			{
				var num = UnityEngine.Random.Range(2, rEntityList.Count);
				GameObject prop;
				for (int n = 0; n < rEntityList.Count; n++)
				{
					if (num >= rEntityList.Count)
						num = 1;

					prop = rEntityList[num].CreateProp(rItem.weaponPrefab);
					if (prop)
					{
						//Debug.Log(num);
						break;
					}
					else
					{
						num++;
					}
				}
			}

			// 所有房间初始化
			startRoom.RoomAwakeSet(true);
			for (int i = 0; i < rEnemyList.Count; i++)
			{
				rEnemyList[i].RoomAwakeSet();
			}
			for (int i = 0; i < rEntityList.Count; i++)
			{
				rEntityList[i].RoomAwakeSet();
			}
			for (int i = 0; i < startRoom.myDoors.Count; i++)
			{
				startRoom.myDoors[i].OpenDoor(false);
			}
		}
		/// <summary>
		/// 创造房间
		/// </summary>
		/// <param name="roomPrefab"> 门的预制件 </param>
		/// <param name="beforeRoom"> 上一个房间 </param>
		/// <param name="newRoomParent"> 父集节点 </param>
		/// <param name="dType">门类型</param>
		/// <param name="attempts">尝试次数</param>
		/// <returns></returns>
		Room_Control TryCreateRoom(RoomPrefabs roomPrefab, Room_Control beforeRoom,
			Transform newRoomParent = null, DoorType? dType = null, int attempts = 6)
		{
			// 若类型为空 则返回空
			if (!beforeRoom)
			{
				Debug.Log("前一房间为空");
				return null;
			}
			// 若输入门类型为空 则随机一个
			if (dType == null)
			{
				dType = AddVoids.RandomEnum<DoorType>();
				Debug.Log(dType);
			}

			// 获取之前房间中的一扇门 定义为 前置门

			Room_Control newRoom = null;
			Room_DoorPoint newDoor = null;
			int newDoorNum = -1;
			Room_DoorPoint beforerDoor = null;
			float rotateY = 0;
			//Debug.Log("开始尝试构建");
			for (int i = 0; i < attempts; i++)
			{
				var a = rItem.RandomRoom(roomPrefab, (DoorType)dType);
				//Debug.Log(a);
				//Debug.Log((DoorType)dType);
				newRoom = a.GetComponent<Room_Control>();
				newDoor = newRoom.GetDoor((DoorType)dType);
				newDoorNum = newRoom.GetDoorNum(newDoor);
				beforerDoor = beforeRoom.GetDoor((DoorType)dType);
				// 设置旋转值
				rotateY = beforerDoor.transform.eulerAngles.y - newDoor.transform.localEulerAngles.y + 180;
				if (Mathf.Abs(rotateY) > 360)
					rotateY %= 360;
				string log = $"构建次数{i + 1}";
				if (!DetectionDoor(
					beforerDoor.transform,
					(newDoor.transform.localPosition - newRoom.roomColl.localPosition),
					rotateY,
					newRoom.roomColl.lossyScale))
				{
					//Debug.Log(log + $"成功 房间为{ newRoom.roomColl}");
					//创建成功 跳出循环
					break;
				}
				if (i >= attempts - 1)
				{
					//Debug.Log(log + $"失败");
					return null;
				}
				//Debug.Log(log + ",继续");
			}
			//Debug.Log("构建成功");

			//====== 创建成功 开始构造 ======
			newRoom = Instantiate(newRoom.gameObject).GetComponent<Room_Control>();
			if (newDoorNum != -1)
				newDoor = newRoom.myDoors[newDoorNum];
			else
				newDoor = null;
			// 如果输入了父集 则设置父对象
			if (newRoomParent)
				newRoom.transform.SetParent(newRoomParent);
			// 设置位置 和 旋转
			newRoom.transform.SetPositionAndRotation(
				beforerDoor.transform.position - AddVoids.AngleTransfor(newDoor.transform.position, rotateY),
				Quaternion.Euler(0, rotateY, 0));
			newRoom.roomDepth = beforeRoom.roomDepth + 1;
			beforerDoor.LinkNextRoom(newRoom, OpenDoorCost);
			newDoor.CloseMe();

			// 返回数据
			return newRoom;
		}
		/// <summary>
		/// 检测区域内的碰撞体
		/// </summary>
		/// <param name="beforeWorldPoint">已经生成过用的 A点 的绝对世界坐标</param>
		/// <param name="createChildPoint">用于检定偏移的 B点 的相对子集坐标</param>
		/// <param name="rotateY"> 两点之间Y值的旋转 </param>
		/// <param name="collScale"> 来自 B点父集的碰撞体大小(原始大小) </param>
		/// <returns> 碰撞体是否存在 </returns>
		bool DetectionDoor(Transform beforeWorldPoint, Vector3 createChildPoint, float rotateY, Vector3 collScale)
		{
			/// 碰撞体区域
			Vector3 CollPosition = beforeWorldPoint.position + beforeWorldPoint.forward * 4 -
								   AddVoids.AngleTransfor(createChildPoint, rotateY);

			int layer = 1 << 6;
			var _list = Physics.OverlapBox(CollPosition, collScale / 2, Quaternion.Euler(0, rotateY, 0), layer);

			/// 生成碰撞区域
			if (createCollider)
			{
				Transform obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
				obj1.position = CollPosition + Vector3.down * 1;
				obj1.eulerAngles = new Vector3(0, rotateY, 0);
				obj1.localScale = collScale;
				//Debug.Log(obj1);
			}
			//Debug.Log(_list.Length);

			//打印碰撞体列表
			if (_list.Length > 0)
			{
				string log = "碰撞列表：";
				for (int i = 0; i < _list.Length; i++)
				{
					log += "\n" + _list[i];
				}
				//Debug.Log(log);
				return true;
			}
			return false;
		}
	}
}
