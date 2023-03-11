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
				for (int i = 0; i < myDoors.Count; i++)
					if (!myDoors[i].unUse)
						return false;
				// 所有点都标记为使用 则更新为是
				return true;
			}
		}
		/// <summary>
		/// 自己身上门的列表
		/// </summary>
		[SerializeField]
		public List<Room_DoorPoint> myDoors;

		[SerializeField]
		private Transform wallParent, floorParent;
		[SerializeField]
		private List<GameObject> walls, floors;

		private GameObject EnvParent;

		public Transform roomColl;

		private void OnValidate()
		{
			//Debug.Log(roomColl.lossyScale);
			if (myDoors == null || myDoors.Count != transform.GetChild(0).childCount)
			{
				myDoors = new List<Room_DoorPoint>();
				for (int i = 0; i < transform.GetChild(0).childCount; i++)
				{
					myDoors.Add(transform.GetChild(0).GetChild(i).GetComponent<Room_DoorPoint>());
				}
			}
			
			if (wallParent)
			{

				wallParent.CreateChildList(ref walls);

				for (int i = 0; i < walls.Count; i++)	
				{
					if (!walls[i].TryGetComponent(out NavMeshSourceTag _))
						walls[i].gameObject.AddComponent<NavMeshSourceTag>();
				}

			}
			if (floorParent)
			{
				floorParent.CreateChildList(ref floors);

				for (int i = 0; i < floors.Count; i++)
				{
					if (!floors[i].TryGetComponent(out NavMeshSourceTag _))
						floors[i].gameObject.AddComponent<NavMeshSourceTag>();
				}
			}

		}
		private void Awake()
		{

		}

		/// <summary>
		/// 初始化设置
		/// </summary>
		public virtual void RoomAwakeSet(bool roomState = false)
		{
			if (!roomState)
			{
				gameObject.SetActive(false);
				transform.localScale = Vector3.zero;
			}
			for (int i = 0; i < myDoors.Count; i++)
			{
				if (!myDoors[i].nextRoom)
				{
					myDoors[i].nextCost.gameObject.SetActive(false);
				}
			}
		}
		public void ShowRoom()
		{
			gameObject.SetActive(true);
			transform.localScale = Vector3.zero;
			transform.DOScale(1, 0.7f).SetEase(Ease.OutCirc);
		}

		/// <summary>
		/// 查找 空置门类型
		/// </summary>
		/// <param name="dType"></param>
		/// <returns> 是否有类型一致的房间 </returns>
		public bool FindDoorType(DoorType dType)
		{
			for (int i = 0; i < myDoors.Count; i++)
			{
				//Debug.Log($"房间名{name}检测门类型{dType} 当前门名称{myDoors[i].gameObject.name} 当前门类型{myDoors[i].dType}  是否使用{myDoors[i].unUse}");
				if (myDoors[i].dType == dType && !myDoors[i].unUse)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 获取一扇未使用过的门 
		/// </summary>
		/// <param name="num"> 随机门的序号 </param>
		/// <param name="dType"> (检测新门匹配时使用) 门的类型 </param>
		/// <returns>返回随机好的一扇门</returns>
		public Room_DoorPoint GetDoor(DoorType dType, int num = -1)
		{
			// 当传入-1 视为随机
			if (num < 0)
			{
				num = Random.Range(0, myDoors.Count - 1);
			}
			// 查找门点
			for (int i = 0; i < myDoors.Count; i++)
			{
				//Debug.Log($"{name} \n 此门是否被使用 {myDoors[num].unUse} " +
				//	$"\n  门类型存在时  (新门) 类型与自身一致 {dType != null && dType != myDoors[num].dType} " +
				//	$"\n 门类型不存在时(原门) 碰撞检测 {dType == null && DetectionDoor(myDoors[num])}");
				// 检测门是否可用
				if (myDoors[num].unUse ||                           // 此门是否被使用
					dType != myDoors[num].dType)                  // 门类型存在时  (新门) 类型与自身一致
				{
					//Debug.Log("序号加1 再次检索");
					// 若不可用 序号加1 再次检索
					num++;
					if (num >= myDoors.Count)
					{
						num = 0;
					}
				}
				// 若没有被使用 返回该门点
				else
				{
					//Debug.Log(myDoors[num]);
					return myDoors[num];
				}
			}
			return null;
		}

		public int GetDoorNum(Room_DoorPoint door)
		{
			for (int i = 0; i < myDoors.Count; i++)
			{
				if (myDoors[i] == door)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
