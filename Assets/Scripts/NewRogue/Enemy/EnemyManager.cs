using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class EnemyManager : MonoBehaviour
	{
		public static EnemyManager inst;
		public static List<Enemy_Control> enemys;
		public static List<Transform> createPoints;

		/// <summary>
		/// 本层需要生成的总数
		/// </summary>
		public int createNum = 50;

		/// <summary>
		/// 基础等待时间
		/// </summary>
		public float baseWait = 0.25f;
		/// <summary>
		/// 减速后的等待时间
		/// </summary>
		public float slowWait = 2;

		/// <summary>
		/// 减速阈值
		/// </summary>
		public int eSlowNum = 30;
		/// <summary>
		/// 总上限
		/// </summary>
		public int eMax = 50;

		float waitTime
		{
			get
			{
				if (enemys.Count > eMax)
					return -1;
				else if (enemys.Count > eSlowNum)
					return slowWait;
				else
					return baseWait;
			}
		}
		public bool createing;
		public GameObject prefab;

		private void Awake()
		{
			inst = this;
			enemys = new List<Enemy_Control>();
			createPoints = new List<Transform>();
		}
		/// <summary>
		/// 开始生成敌人
		/// </summary>
		/// <returns></returns>
		public IEnumerator StartCreate()
		{
			createing = true;

			float cd = 0;

			yield return new WaitForSeconds(1f);

			// 循环生成总数
			for (int i = 0; i < createNum; i++)
			{
				// 生成
				CreateEnemy();
				while (cd < waitTime)
				{
					cd += Time.deltaTime;
					yield return new WaitForFixedUpdate();
				}
				cd = 0;
			}

			createing = false;
		}
		GameObject CreateEnemy()
		{
			if (createPoints.Count > 0)
			{
				var obj = Instantiate(prefab);
				obj.transform.position = createPoints[0].position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
				obj.transform.SetParent(transform);
				obj.GetComponent<Enemy_Control>().Startsetting();
				enemys.Add(obj.GetComponent<Enemy_Control>());
				return obj;
			}
			return null;
		}
		public Enemy_Control FindEnemy()
		{
			if (enemys.Count <= 0)
				return null;

			Enemy_Control target = null;
			float _long = 9999;

			for (int i = 0; i < enemys.Count; i++)
			{
				if (enemys[i].health > 0 &&
					Vector3.Distance(enemys[i].transform.position, Player_Control.I.PlayerPoint) < _long)
				{
					target = enemys[i];
					_long = Vector3.Distance(enemys[i].transform.position, Player_Control.I.PlayerPoint);
				}
			}
			return target;
		}

		public void EnemyOver(Enemy_Control over)
		{
			enemys.Remove(over);
			if (enemys.Count == 0 && !createing)
			{
				Debug.Log("结束");
			}
		}
	}
}
