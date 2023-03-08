using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class EnemyManager : MonoBehaviour
	{
		public static EnemyManager inst;
		public static List<Enemy_Control> enemys;
		public bool createing;
		public GameObject prefab;

		private void Awake()
		{
			inst = this;
			enemys = new List<Enemy_Control>();
		}
		IEnumerator Start()
		{
			createing = true;
			for (int i = 0; i < 20; i++)
			{
				yield return new WaitForFixedUpdate();
				CreateEnemy();
			}

			for (int i = 0; i < 3; i++)
			{
				yield return new WaitForSeconds(10);
				for (int n = 0; n < 10; n++)
				{
					yield return new WaitForFixedUpdate();
					CreateEnemy();
				}
			}
			createing = false;
		}
		GameObject CreateEnemy()
		{
			var obj = Instantiate(prefab);
			obj.transform.position = transform.position;
			obj.transform.SetParent(transform);
			obj.GetComponent<Enemy_Control>().Startsetting();
			enemys.Add(obj.GetComponent<Enemy_Control>());
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
					Vector3.Distance(enemys[i].transform.position, Player_Control.PlayerPoint) < _long)
				{
					target = enemys[i];
					_long = Vector3.Distance(enemys[i].transform.position, Player_Control.PlayerPoint);
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
