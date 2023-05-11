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
		#region 变量

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
		private int Challenge = 1;

		/// <summary>
		/// 基础最大数量
		/// </summary>
		[SerializeField]
		private int baseMaxCount;
		/// <summary>
		/// 最大敌人数量
		/// </summary>
		public int EnemyMaxLimit
		{
			get
			{
				if (Challenge > 15)
					return Challenge * 10;
				return (Challenge + 1) * 6;
			}
		}
		/// <summary>
		/// 每次生成的数量
		/// </summary>
		private int diff_createNum
		{
			get
			{
				return 1 + Challenge / 3;
			}
		}
		private int diff_HP
		{
			get
			{
				return 3 + Challenge * 2;
			}
		}

		public bool CreateReady => createCD <= 0;
		/// <summary>
		/// 生成倒计时
		/// </summary>
		public float CreateCD
		{
			get => createCD;
			set
			{
				// 数值小于0 纠正为0
				if (value < 0)
				{
					createCD = 0;
				}
				// 数值大于上限 纠正为上限
				else if (value > cdUpperLimit)
				{
					createCD = cdUpperLimit;
				}
				else
				{
					createCD = value;
				}
			}
		}
		private float createCD;
		/// <summary>
		/// CD上限 随时间减少
		/// </summary>
		public float cdUpperLimit;
		/// <summary>
		/// 所有场景中的敌人
		/// </summary>
		public static List<GameObject> nowEnemys;

		#endregion

		#region pool
		/// <summary>
		/// 敌人的对象池
		/// </summary>
		private List<GameObject> dieEnemyPool;
		#endregion

		#endregion

		#region unity void

		/// <summary>
		/// 初始化
		/// </summary>
		public void Awake()
		{
			AddEvent();
			isCreate = true;
			nowEnemys = new List<GameObject>();
			dieEnemyPool = new List<GameObject>();
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
			Challenge++;
		}
		#endregion

		#region Create 生成

		/// <summary>
		/// CD倒计时 检测能否生成
		/// </summary>
		private void IsCreate()
		{
			createCD -= Time.deltaTime;
			if (createCD <= 0)
			{
				createCD = cdUpperLimit;
				for (int i = 0; i < diff_createNum; i++)
					if (nowEnemys.Count < EnemyMaxLimit)
						TryCreateEnemy();
			}
		}

		/// <summary>
		/// 尝试生成敌人
		/// </summary>
		private bool TryCreateEnemy()
		{
			// 若敌人
			if (nowEnemys.Count >= EnemyMaxLimit)
			{
				return false;
			}
			GameObject newEnemy;

			// 尝试从对象池中生成一个敌人

			// 否则 创建一个敌人

			if (dieEnemyPool.Count > 0)
			{
				newEnemy = dieEnemyPool[0];
				dieEnemyPool.RemoveAt(0);
				newEnemy.transform.position = RandomPoint();
			}
			else
				newEnemy = GameManager.inst.TeaInstantiate(prefab, RandomPoint(), 1, transform);
			//newEnemy = Instantiate(prefab);

			newEnemy.GetComponent<EnemyBase>().BeCreated(diff_HP);

			nowEnemys.Add(newEnemy);
			return true;
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
		#endregion

		#region Death 死亡

		/// <summary>
		/// 敌人被销毁
		/// </summary>
		public void EnemyDestory(GameObject target, GameObject particle = null)
		{
			// 将敌人从列表中移除
			nowEnemys.Remove(target);
			// 添加到对象池列表
			dieEnemyPool.Add(target);

			target.SetActive(false);
			target.GetComponent<EnemyBase>().BeDestroy();
			// 把死亡的敌人挪到第一个 方便开发时检查(后续可删除)
			target.transform.SetAsFirstSibling();
		}

		#endregion

	}
}
