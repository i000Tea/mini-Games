using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.NewRouge
{
	public class RoomPoint : MonoBehaviour
	{
		public RoomControl nextRoom;
		Collider trggerColl;
		private void Awake()
		{
			if (TryGetComponent(out Collider coll))
			{
				trggerColl = coll;
			}
			else
				trggerColl = gameObject.AddComponent<BoxCollider>();

			if (nextRoom)
			{
				nextRoom.RoomState = false;
				trggerColl.isTrigger = true;
			}
			else
			{
				trggerColl.isTrigger = false;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && nextRoom)
			{
				nextRoom.RoomState = true;
			}
		}
	}
}
