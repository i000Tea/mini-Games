using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Tea.PolygonHit;
using Tea.NewRouge;

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

		#region List
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
		public static GameObject[] ListDistance(List<GameObject> _List, Vector3 target, float _long)
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
		/// 生成子物体集
		/// </summary>
		/// <returns></returns>
		public static void CreateChildList(this Transform parent, ref List<GameObject> _list)
		{
			if (_list == null || _list.Count != parent.childCount)
			{
				_list = new List<GameObject>();

				for (int i = 0; i < parent.childCount; i++)
				{
					_list.Add(parent.GetChild(i).gameObject);
				}
			}
		}
		#endregion

		#region Game1
		/// <summary>
		/// 仅传入了撞击
		/// </summary>
		/// <param name="enemys"></param>
		/// <param name="unC"></param>
		public static void AtkEnemys(this GameObject[] enemys, UnCollision unC)
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
			AtkEnemys(enemys, Hit, unC);
		}

		/// <summary>
		/// 对列表中的敌人进行aoe攻击
		/// </summary>
		/// <param name="enemys"></param>
		/// <param name="Hit"></param>
		/// <param name="Power"></param>
		/// <param name="Point"></param>
		/// <param name="dizzTime"></param>
		public static void AtkEnemys(this GameObject[] enemys, float Hit, UnCollision UnC)
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


		#endregion

		#region Game2
		/// <summary>
		/// 实例化子弹
		/// </summary>
		/// <param name="prefab">子弹预制件</param>
		/// <param name="muzzle">枪口位置</param>
		/// <param name="Damage">伤害值</param>
		/// <param name="HorizOffset">横向偏移</param>
		/// <param name="vertiOffset">纵向偏移</param>
		/// <param name="lifeTime">最大生命时间</param>
		/// <returns></returns>
		public static GameObject InstantiateBullet(this GameObject prefab, Transform muzzle, float Damage = 1,
			float velocity = 1,
			float HorizOffset = 0.05f, float vertiOffset = 0.05f, float Scale = 1, float lifeTime = 3)
		{
			var entity = GameObject.Instantiate(prefab, muzzle.position, muzzle.rotation);
			entity.transform.localScale = Vector3.one * Scale;
			//Debug.Log(muzzle.position);
			//Debug.Log(entity.transform.position);
			var bullet = entity.GetComponent<Bullet>();
			bullet.SetDamage(Damage);

			entity.GetComponent<Rigidbody>().velocity = (muzzle.transform.forward + new Vector3(
				UnityEngine.Random.Range(-HorizOffset, HorizOffset), 0,
				UnityEngine.Random.Range(-vertiOffset, vertiOffset))) * 16* velocity;

			GameObject.Destroy(entity, lifeTime);
			return entity;
		}


		#endregion

		/// <summary>
		/// 加载环
		/// </summary>
		/// <param name="targetImage">图片</param>
		/// <param name="isAdd">是否加载</param>
		/// <returns>加载完成</returns>
		public static bool LoadingRim(this Image targetImage,bool isAdd)
		{
			if (targetImage.fillAmount == 1)
				return true;
			if (isAdd)
				targetImage.fillAmount += Time.deltaTime*PlayerMove.inst.RimRate;
			else
				targetImage.fillAmount -= Time.deltaTime;

			return false;
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
