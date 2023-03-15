using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Tea.NewRouge
{

	[CreateAssetMenu(menuName = "Game02/房间预制件", fileName = "房间预制件")]
	[Serializable]
	public class RoomItem : ScriptableObject
	{
		public List<GameObject> EnemyPrefabs;
		public List<GameObject> BasePrefabs;
		public List<WeightRoom> BasePrefabssss;
		/// <summary>
		/// 从列表中 随机一间 有相同空置门的 房间
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="Seed"></param>
		/// <returns></returns>
		public GameObject RandomRoom(RoomPrefabs whichlist, DoorType dt, int Seed = -1)
		{
			List<GameObject> roomList;
			switch (whichlist)
			{
				case RoomPrefabs.BaseRoom:
					roomList = BasePrefabs;
					break;
				case RoomPrefabs.EnemyCreate:
					roomList = EnemyPrefabs;
					break;
				default:
					return null;
			}
			if (Seed <= 0)
			{
				Seed = UnityEngine.Random.Range(0, roomList.Count);
			}
			// 查询是否有类型一致的房间
			for (int i = 0; i < roomList.Count; i++)
			{
				if (Seed >= roomList.Count)
					Seed = 0;
				if (roomList[Seed].GetComponent<Room_Control>().FindDoorType(dt))
				{
					//Debug.Log($"seed为{Seed}时可以使用");
					return roomList[Seed];
				}
				else
					Seed++;
				//Debug.Log($"seed为{Seed-1}时不行");
			}
			return null;
		}
	}
	public enum DoorType
	{
		_1Door,
		_2Door,
	}
	public enum RoomPrefabs
	{
		BaseRoom,
		EnemyCreate,
	}
	[Serializable]
	public class WeightRoom
	{
		public int Weight;
		public GameObject Prefab;
	}

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(WeightRoom))]
	public class Drawer_WeightRoom : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var WeightRect = new Rect(position)
			{
				height = position.height * 0.95f,
				width = 60,
				x = position.x
			};
			var PrefabRect = new Rect(WeightRect)
			{
				width = position.width - 70,
				x = position.width / 3 + 60,
			};

			//设置属性名宽度
			EditorGUIUtility.labelWidth = 30;
			EditorGUI.PropertyField(WeightRect, property.FindPropertyRelative("Weight"));
			EditorGUI.PropertyField(PrefabRect, property.FindPropertyRelative("Prefab"), new GUIContent("ID"));
		}
	}
#endif
}
