using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Tea.PolygonHit
{
	/// <summary>
	/// GUI的特殊系统类
	/// </summary>
	public class GUISpecialSystems : MonoBehaviour
	{
		public static GUISpecialSystems inst;
		[SerializeField]
		RectTransform setAmmo;
		[SerializeField]
		Text AmmoText;

		Vector3 ammPosition;
		bool useAmmo;
		private void Awake()
		{
			inst = this;
		}
		/// <summary>
		/// 特殊系统的初始化
		/// </summary>
		public void SpecialAwake()
		{

		}
		/// <summary>
		/// 弹药系统
		/// </summary>
		public void Ammo(int ammo,int max)
		{
			if (!useAmmo)
			{
				StartCoroutine(RectOpen(setAmmo));
				useAmmo = true;
			}

			AmmoText.text = ammo + "/" + max;
		}

		IEnumerator RectOpen(RectTransform target)
		{
			//Debug.Log("RectOpen");
			yield return 0;

			int a = target.parent.GetSiblingIndex()+1;
			int b = target.GetSiblingIndex();

			target.DOLocalMove(target.parent.parent.GetChild(a).GetChild(b).localPosition, 1);
		}
	}
}
