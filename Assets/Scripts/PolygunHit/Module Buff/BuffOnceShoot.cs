using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.PolygonHit
{	
	/// <summary>
	/// 单次弹射后所有撞击事件附加伤害
	/// </summary>
	public class BuffOnceShoot : BuffBase
	{
		/// <summary>
		/// 是否使用过的标记
		/// </summary>
		bool useMult;
		public float mult = 1;
		public float mult22;

		public override void BuffAwake()
		{
			base.BuffAwake();
		}

		public override void IsShoot()
		{
			// 当拖拽时 未使用过技能 且倍率不为默认 标记为使用过
			if (!useMult && mult > 1)
			{
				useMult = true;
			}
			// 当使用过本技能时 算法还原 标记为未使用
			else
			{
				mult = 1;
				useMult = false;
			}
		}
		public override void IsStrike(ref float dmg)
		{
			// 倍率大于1时计算
			if (mult > 1)
			{
				dmg *= mult;
			}
		}
	}
}
