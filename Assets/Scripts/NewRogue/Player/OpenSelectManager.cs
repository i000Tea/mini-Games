using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class OpenSelectManager : Singleton<OpenSelectManager>
	{
		int nowPlayer;
		[SerializeField]
		Transform showParent;
		[SerializeField]
		Transform playerParent;
		[SerializeField]
		List<GameObject> players;

		[SerializeField]
		List<GameobjectList> playerlists;

		int nowWeapon;
		[SerializeField]
		Transform weaponsParent;
		[SerializeField]
		List<GameObject> weapons;
		private void OnValidate()
		{
			if (showParent)
			{
				if (players.Count == 0)
				{
					players = new List<GameObject>();
					for (int i = 0; i < showParent.childCount; i++)
					{
						if (showParent.GetChild(i).TryGetComponent(out SkinnedMeshRenderer smr))
						{
							players.Add(smr.gameObject);
						}
					}
				}
			}
			if (weaponsParent)
			{
				if (weapons.Count == 0)
				{
					weapons = new List<GameObject>();

					for (int i = 0; i < weaponsParent.childCount; i++)
					{
						if (weaponsParent.GetChild(i).TryGetComponent(out HoldWeaponItem wCtrl))
						{
							if (wCtrl.IsUse)
								weapons.Add(wCtrl.gameObject);
						}
					}
				}

			}
		}

		public int ChangeSelect(int changeNum, bool isWeapon)
		{
			if (isWeapon)
			{
				nowWeapon += changeNum;
				weapons.NumInTheList(ref nowWeapon);

				for (int i = 0; i < weapons.Count; i++)
					weapons[i].SetActive(false);
				weapons[nowWeapon].SetActive(true);
				return nowWeapon + 1;
			}
			else
			{
				nowPlayer += changeNum;
				playerlists.NumInTheList(ref nowPlayer);
				//Debug.Log(nowPlayer);
				for (int i = 0; i < playerlists.Count; i++)
				{
					playerlists[i].SetActive(false);
				}
				playerlists[nowPlayer].SetActive(true);
				return nowPlayer + 1;
			}
		}

		protected void Start()
		{
			GameManager.I.OnGameStart += GameStart;
		}

		void GameStart()
		{
			//获取武器
			HoldWeapons_Control.I.GetWeapon(weapons[nowWeapon].transform.GetSiblingIndex());

			//激活人物身上相应的模型
			var pList = playerlists[nowPlayer];
			for (int i = 0; i < pList.item.Count; i++)
			{
				List<int> childLengthl = new List<int>();
				Transform obj = pList.item[i].transform;
				for (int n = 0; n < 10; n++)
				{
					//Debug.Log(obj);
					childLengthl.Add(obj.GetSiblingIndex());
					obj = obj.parent;
					if (obj == showParent)
						break;
				}
				//Debug.Log(childLengthl.Count);
				var target = playerParent;
				for (int n = childLengthl.Count - 1; n >= 0; n--)
				{
					target = target.GetChild(childLengthl[n]);
				}
				target.gameObject.SetActive(true);
				//Debug.Log(target + " " + target.parent);
			}

			// 关闭自身
			gameObject.SetActive(false);
		}
	}
	[System.Serializable]
	public class GameobjectList
	{
		public string Name;
		public List<GameObject> item;

		public void SetActive(bool value)
		{
			for (int i = 0; i < item.Count; i++)
			{
				item[i].SetActive(value);
			}
		}
	}
}
