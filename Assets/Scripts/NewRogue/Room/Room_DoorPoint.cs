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
		/// 此门的类型
		/// </summary>
		public DoorType dType;
		/// <summary>
		/// 此点是否已被使用
		/// </summary>
		public bool unUse;
		public Room_Control nextRoom;
		public Transform nextCost;
		[SerializeField]
		List<GameObject> NavMesh;
		private void Update()
		{
			nextCost.transform.rotation = Camera.main.transform.rotation;
		}
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "Player" && nextRoom)
			{
				OpenDoor();
			}
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && nextRoom)
			{
				OpenDoor();
			}
		}

		/// <summary>
		/// 设置房间链接
		/// </summary>
		/// <param name="nR"></param>
		public void SetRoomLink(Room_Control nR)
		{
			unUse = true;
			nextRoom = nR;
			nextCost.gameObject.SetActive(true);
		}
		/// <summary>
		/// 本门作为被开启方向 关闭自身
		/// </summary>
		public void CloseMe()
		{
			unUse = true;
			gameObject.SetActive(false);
		}
		public void OpenDoor()
		{
			if (!nextRoom)
				return;
			if (NavMesh!=null)
			{
				for (int i = 0; i < NavMesh.Count; i++)
				{
					NavMesh[i].GetComponent<NavMeshSourceTag>().enabled = false;
					NavMesh[i].GetComponent<Collider>().enabled = false;
				}
			}
			if (TryGetComponent(out Collider collider))
			{
				collider.isTrigger = true;
			}
			nextRoom.ShowRoom();
			nextRoom = null;
			if (TryGetComponent(out Animator anim))
			{
				anim.SetTrigger("Open");
			}
			else
			{
				transform.GetChild(0).DOScale(0, 0.5f).SetEase(Ease.InBack);
			}
			nextCost.DOScale(0, 0.5f).SetEase(Ease.InBack);
			LocalNavMeshBuilder_Change.inst.UpdateTime(1f);
		}
	}
}
