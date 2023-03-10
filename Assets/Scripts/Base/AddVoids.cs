using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Tea.PolygonHit;

namespace Tea
{
    public static class AddVoids
    {
        /// <summary>
        /// 图像在一段时间内闪烁一次
        /// </summary>
        /// <param name="targetImage"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        public static void ColorFlicker(this Image targetImage, Color Start, Color End,
            float flickerTime = 0.3f)
        {
            targetImage.DOColor(Start, flickerTime / 2).OnComplete(() => targetImage.DOColor(End, flickerTime / 2));
        }

		/// <summary>
		/// 提取列表与目标点距离最小的一个物体
		/// </summary>
		/// <param name="_List"></param>
		/// <param name="target"></param>
		/// <param name="_long">可传入最大范围</param>
		/// <returns></returns>
		public static GameObject ListMin(List<GameObject> _List, Vector3 target, float _long = 999999)
		{
			// 新建物体和最大长度
			GameObject targetObj = null;
			float longTarget;
			// 循环
			for (int i = 0; i < _List.Count; i++)
			{
				longTarget = Vector3.Distance(_List[i].transform.position, target);
				if (longTarget < _long)
				{
					targetObj = _List[i];
					_long = longTarget;
				}
			}
			// 返回物体
			return targetObj;
		}


		/// <summary>
		/// 提取列表中与目标小于一定距离的所有物体
		/// </summary>
		/// <param name="_List"></param>
		/// <param name="traget"></param>
		/// <param name="_long"></param>
		/// <returns></returns>
		public static GameObject[] ListDistance(List<GameObject> _List,Vector3 target, float _long)
        {
			// 新建数列
            var newList = new List<GameObject>();
			// 循环添加
            for (int i = 0; i < _List.Count; i++)
                if (Vector3.Distance(_List[i].transform.position, target) < _long)
                    newList.Add(_List[i]);
			// 返回数组
            return newList.ToArray();
        }

		/// <summary>
		/// 仅传入了撞击
		/// </summary>
		/// <param name="enemys"></param>
		/// <param name="unC"></param>
		public static void AtkEnemys(this GameObject[] enemys,UnCollision unC)
		{
			AtkEnemys(enemys, 0, unC);
		}
		/// <summary>
		/// 仅传入了伤害
		/// </summary>
		/// <param name="enemys"></param>
		/// <param name="Hit"></param>
		public static void AtkEnemys(this GameObject[] enemys, float Hit)
		{
			UnCollision unC = new UnCollision();
			AtkEnemys(enemys, Hit,unC);
		}

		/// <summary>
		/// 对列表中的敌人进行aoe攻击
		/// </summary>
		/// <param name="enemys"></param>
		/// <param name="Hit"></param>
		/// <param name="Power"></param>
		/// <param name="Point"></param>
		/// <param name="dizzTime"></param>
		public static void AtkEnemys(this GameObject[] enemys, 
			float Hit, 
			UnCollision UnC)
		{
			EnemyBase a;
			for (int i = 0; i < enemys.Length; i++)
			{
				a = enemys[i].GetComponent<EnemyBase>();
				if (Hit > 0)
					a.UnHit(Hit);
				if (UnC.Power > 0)
					a.UnCollision(UnC);
			}
		}

		// 
		public static UnCollision SetUnColl(float Power, Vector3 Target)
		{
			UnCollision data = new UnCollision();
			data.Power = Power;
			data.Target = Target;
			return data;
		}

		/// <summary>
		/// 随机获取枚举值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T RandomEnum<T>()
		{
			T[] results = Enum.GetValues(typeof(T)) as T[];
			T result = results[UnityEngine.Random.Range(0, results.Length)];
			return result;
		}


		/// <summary>
		/// 经过角度转换后的V3
		/// </summary>
		/// <param name="beforePoint"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Vector3 AngleTransfor(Vector3 beforePoint, float angle)
		{
			Vector3 a = new Vector3(
				beforePoint.x * Mathf.Cos(angle * Mathf.Deg2Rad) +
				beforePoint.z * Mathf.Sin(angle * Mathf.Deg2Rad),
				0,
				beforePoint.x * -Mathf.Sin(angle * Mathf.Deg2Rad) +
				beforePoint.z * Mathf.Cos(angle * Mathf.Deg2Rad)
				);

			return a;
		}
	}
}
/// <summary>
/// 受撞击的数据(撞击力度和撞击点)
/// </summary>
[System.Serializable]
public class UnCollision
{
	public float Power;
	[HideInInspector]
	public Vector3 Target;
}
