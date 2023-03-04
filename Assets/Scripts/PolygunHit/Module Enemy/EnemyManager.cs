using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.PolygonHit
{
	/// <summary>
	/// 敌人管理器
	/// </summary>
    public class EnemyManager : MonoBehaviour
    {
        #region Create 生成敌人

        /// <summary>
        /// 用于生成的预制件
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// 敌人生成开关
        /// </summary>
        public static bool isCreate = true;

		/// <summary>
		/// 难度阶梯
		/// </summary>
		private int difficulty = 1;
		/// <summary>
		/// 最大敌人数量
		/// </summary>
		private int diff_MaxCount
		{
			get
			{
				if (difficulty > 15)
					return difficulty * 10;
				return (difficulty + 1) * 6;
			}
		}
		/// <summary>
		/// 每次生成的数量
		/// </summary>
		private int diff_createNum
		{
			get
			{
				return 1 + difficulty / 3;
			}
		}
		private int diff_HP
		{
			get
			{
				return 3 + difficulty * 2;
			}
		}

		/// <summary>
		/// CD上限 随时间减少
		/// </summary>
		public float cdUpper;
        /// <summary>
        /// 生成倒计时
        /// </summary>
        float countDown;

        /// <summary>
        /// 所有场景中的敌人
        /// </summary>
        public static List<GameObject> enemys;

        #endregion

        #region pool
        /// <summary>
        /// 敌人的对象池
        /// </summary>
        private List<GameObject> enemyPool;
        #endregion

        #region unity void

        /// <summary>
        /// 初始化
        /// </summary>
        public void Awake()
        {
            AddEvent();
            isCreate = true;
            enemys = new List<GameObject>();
            enemyPool = new List<GameObject>();
        }

        private void Update()
        {
            if (isCreate)
                IsCreate();
        }

        private void OnDestroy()
        {
            RemoveEvent();
        }
        #endregion

        #region Event
        void AddEvent()
        {
            EventController.AddListener(EventType.PlayerDestory, StopCreate);
            EventController.AddListener(EventType.action_LevelUp, PlayerLevelUp);
		}
        void RemoveEvent()
        {
            EventController.RemoveListener(EventType.PlayerDestory, StopCreate);
            EventController.RemoveListener(EventType.action_LevelUp, PlayerLevelUp);
		}

		void PlayerLevelUp()
		{
			difficulty++;
		}
        #endregion

        #region Create 生成

        /// <summary>
        /// CD倒计时 检测能否生成
        /// </summary>
        private void IsCreate()
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                countDown = cdUpper;
				for (int i = 0; i < diff_createNum; i++)
					if (enemys.Count < diff_MaxCount)
						EnemyCreate();
            }
        }

        /// <summary>
        /// 生成敌人
        /// </summary>
        private void EnemyCreate()
        {
            GameObject newEnemy;

			if (enemyPool.Count > 0)
			{
				newEnemy = enemyPool[0];
				enemyPool.RemoveAt(0);
				newEnemy.transform.position = RandomPoint();
			}
			else
				newEnemy = GameManager.inst.TeaInstantiate(prefab, RandomPoint(), 1, transform);
			//newEnemy = Instantiate(prefab);

			newEnemy.GetComponent<EnemyBase>().BeCreated(diff_HP);

            enemys.Add(newEnemy);
        }

        /// <summary>
        /// 停止生成
        /// </summary>
        public void StopCreate()
        {
            isCreate = false;
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).TryGetComponent(out EnemyBase enemy)) enemy.enabled = false;
        }

        #endregion

        #region Death 死亡

        /// <summary>
        /// 敌人被销毁
        /// </summary>
        public void EnemyDestory(GameObject target,GameObject particle = null)
        {
            // 将敌人从列表中移除
            enemys.Remove(target);
            // 添加到对象池列表
            enemyPool.Add(target);

            target.SetActive(false);
            target.GetComponent<EnemyBase>().BeDestroy();
            // 把死亡的敌人挪到第一个 方便开发时检查(后续可删除)
            target.transform.SetAsFirstSibling();
        }

        #endregion

        /// <summary>
        /// 随机方向
        ///       0 
        ///       
        ///   1       2
        ///     
        ///       3
        /// </summary>
        /// <returns></returns>
        Vector3 RandomPoint()
        {
            int a = Random.Range(0, 4);
            float pointX = 0, pointY = 0;
            switch (a)
            {
                case 0:
                    pointX = Random.Range(CameraController.inst.MaxLeft(), CameraController.inst.MaxRight());
                    pointY = CameraController.inst.MaxUp() + 0.2f;
                    break;
                case 1:
                    pointY = Random.Range(CameraController.inst.MaxUp(), CameraController.inst.MaxDown());
                    pointX = CameraController.inst.MaxLeft() - 0.2f;
                    break;
                case 2:
                    pointY = Random.Range(CameraController.inst.MaxUp(), CameraController.inst.MaxDown());
                    pointX = CameraController.inst.MaxRight() + 0.2f;
                    break;
                case 3:
                    pointX = Random.Range(CameraController.inst.MaxLeft(), CameraController.inst.MaxRight());
                    pointY = CameraController.inst.MaxDown() - 0.2f;
                    break;
                default:
                    break;
            }
            return new Vector3(pointX, pointY, 10);
        }
    }
}   
