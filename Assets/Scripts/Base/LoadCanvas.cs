using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Tea
{
    public class LoadCanvas : Singleton<LoadCanvas>
    {
		RectTransform rect;
		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(gameObject);
			rect = (transform.GetChild(0).transform as RectTransform);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				rect.DOAnchorPosX(-rect.sizeDelta.x,1);
			}
		}
	}
}
