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
		private Transform roomParent;
		private GameObject EnvParent;
		private void OnValidate()
		{
			if (myDoors == null || myDoors.Count != transform.GetChild(0).childCount)
			{
				myDoors = new List<Room_DoorPoint>();
				for (int i = 0; i < transform.GetChild(0).childCount; i++)
				{
					myDoors.Add(transform.GetChild(0).GetChild(i).GetComponent<Room_DoorPoint>());
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
			for (int i = 0; i < myDoors.Count; i++)
			{
				if (!myDoors[i].nextRoom)
					myDoors[i].nextCost.gameObject.SetActive(false);
			}
		}
		public void ShowRoom()
		{
			gameObject.SetActive(true);
			transform.localScale = Vector3.zero;
			transform.DOScale(1, 0.7f).SetEase(Ease.OutCirc);
		}

		/// <summary>
		/// 查找房间
		/// </summary>
		/// <param name="dType"></param>
		/// <returns> 是否有类型一致的房间 </returns>
		public bool FindDoorType(DoorType dType)
		{
			for (int i = 0; i < myDoors.Count; i++)
			{
				if (myDoors[i].dType == dType)
					return true;
			}
			return false;
		}

		/// <summary>
		/// 获取一扇门 
		/// </summary>
		/// <param name="num"> 随机门的序号 </param>
		/// <param name="dType"> (检测新门匹配时使用) 门的类型 </param>
		/// <returns>返回随机好的一扇门</returns>
		public Room_DoorPoint GetDoor(DoorType? dType = null, int num = -1)
		{
			// 当传入-1 视为随机
			if (num < 0)
			{
				num = Random.Range(0, myDoors.Count - 1);
			}
			// 查找门点
			for (int i = 0; i < myDoors.Count; i++)
			{
				Debug.Log($"{name} \n 此门是否被使用 {myDoors[num].unUse} " +
					$"\n  门类型存在时  (新门) 类型与自身一致 {dType != null && dType != myDoors[num].dType} " +
					$"\n 门类型不存在时(原门) 碰撞检测 {dType == null && DetectionDoor(myDoors[num])}");
				// 检测门是否可用
				if (
					myDoors[num].unUse ||                               // 此门是否被使用
					(dType != null && dType != myDoors[num].dType) ||   // 门类型存在时  (新门) 类型与自身一致
					(dType == null && DetectionDoor(myDoors[num])))     // 门类型不存在时(原门) 碰撞检测
				{
					Debug.Log("跳过");
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
					Debug.Log(myDoors[num]);
					return myDoors[num];
				}
			}
			return null;
		}

		//const Vector3 targetScale = new Vector3
		//{
		//	x=1,
		//	y=1,
		//	z=1,
		//}

		/// <summary>
		/// 检测此门后碰撞体
		/// </summary>
		/// <param name="dPoint"></param>
		/// <returns> 碰撞体是否存在 </returns>
		bool DetectionDoor(Room_DoorPoint dPoint, Vector3? targetScale = null)
		{
			Vector3 targetPosi = (dPoint.transform.position + dPoint.transform.forward * 12);
			if (targetScale == null)
				targetScale = new Vector3(8, 4, 8);

			int layer = 1 << 6;
			var _list = Physics.OverlapBox(targetPosi, (Vector3)targetScale, dPoint.transform.rotation, layer);

			
			if (_list.Length > 0)
				return true;

			//Transform obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			//obj1.position = targetPosi;
			//obj1.rotation = dPoint.transform.rotation;
			//obj1.localScale = (Vector3)targetScale * 2;


			return false;
		}
	}
}
