using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{

	[CreateAssetMenu(menuName = "Game02/房间预制件", fileName = "房间预制件")]
	[Serializable]
	public class RoomItem : ScriptableObject
	{
		public List<GameObject> DoorPrefabs;
		/// <summary>
		/// 从列表中 随机一间 有相同门型的 房间
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="Seed"></param>
		/// <returns></returns>
		public GameObject RandomRoom(DoorType dt, int Seed = -1)
		{
			if (Seed <= 0)
			{
				Seed = UnityEngine.Random.Range(0, DoorPrefabs.Count);
			}
			// 查询是否有类型一致的房间
			for (int i = 0; i < DoorPrefabs.Count; i++)
			{
				if (DoorPrefabs[Seed].GetComponent<Room_Control>().FindDoorType(dt))
					return DoorPrefabs[Seed];
				else
					Seed++;
				if (Seed >= DoorPrefabs.Count - 1)
					Seed = 0;
			}
			return null;
		}
	}
	public enum DoorType
	{
		_1Door,
		_2Door,
	}
}
