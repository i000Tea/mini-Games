using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea {
	public class TestFPS :Singleton<TestFPS>
	{
		[SerializeField]
		private Text fps;
		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(this);
		}
		private IEnumerator Start()
		{
			while (true)
			{
				yield return new WaitForSeconds(0.25f);
				fps.text = (1 / Time.deltaTime).ToString();
			}
		}
	}
}
