using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
		/// <summary>
		/// 门后下一个房间
		/// </summary>
		public Room_Control nextRoom;

		#region cost
		public Transform nextCost;
		private Image costLoading;
		private bool LoadingAdd;
		private int CostNeedNum;
		#endregion


		[SerializeField]
		List<GameObject> NavMesh;

		private void Awake()
		{
			if (nextCost)
			{
				costLoading = nextCost.GetChild(1).GetComponent<Image>();
			}
		}
		private void Update()
		{
			if (costLoading.LoadingRim(LoadingAdd))
			{
				this.enabled = false;
				OpenDoor();
			}
			else
			{
				nextCost.transform.rotation = Camera.main.transform.rotation;
			}
		}
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag == "Player" && nextRoom)
			{
				LoadingAdd = true;
			}
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.tag == "Player" && nextRoom)
			{
				LoadingAdd = false;
			}
		}

		/// <summary>
		/// 链接下一个房间
		/// </summary>
		/// <param name="nR"></param>
		public void LinkNextRoom(Room_Control nR, int Cost)
		{
			unUse = true;
			nextRoom = nR;
			nextCost.gameObject.SetActive(true);

			CostNeedNum = Cost;
			if (nextCost)
				nextCost.GetChild(2).GetComponent<Text>().text = Cost.ToString();
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
			if (NavMesh != null)
			{
				for (int i = 0; i < NavMesh.Count; i++)
				{
					if (NavMesh[i].TryGetComponent(out NavMeshSourceTag tag))
						tag.enabled = false;
					if (NavMesh[i].TryGetComponent(out Room_FloorAndWall faw))
						faw.enabled = false;
					//NavMesh[i].GetComponent<Room_FloorAndWall>().enabled = false;
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
