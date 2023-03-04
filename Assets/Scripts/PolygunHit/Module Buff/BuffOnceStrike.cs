using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.PolygonHit
{	
	/// <summary>
	/// 单次撞击附加伤害
	/// </summary>
	public class BuffOnceStrike : BuffBase
	{
		public float mult = 1;
		public float mult22;
		public override void BuffAwake()
		{
			base.BuffAwake();
		}

		public override void IsStrike(ref float dmg)
		{
			// 倍率大于1时计算 计算结束后倍率恢复到1
			if (mult > 1)
			{
				dmg *= mult;
				mult = 1;
			}
		}
	}
}
