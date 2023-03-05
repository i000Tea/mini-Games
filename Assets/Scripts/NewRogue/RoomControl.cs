using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Tea.NewRouge
{
	public class RoomControl : MonoBehaviour
	{
		public bool RoomState
		{
			get
			{
				return (bool)roomState;
			}
			set
			{
				//Debug.Log(value + " " + roomState);
				//Debug.Log(8844);
				if (roomState != value)
				{
					roomState = value;
					if (roomState == true)
					{
						transform.DOScale(1, 0.7f).SetEase(Ease.OutCirc);
					}
					else
					{
						transform.localScale = Vector3.zero;
					}
				}
			}
		}
		private bool? roomState = null;
		[SerializeField]
		private List<RoomPoint> rPoints;
		private void OnValidate()
		{
			if (rPoints == null || rPoints.Count != transform.GetChild(0).childCount)
			{
				rPoints = new List<RoomPoint>();
				for (int i = 0; i < transform.GetChild(0).childCount; i++)
				{
					rPoints.Add(transform.GetChild(0).GetChild(i).GetComponent<RoomPoint>());
				}
			}
		}
		private void Awake()
		{

		}
	}
}
