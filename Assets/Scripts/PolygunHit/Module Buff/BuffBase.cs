using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.PolygonHit
{
	public class BuffBase
	{
		BuffTarget m_target;
		public virtual void BuffAwake()
		{

		}
		public virtual void IsTime()
		{

		}
		
		/// <summary>
		/// 拖拽时执行
		/// </summary>
		/// <param name="dmg"></param>
		public virtual void IsShoot()
		{

		}
		/// <summary>
		/// 
		/// </summary>
		public virtual void IsMove()
		{

		}
		/// <summary>
		/// 撞击时执行
		/// </summary>
		/// <param name="dmg">原伤害值</param>
		public virtual void IsStrike(ref float dmg)
		{

		}
		/// <summary>
		/// 受到撞击
		/// </summary>
		/// <param name="unDmg">受到伤害值</param>
		public virtual void UnStrike(ref float unDmg)
		{

		}
	}
}
