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
		#region 变量
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
		/// <summary>
		/// 加载环图片
		/// </summary>
		private Image costLoading;
		/// <summary>
		/// 是否加载
		/// </summary>
		private bool LoadingAdd;
		/// <summary>
		/// 加载数量
		/// </summary>
		private int CostNeedNum;
		#endregion

		/// <summary>
		/// 寻路网格
		/// </summary>
		[SerializeField]
		List<GameObject> NavMesh;
		#endregion

		private void Awake()
		{
			if (nextCost)
			{
				costLoading = nextCost.GetChild(1).GetComponent<Image>();
			}
		}
		private void Update()
		{
			if (Camera.main)
				nextCost.transform.rotation = Camera.main.transform.rotation;
			Loading();
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

		void Loading()
		{
			if (Player_Control.I.Keycord >= CostNeedNum)
			{
				if (costLoading.LoadingRim(LoadingAdd))
				{
					Player_Control.I.Keycord -= CostNeedNum;
					OpenDoor();
				}
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
		/// <summary>
		/// 是否使用开门动画
		/// </summary>
		/// <param name="playAnim"></param>
		public void OpenDoor(bool playAnim = true)
		{
			enabled = false;
			// 当碰撞体存在时 直接关闭碰撞
			if (TryGetComponent(out Collider collider))
			{
				collider.enabled = false;
			}

			// 当没有与下一房间链接时 直接返回
			if (!nextRoom)
				return;
			// 当寻路网格存在时
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

			nextRoom.ShowRoom(playAnim);
			nextRoom = null;

			if (playAnim)
			{
				if (TryGetComponent(out Animator anim))
				{
					anim.SetTrigger("Open");
				}
				else
				{
					transform.GetChild(0).DOScale(0, 0.5f).SetEase(Ease.InBack);
				}
				nextCost.DOScale(0, 0.5f).SetEase(Ease.InBack);
			}
			else
			{
				if (TryGetComponent(out Animator anim))
				{
					anim.SetTrigger("Open");
					anim.speed = 9999;
				}
				else
				{
					transform.GetChild(0).localScale = Vector3.zero;
				}
				nextCost.transform.localScale = Vector3.zero;
			}
			LocalNavMeshBuilder_Change.inst.UpdateTime(1f);
		}
	}
}
