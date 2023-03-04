using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.PolygonHit {
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager inst;

		public GUISpecialSystems specialSystems;

		#region Show UI
		[SerializeField]
        GameObject[] showCanvas;

        [SerializeField]
        private Image exaImage;

        [SerializeField]
        private Text levelText;

        [SerializeField]
        private Text scoreText;

        [SerializeField]
        private Text HealthText;
		#endregion

		private void Awake()
        {
            inst = this;
			if(TryGetComponent(out GUISpecialSystems special))
			{
				specialSystems = special;
				specialSystems.SpecialAwake();
			}
			
		}

        #region Calculation ui更新

		/// <summary>
		/// 玩家界面UI更新
		/// </summary>
		/// <param name="level"></param>
		/// <param name="now"></param>
		/// <param name="max"></param>
		/// <param name="health"></param>
        public void PlayerMessageUpdate(int level, int now, int max, int health)
        {
            exaImage.fillAmount = (float)now / (float)max;
            levelText.text = level.ToString();
            HealthText.text = health.ToString();
        }

        /// <summary>
        /// 显示更新后的分数
        /// </summary>
        /// <param name="score"></param>
        public void CalculationScore(int score)
        {
            scoreText.text = score.ToString();
        }


        /// <summary>
        /// 切换Canvas显示
        /// </summary>
        /// <param name="state"></param>
        public void CanvasSwitch(GameState state)
        {
            for (int i = 1; i < showCanvas.Length; i++)
                showCanvas[i].SetActive(false);

            showCanvas[(int)state].SetActive(true);
        }

		#endregion

		#region SpecialSystems 特殊系统

		#endregion
	}
}
