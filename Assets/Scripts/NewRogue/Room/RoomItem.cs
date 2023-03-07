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
		public List<GameObject> RoomPrefabs;
	}
}
