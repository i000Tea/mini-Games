using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.NewRouge
{
	public class GUI_Control : MonoBehaviour
	{
		[SerializeField]
		private Text GetKeycard;
		public void UpdateKeycord(int num)
		{
			GetKeycard.text = num.ToString();
		}
	}
}
