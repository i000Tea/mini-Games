using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Tea.NewRouge
{
	/// <summary>
	/// 开门的点
	/// </summary>
	public class Room_DoorPoint : MonoBehaviour
	{
		/// <summary>
		/// 此点是否已被使用
		/// </summary>
		public bool unUse;
		public Room_Control nextRoom;
		public Transform nextCost;

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "Player" && nextRoom)
			{
				ShowRoom();
			}
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && nextRoom)
			{
				ShowRoom();
			}
		}
		
		public void SetRoomLink(Room_Control nR = null)
		{
			if (nR)
			{
				nextRoom = nR;
				nextCost.gameObject.SetActive(true);
			}
			else
				gameObject.SetActive(false);
			unUse = true;
		}
		void ShowRoom()
		{
			if (TryGetComponent(out Collider collider))
			{
				collider.isTrigger = true;
			}
			nextRoom.ShowRoom();
			nextRoom = null;
			transform.GetChild(0).DOScale(0, 0.5f).SetEase(Ease.InBack);
			nextCost.DOScale(0, 0.5f).SetEase(Ease.InBack);
		}
		
	}
}
